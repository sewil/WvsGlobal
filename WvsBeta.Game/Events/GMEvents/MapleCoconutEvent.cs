using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events.Packets;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.Packets;
using static WvsBeta.MasterThread;

namespace WvsBeta.Game.Events.GMEvents
{
    public enum EventTeam
    {
        Maple = 0,
        Story = 1
    }
    public class Coconut
    {
        public short ID { get; set; }
        public int Hits { get; set; }
        public CoconutType TypeOnDestroy { get; }
        public bool Hittable { get; set; }
        public Coconut(short id, CoconutType typeOnDestroy)
        {
            Hittable = true;
            ID = id;
            this.TypeOnDestroy = typeOnDestroy;
        }
    }
    public class MapleCoconutEvent : EventFieldSet
    {
        Map_Coconut CoconutMap => Lobby as Map_Coconut;

        Dictionary<short, Coconut> Coconuts = new Dictionary<short, Coconut>();
        public short MapleScore { get; private set; }
        public short StoryScore { get; private set; }
        public MapleCoconutEvent(ConfigReader.Node fsNode) : base(fsNode)
        {
        }

        public override void Enable()
        {
            base.Enable();
            CoconutMap.MapleTeam.Clear();
            CoconutMap.StoryTeam.Clear();
            Coconuts.Clear();
        }

        public override EnterStatus Enter(GameCharacter chr, int mapIdx)
        {
            string portalName = chr.IsAdmin ? "h001" : null;
            var status = EnterCheck(chr, mapIdx, out var partyMembers);
            FinishEnter(chr, mapIdx, partyMembers, portalName);
            CoconutPackets.SendScore(chr, MapleScore, StoryScore);
            return status;
        }

        public override void Start()
        {
            base.Start();
            StartRound();
            BroadcastTimeMsg(StringPool.Instance[2460]); // 2460;"Coconut Season has started!"
        }

        private void BroadcastTimeMsg(string msg)
        {
            BroadcastMsg(CoconutMap.TimeMessage, msg);
        }

        public void ResetCoconuts()
        {
            Coconuts.Clear();
            short count = (short)(CoconutMap.CountBombing + CoconutMap.CountFalling + CoconutMap.CountStopped);
            var ids = new Queue<short>(new List<short>().Fill(0, count).Shuffle());
            AddCoconuts(ids, CoconutMap.CountFalling, CoconutType.Destroy);
            AddCoconuts(ids, CoconutMap.CountStopped, CoconutType.Hit);
            AddCoconuts(ids, CoconutMap.CountBombing, CoconutType.Break);
            Coconuts.Values.ForEach(c =>
            {
                CoconutPackets.SendCoconut(CoconutMap, c.ID, 0, CoconutType.Spawn);
            });
        }

        private void AddCoconuts(Queue<short> ids, int num, CoconutType destroyType)
        {
            for (int i = 0; i < num; i++)
            {
                short id = ids.Dequeue();
                Coconuts.Add(id, new Coconut(id, destroyType));
            }
        }

        private int round = 0;
        public void StartRound()
        {
            round++;
            int durationSeconds;
            if (round == 1)
            {
                durationSeconds = CoconutMap.TimeDefault;
            }
            else
            {
                durationSeconds = CoconutMap.TimeExpand;
            }

            ResetCoconuts();
            MapleScore = 0;
            StoryScore = 0;
            CoconutPackets.SendScore(CoconutMap, MapleScore, StoryScore);
            ShowTimerAll(durationSeconds, () => EndRound()); // TimeMessage?
        }

        public void EndRound()
        {
            Coconuts.ForEach(c => c.Value.Hittable = false);
            bool extend = false;
            var winners = new List<GameCharacter>();
            var losers = new List<GameCharacter>();
            if (MapleScore == StoryScore)
            {
                extend = round == 1;
                if (!extend)
                {
                    losers = CoconutMap.MapleTeam.Values.Concat(CoconutMap.StoryTeam.Values).ToList(); // Big L
                }
            }
            else if (MapleScore > StoryScore)
            {
                winners = CoconutMap.MapleTeam.Values.ToList();
                losers = CoconutMap.StoryTeam.Values.ToList();
            }
            else
            {
                winners = CoconutMap.StoryTeam.Values.ToList();
                losers = CoconutMap.MapleTeam.Values.ToList();
            }
            if (extend)
            {
                BroadcastTimeMsg(StringPool.Instance[2466]);
                BroadcastTimeMsg(StringPool.Instance[2467]);
                // 2466;"The game ended in a tie, so an additional 2 minutes is rewarded for overtime."
                // 2467;"If the game ends in a tie after 2 minutes, both teams will be deemed losers and the prizes will not be awarded."
            }
            else
            {
                // 2463;"  	The game of Coconut Season has ended, and you'll be transported to a different map. Please wait."
                BroadcastTimeMsg(StringPool.Instance[2463]);
                winners.ForEach(w =>
                {
                    w.SendPacket(FieldEffectPacket.EffectScreen(CoconutMap.EffectWin));
                    w.SendPacket(FieldEffectPacket.EffectSound(CoconutMap.SoundWin));
                });
                losers.ForEach(l =>
                {
                    l.SendPacket(FieldEffectPacket.EffectScreen(CoconutMap.EffectLose));
                    l.SendPacket(FieldEffectPacket.EffectSound(CoconutMap.SoundLose));
                });
            }
            ShowTimerAll(CoconutMap.TimeFinish, () =>
            {
                if (extend)
                {
                    StartRound();
                }
                else
                {
                    winners.ForEach(w => w.ChangeMap(WinMap.ID));
                    End(); // Losers are automatically returned
                }
            });
        }

        private void DestroyCoconut(int chrID)
        {
            if (CoconutMap.MapleTeam.TryGetValue(chrID, out GameCharacter chr))
            {
                MapleScore++;
            }
            else if (CoconutMap.StoryTeam.TryGetValue(chrID, out chr))
            {
                StoryScore++;
            }
            if (chr != null)
            {
                BroadcastTimeMsg($"{chr.Name} of Team Maple knocks down a coconut.");
                CoconutPackets.SendScore(Lobby, MapleScore, StoryScore);
            }
        }

        private void HitCoconut(int chrID, short coconutID, short delay)
        {
            if (!Coconuts.TryGetValue(coconutID, out Coconut coconut)) return;
            if (!coconut.Hittable) return;

            coconut.Hits++;
            CoconutType type;
            if (coconut.Hits >= CoconutMap.CountHit)
            {
                coconut.Hittable = false;
                type = coconut.TypeOnDestroy;
                if (type == CoconutType.Destroy)
                {
                    DestroyCoconut(chrID);
                }
            }
            else
            {
                type = CoconutType.Hit;
            }
            CoconutPackets.SendCoconut(CoconutMap, coconut.ID, delay, type);
        }

        public static void HandleHit(GameCharacter chr, Packet pr)
        {
            if (!(EventFieldSet.CurrentEvent is MapleCoconutEvent e)) return;
            // Check hit type, break etc
            short id = pr.ReadShort();
            short delay = pr.ReadShort();
            //][FIELD_COCONUT_ATTACK] 75 8D 01 1B 01
            e.HitCoconut(chr.ID, id, delay);
        }
    }
}
