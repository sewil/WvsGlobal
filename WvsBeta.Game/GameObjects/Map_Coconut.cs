using reNX.NXProperties;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.Events.GMEvents;
using WvsBeta.Game.Events.Packets;
using System.Collections.Generic;
using WvsBeta.Game.Events;
using static WvsBeta.Game.FieldSet;
using WvsBeta.Common.Extensions;
using WvsBeta.Common;
using WvsBeta.Game.Packets;
using static WvsBeta.MasterThread;
using WvsBeta.Common.Enums;
using System.Linq;
using System.Drawing;

namespace WvsBeta.Game.GameObjects
{
    public class Map_Coconut : TeamEventMap
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
                    short id = packet.ReadShort();
                    short delay = packet.ReadShort();
                    HitCoconut(character, id, delay);
                    return true;
                default: break;
            }

            return base.HandlePacket(character, packet, opcode);
        }

        public override void SendCharacterEnter(GameCharacter player, GameCharacter victim)
        {
            var pw = MapPacket.CharacterEnterPacket(player);
            pw.WriteByte((byte)player.Team);
            victim.SendPacket(pw);
        }

        public override void AddPlayer(GameCharacter chr)
        {
            base.AddPlayer(chr);
            CoconutPackets.ForcedEquip(chr);
            CoconutPackets.SendScore(chr, ScoreMaple, ScoreStory);
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

        Dictionary<short, Coconut> Coconuts = new Dictionary<short, Coconut>();
        public short ScoreMaple { get; private set; }
        public short ScoreStory { get; private set; }

        public override void Reset()
        {
            base.Reset();
            Coconuts.Clear();
        }

        public override void Start()
        {
            base.Start();
            StartRound();
            BroadcastTimeMsg(StringPool.Instance[2460]); // 2460;"Coconut Season has started!"
        }

        private void BroadcastTimeMsg(string msg)
        {
            ChatPacket.SendBroadcastMessageToMap(this, msg, TimeMessage);
        }

        public void ResetCoconuts()
        {
            Coconuts.Clear();
            short count = (short)(CountBombing + CountFalling + CountStopped);
            var ids = new Queue<short>(new List<short>().Fill(0, count).Shuffle());
            AddCoconuts(ids, CountFalling, CoconutType.Destroy);
            AddCoconuts(ids, CountStopped, CoconutType.Hit);
            AddCoconuts(ids, CountBombing, CoconutType.Break);
            Coconuts.Values.ForEach(c =>
            {
                CoconutPackets.SendCoconut(this, c.ID, 0, CoconutType.Spawn);
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
                durationSeconds = TimeDefault;
            }
            else
            {
                durationSeconds = TimeExpand;
            }

            ResetCoconuts();
            ScoreMaple = 0;
            ScoreStory = 0;
            CoconutPackets.SendScore(this, ScoreMaple, ScoreStory);
            StartTimer(durationSeconds);
            OnTimerEnd += EndRound;
        }

        private bool extendRound;
        private IList<GameCharacter> winners = new List<GameCharacter>();
        private IList<GameCharacter> losers = new List<GameCharacter>();
        public void EndRound(Map map)
        {
            OnTimerEnd -= EndRound;
            Coconuts.ForEach(c => c.Value.Hittable = false);
            if (ScoreMaple == ScoreStory)
            {
                extendRound = round == 1;
                if (!extendRound)
                {
                    losers = TeamMaple.Concat(TeamStory).ToList(); // Big L
                }
            }
            else if (ScoreMaple > ScoreStory)
            {
                winners = TeamMaple.ToList();
                losers = TeamStory.ToList();
            }
            else
            {
                winners = TeamStory.ToList();
                losers = TeamMaple.ToList();
            }
            if (extendRound)
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
                    w.SendPacket(FieldEffectPacket.EffectScreen(EffectWin));
                    w.SendPacket(FieldEffectPacket.EffectSound(SoundWin));
                });
                losers.ForEach(l =>
                {
                    l.SendPacket(FieldEffectPacket.EffectScreen(EffectLose));
                    l.SendPacket(FieldEffectPacket.EffectSound(SoundLose));
                });
            }
            StartTimer(TimeFinish);
            OnTimerEnd += FinishRound;
        }

        private void FinishRound(Map map)
        {
            OnTimerEnd -= FinishRound;
            if (extendRound)
            {
                StartRound();
            }
            else
            {
                winners.ForEach(w => w.ChangeMap(WinMap.ID));
                End(); // Losers are automatically returned
            }
        }

        private void DestroyCoconut(GameCharacter chr)
        {
            if (chr.Team == EventTeam.Story)
            {
                ScoreStory++;
            }
            else
            {
                ScoreMaple++;
            }
            if (chr != null)
            {
                BroadcastTimeMsg($"{chr.Name} of Team Maple knocks down a coconut.");
                CoconutPackets.SendScore(this, ScoreMaple, ScoreStory);
            }
        }

        private void HitCoconut(GameCharacter chr, short coconutID, short delay)
        {
            if (!Coconuts.TryGetValue(coconutID, out Coconut coconut)) return;
            if (!coconut.Hittable) return;

            coconut.Hits++;
            CoconutType type;
            if (coconut.Hits >= CountHit)
            {
                coconut.Hittable = false;
                type = coconut.TypeOnDestroy;
                if (type == CoconutType.Destroy)
                {
                    DestroyCoconut(chr);
                }
            }
            else
            {
                type = CoconutType.Hit;
            }
            CoconutPackets.SendCoconut(this, coconut.ID, delay, type);
        }
    }
}
