using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Characters;
using static WvsBeta.MasterThread;

namespace WvsBeta.Game.Events.GMEvents
{
    public enum EventType
    {
        Ola = 1,
        Fitness,
        OX,
        Coconut,
        Snowball,
        Jewel,
    }
    /*
    2464;"/usercount"
    2465;"Total:%d Maple:%d Story:%d"
    2468;"The game between Team Maple and Team Story have resulted in a tie."
    2469;" Team Maple WINS!"
    2470;" Team Story WINS!"
    2472;"The game of SnowBall has ended, and you'll be transported to a different map. Please wait."
     */
    public abstract class EventFieldSet : FieldSet
    {
        public static IEnumerable<EventFieldSet> Events => Instances.Values.OfType<EventFieldSet>();
        public static EventFieldSet CurrentEvent { get; private set; }

        public bool IsEnabled { get; protected set; }
        protected static readonly Map WinMap = GameDataProvider.Maps[109050001];
        public Map Lobby => Maps[0];
        protected int LoseMapId => ReturnMap;


        public int Capacity { get; }
        public EventType Type { get; }
        protected EventFieldSet(ConfigReader.Node fsNode) : base(fsNode)
        {
            Capacity = fsNode["capacity"].GetInt();
            Type = (EventType)fsNode["type"].GetInt();
        }

        public static void Create(ConfigReader.Node fsNode)
        {
            var type = (EventType)fsNode["type"].GetInt();
            switch (type)
            {
                case EventType.Ola:
                    new MapleOlaEvent(fsNode);
                    break;
                case EventType.Fitness:
                    new MapleFitnessEvent(fsNode);
                    break;
                case EventType.OX:
                    new MapleOXEvent(fsNode);
                    break;
                case EventType.Coconut:
                    new MapleCoconutEvent(fsNode);
                    break;
                case EventType.Snowball:
                    new MapleSnowballEvent(fsNode);
                    break;
                case EventType.Jewel:
                    new MapleJewelEvent(fsNode);
                    break;
            }
        }

        public virtual void Enable()
        {
            IsEnabled = true;

            Maps.ForEach(m => m.PortalsOpen = false);
            ChatPacket.SendBroadcastMessageToAllPlayers("The event is now open. Please click the Event NPC to enter the Event Map.", BroadcastMessageType.Notice);
            CurrentEvent = this;
        }

        public virtual void Disable()
        {
            IsEnabled = false;
            CurrentEvent = null;
        }

        protected void ShowTimerAll(int durationSeconds, Action onEndCB)
        {
            Characters.ForEach(c => MapPacket.ShowMapTimerForCharacter(c, durationSeconds));
            RepeatingAction.Start(onEndCB, durationSeconds * 1000, 0);
        }

        public abstract EnterStatus Enter(GameCharacter chr, int mapIdx);

        public override EnterStatus EnterCheck(GameCharacter chr, int mapIdx, out IList<GameCharacter> partyMembers)
        {
            partyMembers = new List<GameCharacter>();
            if (!IsEnabled) return EnterStatus.Invalid;
            if (UserCount >= Capacity) return EnterStatus.Full;

            var qr = chr.QuestRecord;
            var inventory = chr.Inventory;
            
            string val2 = qr.Get(9001);
            string cTime = MasterThread.CurrentTimeStr;

            if (!chr.IsAdmin)
            {
                if (!Server.Tespia && val2 != "")
                {
                    var aTime = MasterThread.CompareTime(cTime, val2);
                    if (aTime < 1440) return EnterStatus.Invalid;
                }

                if (inventory.ItemCount(4031019) >= 1) return EnterStatus.Invalid;
                var ret = inventory.CanExchange(0, (4000038, 1));
                if (!ret) return EnterStatus.InventoryFull;
            }
            return base.EnterCheck(chr, mapIdx, out partyMembers);
        }

        protected EnterStatus Enter(GameCharacter chr, int mapIdx, string portalName)
        {
            var status = EnterCheck(chr, mapIdx, out var partyMembers);
            if (status != EnterStatus.Success) return status;

            FinishEnter(chr, mapIdx, partyMembers, portalName, false);
            return status;
        }

        protected virtual void FinishEnter(GameCharacter chr, int mapIdx, IList<GameCharacter> partyMembers, string portalName)
        {
            string cTime = MasterThread.CurrentTimeStr;
            var qr = chr.QuestRecord;
            int cmap = chr.Field.ID;
            chr.Inventory.Exchange(0, 4000038, 1);
            if (!Server.Tespia) qr.Set(9001, cTime);

            if (cmap == 60000) qr.Set(9000, "maple");
            else if (cmap == 104000000) qr.Set(9000, "victoria");
            else if (cmap == 200000000) qr.Set(9000, "ossyria");
            else if (cmap == 220000000) qr.Set(9000, "ludi");
            FinishEnter(chr, mapIdx, partyMembers, portalName, false);
        }

        public void RunTimer()
        {
            foreach (var map in Maps)
            {
                foreach (var character in map.Characters)
                {
                    RunTimer(character, map);
                }
            }
        }

        public void EnablePortals(bool sendNotice = true)
        {
            foreach (var map in Maps)
            {
                map.PortalsOpen = true;
                foreach (var portal in map.Portals.Values)
                {
                    portal.Enabled = true;
                }
            }
            if (sendNotice)
            {
                ChatPacket.SendBroadcastMessageToMap(Lobby, "The portal has now opened. Press the up arrow key at the portal to enter.", BroadcastMessageType.Notice);
            }
        }

        public override void End()
        {
            base.End();
            Disable();
        }

        public void SendInstructions()
        {
            MapPacket.SendGMEventInstructions(Lobby);
        }
    }
}
