using reNX.NXProperties;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events.GMEvents;
using WvsBeta.Game.Events.Packets;
using System.Collections.Generic;

namespace WvsBeta.Game.GameObjects
{
    public class Map_Coconut : Map
    {
        /// <summary>
        /// Number of coconuts that will be destroyed
        /// </summary>
        public int CountFalling { get; }
        /// <summary>
        /// Number of coconuts that will break
        /// </summary>
        public int CountBombing { get; }
        /// <summary>
        /// Number of coconuts that will stop being hittable
        /// </summary>
        public int CountStopped { get; }
        /// <summary>
        /// Number of hits to destroy a coconut
        /// </summary>
        public int CountHit { get; }
        /// <summary>
        /// Round time, default
        /// </summary>
        public int TimeDefault { get; }
        /// <summary>
        /// Extension round, if there's a tie
        /// </summary>
        public int TimeExpand { get; }
        public BroadcastMessageType TimeMessage { get; }
        /// <summary>
        /// Time between rounds, or between win/lose announce and warp
        /// </summary>
        public int TimeFinish { get; }
        public string EffectWin { get; }
        public string EffectLose { get; }
        public string SoundWin { get; }
        public string SoundLose { get; }
        public MapleCoconutEvent Event => EventFieldSet.CurrentEvent as MapleCoconutEvent;
        public readonly Dictionary<int, GameCharacter> MapleTeam = new Dictionary<int, GameCharacter>();
        public readonly Dictionary<int, GameCharacter> StoryTeam = new Dictionary<int, GameCharacter>();
        public Map_Coconut(int id, NXNode node) : base(id)
        {
            var cnode = node["coconut"];
            CountFalling = cnode["countFalling"].ValueInt32();
            CountBombing = cnode["countBombing"].ValueInt32();
            CountStopped = cnode["countStopped"].ValueInt32();
            CountHit = cnode["countHit"].ValueInt32();
            TimeDefault = cnode["timeDefault"].ValueInt32();
            TimeExpand = cnode["timeExpand"].ValueInt32();
            TimeMessage = (BroadcastMessageType)cnode["timeMessage"].ValueByte();
            TimeFinish = cnode["timeFinish"].ValueInt32();
            EffectWin = cnode["effectWin"].ValueString();
            EffectLose = cnode["effectLose"].ValueString();
            SoundWin = cnode["soundWin"].ValueString();
            SoundLose = cnode["soundLose"].ValueString();
        }

        public override bool HandlePacket(GameCharacter character, Packet packet, ClientMessages opcode)
        {
            switch (opcode)
            {
                case ClientMessages.FIELD_COCONUT_ATTACK:
                    MapleCoconutEvent.HandleHit(character, packet);
                    return true;
                default: break;
            }

            return base.HandlePacket(character, packet, opcode);
        }

        public override void SendCharacterEnter(GameCharacter player, GameCharacter victim)
        {
            var pw = MapPacket.CharacterEnterPacket(player);
            var team = EventTeam.Maple;
            if (StoryTeam.TryGetValue(player.ID, out var _))
            {
                team = EventTeam.Story;
            }
            pw.WriteByte((byte)team);
            victim.SendPacket(pw);
        }

        public override void AddPlayer(GameCharacter chr)
        {
            EventTeam team = EventTeam.Maple;
            if (Event != null)
            {
                if (MapleTeam.Count > StoryTeam.Count)
                {
                    team = EventTeam.Story;
                    StoryTeam[chr.ID] = chr;
                }
                else
                {
                    MapleTeam[chr.ID] = chr;
                }
            }
            base.AddPlayer(chr);
            CoconutPackets.ForcedEquip(chr, team);
        }

        public override void RemovePlayer(GameCharacter chr, bool gmhide = false)
        {
            if (Event != null)
            {
                MapleTeam.Remove(chr.ID);
                StoryTeam.Remove(chr.ID);
            }
            base.RemovePlayer(chr, gmhide);
        }
    }
}
