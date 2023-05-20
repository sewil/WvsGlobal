using System.Collections.Generic;
using WvsBeta.Common;
using System.Linq;
using WvsBeta.Game.GameObjects;
using static WvsBeta.MasterThread;
using static WvsBeta.Game.GameObjects.Map_Snowball;
using System;
using log4net;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game.Events
{
    class MapleSnowballEvent : TeamEvent
    {
        private static ILog _log = LogManager.GetLogger("MapleSnowballEvent");
        private Map_Snowball SnowballMap => (Map_Snowball)Maps[1];

        public MapleSnowballEvent(ConfigReader.Node fsNode) : base(fsNode)
        {
        }

        public override void Enable()
        {
            base.Enable();
            SnowballMap.Reset();
        }

        public override EnterStatus Enter(GameCharacter chr, int mapIdx)
        {
            return Enter(chr, mapIdx, null);
        }

        public override void Start()
        {
            base.Start();
            foreach (var chr in Lobby.Characters.ToList())
            {
                if (MapleTeam.Count < StoryTeam.Count)
                {
                    MapleTeam.Add(chr);
                    chr.ChangeMap(SnowballMap.ID, SnowballMap.Top.Name);
                }
                else
                {
                    StoryTeam.Add(chr);
                    chr.ChangeMap(SnowballMap.ID, SnowballMap.Bottom.Name);
                }
            }

            Program.MainForm.LogDebug("Starting..." + " Maple " + string.Join(", ", MapleTeam.Select(c => c.Name)) + "... Story " + string.Join(", ", StoryTeam.Select(c => c.Name)));
            SnowballMap.SnowballState = SnowballEventState.IN_PROGRESS;
        }

        public override void End()
        {
            Started = false;
            Program.MainForm.LogDebug("Stopping.");

            List<GameCharacter> Winners;
            List<GameCharacter> Losers;

            if (SnowballMap.GetWinner() == SnowballEventState.MAPLE_WIN)
            {
                Winners = MapleTeam.ToList();
                Losers = StoryTeam.ToList();
            }
            else
            {
                Winners = StoryTeam.ToList();
                Losers = MapleTeam.ToList();
            }

            _log.Info("Total players: " + (Winners.Count + Losers.Count));
            _log.Info("Winners: " + string.Join(", ", Winners.Select(x => x.Name)));
            _log.Info("Losers: " + string.Join(", ", Losers.Select(x => x.Name)));

            Winners.ForEach(c =>
            {
                c.SendPacket(FieldEffectPacket.EffectSound("Coconut/Victory"));
                c.SendPacket(FieldEffectPacket.EffectScreen("event/coconut/victory"));
            });
            Losers.ForEach(c =>
            {
                c.SendPacket(FieldEffectPacket.EffectSound("Coconut/Failed"));
                c.SendPacket(FieldEffectPacket.EffectScreen("event/coconut/lose"));
            });

            RepeatingAction.Start("snowball warper", e =>
            {
                Winners.ForEach(c => c.ChangeMap(WinMap.ID));
                base.End();
            }, 10 * 1000, 0);
        }

        public void PlayerLeft(GameCharacter chr)
        {
            MapleTeam.Remove(chr);
            StoryTeam.Remove(chr);
        }
    }
}
