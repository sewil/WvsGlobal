using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using log4net;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Objects.Stats;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.GameObjects;
using WvsBeta.Game.GameObjects.MiniRooms;
using WvsBeta.Game.Handlers;
using WvsBeta.Game.Handlers.Contimove;
using WvsBeta.Game.Handlers.MiniRooms;
using WvsBeta.Game.Packets;
using WvsBeta.Common.Extensions;

namespace WvsBeta.Game
{
    public class Map
    {
        protected static ILog log = LogManager.GetLogger("MapLogger");

        public double m_dIncRate_Exp = 1.0;
        public double m_dIncRate_Drop = 1.0;

        public int ID { get; }
        public int ForcedReturn { get; set; }
        public int ReturnMap { get; set; }
        public bool Town { get; set; }
        public FieldLimit Limitations { get; set; }
        public double MobRate { get; set; }
        public bool HasClock { get; set; }
        public bool DisableScrolls { get; set; } = false;
        public bool AcceptPersonalShop { get; set; } = false;
        public bool DisableGoToCashShop { get; set; } = false;
        public bool DisableChangeChannel { get; set; } = false;
        public string Name { get; set; }

        public bool EverlastingDrops
        {
            get => DropPool.DropEverlasting;
            set => DropPool.DropEverlasting = value;
        }

        public FieldSet ParentFieldSet { get; set; } = null;

        public short DecreaseHP { get; set; }
        public int ProtectItem { get; set; }
        public short TimeLimit { get; set; } = 0;

        public int WeatherID { get; set; }
        public string WeatherMessage { get; set; }
        public bool WeatherIsAdmin { get; set; }

        public int JukeboxID { get; set; } = -1;
        public string JukeboxUser { get; set; }

        public int MobCapacityMin;
        public int MobCapacityMax;

        private LoopingID _objectIDs { get; } = new LoopingID();

        public List<Foothold> Footholds { get; private set; }
        public List<NpcLife> NPC { get; } = new List<NpcLife>();
        public Dictionary<string, Portal> Portals { get; } = new Dictionary<string, Portal>();
        public List<Portal> SpawnPoints { get; } = new List<Portal>();
        public List<Portal> DoorPoints { get; } = new List<Portal>();
        public Dictionary<int, Seat> Seats { get; } = new Dictionary<int, Seat>();
        public Dictionary<int, Mist> SpawnedMists { get; } = new Dictionary<int, Mist>();
        public Dictionary<int, BalloonRoom> BalloonRooms { get; } = new Dictionary<int, BalloonRoom>();
        public HashSet<string> EffectObjects = new HashSet<string>();
        public List<Kite> Kites { get; } = new List<Kite>();
        public Dictionary<string, MapArea> Areas { get; } = new Dictionary<string, MapArea>();
        public Rectangle MBR { get; private set; }
        public Rectangle VRLimit { get; private set; }
        public Rectangle ReallyOutOfBounds { get; private set; }

        public List<short> UsedSeats { get; } = new List<short>();

        public Dictionary<int, Mob> Mobs { get; } = new Dictionary<int, Mob>();
        public List<MobGenItem> MobGen { get; } = new List<MobGenItem>();
        private long _lastCreateMobTime;
        public List<GameCharacter> Characters { get; } = new List<GameCharacter>();
        public IEnumerable<GameCharacter> GetRegularPlayers => Characters.Where(x => !x.IsGM);
        public IEnumerable<GameCharacter> GetGMs => Characters.Where(x => x.IsGM);

        public bool PeopleInMap => Characters.Count > 0;

        public DropPool DropPool { get; }
        public ReactorPool ReactorPool { get; }
        public readonly DoorManager DoorPool;
        public readonly SummonPool Summons;

        public Dictionary<string, long> PlayersThatHaveBeenHere { get; } = new Dictionary<string, long>();
        public Dictionary<int, int> MobKillCount { get; } = new Dictionary<int, int>();

        public const double MAP_PREMIUM_EXP = 1.0;
        public bool PortalsOpen { get; set; } = true;
        public Action<GameCharacter, Map> OnEnter { get; set; }
        public Action<GameCharacter, Map> OnExit { get; set; }

        public Action<Map> OnTimerEnd { get; set; }
        public long TimerEndTime { get; set; }

        public bool ChatEnabled { get; set; }

        public void StartTimer(long seconds)
        {
            TimerEndTime = MasterThread.CurrentTime + (seconds * 1000);
            SendMapTimer(null);
        }


        public Map(int id)
        {
            ID = id;

            DropPool = new DropPool(this);
            ReactorPool = new ReactorPool(this);

            MobRate = 1.0;
            _lastCreateMobTime = MasterThread.CurrentTime;
            ChatEnabled = true;

            DoorPool = new DoorManager(this);
            Summons = new SummonPool(this);
        }

        internal Mob GetMob(int SpawnID) => Mobs.TryGetValue(SpawnID, out Mob ret) ? ret : null;
        internal NpcLife GetNPC(int SpawnID) => NPC.FirstOrDefault(n => n.SpawnID == SpawnID);
        public GameCharacter GetPlayer(int id) => Characters.FirstOrDefault(a => a.ID == id);
        public IEnumerable<GameCharacter> GetInParty(int ptId) => Characters.Where(c => ptId > 0 && c.PartyID == ptId);
        public List<int> GetIDsInParty(int ptId) => GetInParty(ptId).Select(x => x.ID).ToList();

        public virtual bool FilterAdminCommand(GameCharacter character, CommandHandling.CommandArgs command)
        {
            return false;
        }

        public virtual bool HandlePacket(GameCharacter character, Packet packet, ClientMessages opcode)
        {
            return false;
        }


        public struct MobKillCountInfo
        {
            public int mapId;
            public int mobId;
            public int killCount;
        }

        public void FlushMobKillCount()
        {
            if (MobKillCount.Count == 0) return;
            foreach (var keyValuePair in MobKillCount)
            {
                log.Info(new MobKillCountInfo
                {
                    killCount = keyValuePair.Value,
                    mapId = ID,
                    mobId = keyValuePair.Key
                });
            }
            MobKillCount.Clear();
        }

        private long lastDamageGiven = 0;
        public virtual void MapTimer(long pNow)
        {
            DropPool.Update(pNow);

            if (SpawnedMists.Count > 0)
                UpdateMists(pNow);
            else
                Mobs.Values.ToList().ForEach(x =>
                {
                    x.UpdateDeads(pNow);
                });

            if (PeopleInMap)
            {

                TryCreateMobs(pNow, false);

                var ft = BuffStat.GetTimeForBuff();
                Characters.ForEach(x => x.PrimaryStats.CheckExpired(ft));
                Characters.ForEach(x => x.Summons.Update(pNow));
                CheckForMobVac();


                // Find new controllers

                Mobs.Values
                    .Where(x => x.IsControlled && (MasterThread.CurrentTime - x.LastMove) > 10000)
                    .ForEach(x => FindNewController(x, null));

                Mobs.Values
                    .Where(x => !x.IsControlled)
                    .ForEach(x => FindNewController(x, x.Controller));


                if (lastDamageGiven == 0 || (pNow - lastDamageGiven) > 10000)
                {
                    lastDamageGiven = pNow;
                    if (DecreaseHP > 0)
                    {
                        // Damage
                        foreach (var character in Characters)
                        {
                            var actualDamage = DecreaseHP;
                            if (character.PrimaryStats.BuffThaw.IsSet(pNow))
                                actualDamage -= character.PrimaryStats.BuffThaw.N;
                            if (ProtectItem > 0 && character.Inventory.GetEquippedItem(ProtectItem, out EquippedType __) != null)
                                actualDamage = 0;
                            // TODO: If swim, negative multiple N value (Freeze damage?)

                            // TODO: If we've got snowboots, do no damage
                            if (actualDamage > 0)
                            {
                                character.DamageHP(actualDamage);
                            }

                            // TODO: If pets and consumeHP flag set, trigger pet consume pot
                        }
                    }
                    else if (DecreaseHP < 0)
                    {
                        // Heal
                    }
                }
            }

            if (MasterThread.CurrentTime >= TimerEndTime)
            {
                OnTimerEnd?.Invoke(this);
            }

            DoorPool.Update(pNow);
        }

        private long lastVacCheck = 0;

        private void CheckForMobVac()
        {
            if ((MasterThread.CurrentTime - lastVacCheck) > 5000) return;
            lastVacCheck = MasterThread.CurrentTime;

            if (Mobs.Count < 10) return;

            int MaxMobsForTrigger = (Mobs.Count * 100) / 60;


            foreach (var grouping in Mobs.Values.GroupBy(x => (ushort)x.Position.X << 16 | (ushort)x.Position.Y))
            {
                if (grouping.Count() >= MaxMobsForTrigger)
                {
                    int x = (grouping.Key >> 16) & 0xFFFF;
                    int y = (grouping.Key >> 0) & 0xFFFF;
                    ChatPacket.SendBroadcastMessageToGMs(
                        $"Possible mobvac on map {ID}. More than {MaxMobsForTrigger} mobs on the exact same position. Location: {x} {y}." + GetMobvacPlayerNameInfo(),
                        BroadcastMessageType.Megaphone
                    );
                    return;
                }
            }

            int mobsWithPossiblePvac_Left = 0;
            int mobsWithPossiblePvac_Right = 0;
            int mobsWithInvalidFoothold = 0;
            int mobsOutOfBounds = 0;

            foreach (var mobsValue in Mobs.Values)
            {
                if (mobsValue.Data.Flies) continue;

                Point mobPos = mobsValue.Position;
                var mobFoothold = mobsValue.Foothold;


                Footholds.TryFind(x => x.ID == mobFoothold, (fh) =>
                {
                    if (VRLimit.Contains(mobPos) == false)
                    {
                        Trace.WriteLine("Not in bounds of VRLimit");
                        mobsOutOfBounds++;
                        return;
                    }

                    var rect = new Rectangle(
                        Math.Min(fh.X1, fh.X2),
                        Math.Min(fh.Y1, fh.Y2),
                        Math.Abs(fh.X2 - fh.X1),
                        Math.Abs(fh.Y2 - fh.Y1)
                    );

                    rect = Rectangle.Inflate(rect, 3, 3);

                    if (rect.Contains(mobPos) == false)
                    {
                        Trace.WriteLine("Not in bounds of foothold");
                    }


                    const int xOffset = 30; // A mob has fhWidth - (20 + 20) walking space
                    const int xMinForTrigger = 10; // Must be within 'x + 20' to 'x + 20 + xMinForTrigger' range

                    int xMoveOffset = rect.Width - (xOffset + xMinForTrigger);

                    if (fh.PreviousIdentifier == 0)
                    {
                        rect.Offset(-xMoveOffset, 0);

                        if (rect.Contains(mobPos))
                        {
                            //Trace.WriteLine("On the edge of the foothold (left)");
                            mobsWithPossiblePvac_Left++;
                        }
                    }
                    else if (fh.NextIdentifier == 0)
                    {
                        rect.Offset(xMoveOffset, 0);

                        if (rect.Contains(mobPos))
                        {
                            //Trace.WriteLine("On the edge of the foothold (right)");
                            mobsWithPossiblePvac_Right++;
                        }
                    }
                }, () =>
                {
                    mobsWithInvalidFoothold++;
                });

            }

            // either side

            int pvacTriggerAmount = (Mobs.Count * 100) / 60;

            if ((mobsWithPossiblePvac_Left > pvacTriggerAmount && mobsWithPossiblePvac_Right == 0) ||
                (mobsWithPossiblePvac_Right > pvacTriggerAmount && mobsWithPossiblePvac_Left == 0))
            {
                ChatPacket.SendBroadcastMessageToGMs(
                    $"Possible mobvac on map {ID}. pvac left {mobsWithPossiblePvac_Left}, pvac right {mobsWithPossiblePvac_Right}" + GetMobvacPlayerNameInfo(),
                    BroadcastMessageType.Megaphone
                );
            }

            if (mobsOutOfBounds > 3)
            {
                ChatPacket.SendBroadcastMessageToGMs($"Possible mobvac on map {ID}. Mobs out of bounds {mobsOutOfBounds}" + GetMobvacPlayerNameInfo(), BroadcastMessageType.Megaphone);
            }
            /*
            if (mobsWithInvalidFoothold > 3)
            {
                MessagePacket.SendNoticeGMs($"Possible mobcrash? on map {ID}. Mobs on unknown foothold {mobsWithInvalidFoothold}", MessagePacket.MessageTypes.Megaphone);
            }
            */
        }

        public string GetMobvacPlayerNameInfo()
        {
            var nonAdminChars = GetRegularPlayers.ToList();
            if (nonAdminChars.Count == 0) return "- no players? -";
            if (nonAdminChars.Count != 1) return $"- {nonAdminChars.Count} players -";

            var chr = nonAdminChars[0];
            return $"- {chr.Name} (id: {chr.ID}) in map -";
        }

        public void UpdateMists(long pNow)
        {
            var tmplist = SpawnedMists.Values.Where(x => x.Time < pNow).ToArray();
            foreach (var mist in tmplist)
            {
                MistPacket.SendMistDespawn(mist);
                SpawnedMists.Remove(mist.SpawnID);
            }

            var mobsCopy = Mobs.Values.ToArray();
            foreach (var mob in mobsCopy)
            {
                mob.UpdateDeads(pNow);
                if (mob.DeadAlreadyHandled) continue;

                var fart = SpawnedMists.Values.FirstOrDefault((a) =>
                {
                    return
                        !a.MobMist &&
                        (
                            mob.Position.X >= a.LT_X &&
                            mob.Position.Y >= a.LT_Y &&
                            mob.Position.X <= a.RB_X &&
                            mob.Position.Y <= a.RB_Y
                        );
                });
                if (fart != null)
                {
                    var sld = GameDataProvider.Skills[fart.SkillID].Levels[fart.SkillLevel];
                    if (Rand32.NextBetween(0, 100) < sld.Property)
                    {
                        long buffTimeInMilliseconds = sld.BuffTime * 1000;
                        mob.DoPoison(fart.OwnerID, fart.SkillLevel, buffTimeInMilliseconds, fart.SkillID, sld.MagicAttack, 0);
                    }
                }
            }
        }

        public bool IsBoostedMobGen => Characters.Count > MobCapacityMin / 2;

        public int GetCapacity()
        {
            if (Limitations.HasFlag(FieldLimit.NoMobCapacityLimit))
                return MobGen.Count;

            if (IsBoostedMobGen)
            {
                if (Characters.Count < MobCapacityMin * 2)
                {
                    return (MobCapacityMin + (MobCapacityMax - MobCapacityMin) * (2 * Characters.Count - MobCapacityMin) / (3 * MobCapacityMin));
                }
                else
                {
                    return MobCapacityMax;
                }
            }
            return MobCapacityMin;
        }

        public void TryCreateMobs(long tCur, bool bReset)
        {
            if ((!bReset && tCur - _lastCreateMobTime < 7000))
                return;

            // This should be done at the end, but it gives odd spawns
            _lastCreateMobTime = tCur;

            int capacity = GetCapacity();
            if (capacity < 0) return;

            int remainCapacity = capacity - Mobs.Count;

            if (remainCapacity <= 0)
                return;

            var mobGenPossible = new List<MobGenItem>(remainCapacity);
            // Fill the list with currently shown mobs
            var posList = Mobs.Values.Select(x => x.Position).ToList();

            foreach (var mgi in MobGen)
            {
                // TODO: Check if we can summon this mob (MobExcept)...
                bool add_in_front = true;
                var regenInterval = mgi.RegenInterval;
                if (regenInterval == 0)
                {
                    bool anyMobSpawned = posList.Count != 0;
                    if (anyMobSpawned)
                    {
                        // Figure if any mob is already spawned
                        var rect = Rectangle.FromLTRB(
                            mgi.X - 100, mgi.Y - 100,
                            mgi.X + 100, mgi.Y + 100);

                        var canSpawn = !posList.Exists(vec => rect.Contains(vec.X, vec.Y));
                        if (!canSpawn) continue;
                        Trace.WriteLineIf(ID == 108010301, "Trying to spawn mob 1");
                    }
                    else
                    {
                        // Add to end
                        add_in_front = false;
                        Trace.WriteLineIf(ID == 108010301, "Trying to spawn mob 2");
                    }
                }
                else if (regenInterval < 0)
                {
                    if (!bReset) continue;
                    Trace.WriteLineIf(ID == 108010301, "Trying to spawn mob 3");
                }
                else
                {
                    if (mgi.MobCount != 0) continue;
                    if (tCur - mgi.RegenAfter < 0) continue;

                    Trace.WriteLineIf(ID == 108010301, "Trying to spawn mob 4");
                }

                if (add_in_front)
                    mobGenPossible.Insert(0, mgi);
                else
                    mobGenPossible.Add(mgi);

                posList.Add(new Pos(mgi.X, mgi.Y));
            }

            while (mobGenPossible.Count > 0 && remainCapacity != 0)
            {
                var mgi = mobGenPossible[0];

                if (mgi.RegenInterval == 0)
                {
                    // Take random
                    mgi = mobGenPossible[(int)(Rand32.Next() % mobGenPossible.Count)];
                }

                mobGenPossible.Remove(mgi);

                if (SpawnMob(mgi.ID, mgi, new Pos(mgi.X, mgi.Y), mgi.Foothold, summonType: SummonType.Regen) != -1)
                {
                    remainCapacity--;
                }

            }

        }


        public void AddSeat(Seat ST)
        {
            Seats.Add(ST.ID, ST);
        }

        private static IEnumerable<T> InRange<T>(IEnumerable<T> elements, Pos pAround, Pos pLeftTop, Pos pRightBottom)
            where T : MovableLife
        {
            return elements.Where(mob => MovableInRange(mob, pAround, pLeftTop, pRightBottom));
        }

        private static bool MovableInRange(MovableLife mob, Pos pAround, Pos pLeftTop, Pos pRightBottom)
        {
            return (
                (mob.Position.Y >= pAround.Y + pLeftTop.Y) && (mob.Position.Y <= pAround.Y + pRightBottom.Y) &&
                (mob.Position.X >= pAround.X + pLeftTop.X) && (mob.Position.X <= pAround.X + pRightBottom.X)
            );
        }

        public IEnumerable<Mob> GetMobsInRange(Pos pAround, Pos pLeftTop, Pos pRightBottom) => InRange(Mobs.Values, pAround, pLeftTop, pRightBottom);
        public IEnumerable<GameCharacter> GetCharactersInRange(Pos pAround, Pos pLeftTop, Pos pRightBottom) => InRange(Characters, pAround, pLeftTop, pRightBottom);

        public void CreateMist(MovableLife pLife, int pSpawnID, int pSkillID, byte pSkillLevel, int pTime, int pX1, int pY1, int pX2, int pY2, short delay)
        {
            int x1, x2, y1, y2;
            /*if (pLife.IsFacingRight())
            {
                x1 = pX2 * -1 + pLife.Position.X;
                y1 = pY2 + pLife.Position.Y;
                x2 = pX1 * -1 + pLife.Position.X;
                y2 = pY1 + pLife.Position.Y;
            }
            else*/
            //{
            x1 = pX1 + pLife.Position.X;
            y1 = pY1 + pLife.Position.Y;
            x2 = pX2 + pLife.Position.X;
            y2 = pY2 + pLife.Position.Y;
            //}


            Mist mist = new Mist(pSkillID, pSkillLevel, this, pSpawnID, (pTime * 1000) + delay, x1, y1, x2, y2);
            SpawnedMists.Add(mist.SpawnID, mist);

            MistPacket.SendMistSpawn(mist, null, 0);
        }

        /**
public void AddMessageBox(Character ch, int itemid, string msg, string name, short x, short y)
{
            int id = mObjectIDs.NextValue();
            CMessageBox m = new CMessageBox();
            m.ID = id;
            m.ItemID = itemid;
            m.Message = msg;
            m.Owner = name;
            m.X = x;
            m.Y = y;
            _MessageBoxes.Add(id, m);
            ch.mMessageBoxOID = id;
            SendPacket(MapPacket.MessageBoxEnterField(id, itemid, msg, name, x, y));
}

public void AddMinigame(Character ch, string name, byte function, int x, int y, byte piecetype)
{
            int id = mObjectIDs.NextValue();
            Minigame m = new Minigame();
            m.Name = name;
            m.Type = function;
            m.Mapid = ch.mMap;
            m.X = x;
            m.Y = y;
            m.Piecetype = piecetype;
            m.ID = id;
            ch.mMinigameOID = id;
            _Minigames.Add(id, m);
}
         *
         * */

        public void SetFootholds(List<Foothold> FHs)
        {
            // Cleanup footholds:
            // If there is a vertical foothold, make footholds pointing to that as next/prev be zeroed instead.
            var verticalFHs = FHs.Where(x => x.X1 == x.X2 && Math.Abs(x.Y1 - x.Y2) > 3).Select(x => x.ID).ToList();

            Footholds = FHs.Select(x =>
            {
                var next = x.NextIdentifier;
                if (verticalFHs.Contains(next)) next = 0;
                var prev = x.PreviousIdentifier;
                if (verticalFHs.Contains(prev)) prev = 0;

                return new Foothold
                {
                    ID = x.ID,
                    NextIdentifier = next,
                    PreviousIdentifier = prev,
                    X1 = x.X1,
                    Y1 = x.Y1,
                    X2 = x.X2,
                    Y2 = x.Y2,
                };
            }).ToList();
        }

        private static volatile uint _npcidCounter = 1337;
        public void AddLife(Life LF)
        {
            if (LF.Type == 'm')
            {
                MobGen.Add(new MobGenItem(LF, null));
            }
            else if (LF.Type == 'n')
            {
                NPC.Add(new NpcLife(this, LF)
                {
                    SpawnID = _npcidCounter++
                });
            }
        }
        public void RemoveNpcLife(int id)
        {
            var idx = NPC.FindIndex(i => i.ID == id);
            if (idx > -1)
            {
                var life = NPC[idx];
                SendPacket(MapPacket.NpcLeaveField(life.SpawnID));
                NPC.RemoveAt(idx);
            }
        }
        public NpcLife SpawnNpc(int npcid, Pos position, Foothold? fh)
        {
            if (GameDataProvider.NPCs.TryGetValue(npcid, out NPCData npc))
            {
                var life = new Life();
                life.Foothold = fh?.ID ?? 0;
                life.ID = npcid;
                life.X = position.X;
                life.Y = position.Y;
                life.Type = 'n';
                AddLife(life); // Can only do gpq 4 billion times :(
                NpcLife npcLife = NPC.Last();
                SendPacket(MapPacket.NpcEnterField(npcLife));
                return npcLife;
            }
            return null;
        }
        public void GenerateMBR(Rectangle VRLimit)
        {
            int Left = int.MaxValue;
            int Top = int.MaxValue;
            int Right = int.MinValue;
            int Bottom = int.MinValue;

            foreach (var Foothold in Footholds)
            {
                if (Foothold.X1 < Left) Left = Foothold.X1;
                if (Foothold.Y1 < Top) Top = Foothold.Y1;
                if (Foothold.X2 < Left) Left = Foothold.X2;
                if (Foothold.Y2 < Top) Top = Foothold.Y2;
                if (Foothold.X1 > Right) Right = Foothold.X1;
                if (Foothold.Y1 > Bottom) Bottom = Foothold.Y1;
                if (Foothold.X2 > Right) Right = Foothold.X2;
                if (Foothold.Y2 > Bottom) Bottom = Foothold.Y2;
            }

            if (VRLimit != Rectangle.Empty)
            {
                this.VRLimit = VRLimit;
            }
            else
            {
                this.VRLimit = Rectangle.FromLTRB(Left, Top - 300, Right, Bottom + 75);
            }


            Left += 30;
            Top -= 300;
            Right -= 30;
            Bottom += 10;

            if (VRLimit != Rectangle.Empty)
            {
                if (VRLimit.Left + 20 < Left) Left = VRLimit.Left + 20;
                if (VRLimit.Top + 65 < Top) Top = VRLimit.Top + 65;
                if (VRLimit.Right - 5 > Right) Right = VRLimit.Right - 5;
                if (VRLimit.Bottom > Bottom) Bottom = VRLimit.Bottom;
            }

            MBR = Rectangle.FromLTRB(Left + 10, Top - 375, Right - 10, Bottom + 60);
            MBR = Rectangle.Inflate(MBR, 10, 10);

            ReallyOutOfBounds = Rectangle.Inflate(MBR, 60, 60);

            int MobX = (MBR.Width > 800) ? MBR.Width : 800;
            int MobY = (MBR.Height - 450 > 600) ? MBR.Height - 450 : 600;

            MobCapacityMin = (int)(((MobY * MobX) * MobRate) * 0.0000078125);
            MobCapacityMin = (MobCapacityMin < 1) ? 1 : MobCapacityMin;
            MobCapacityMin = (MobCapacityMin > 40) ? 40 : MobCapacityMin;
            MobCapacityMax = MobCapacityMin * 2;
        }

        private bool initialSpawnDone = false;
        private void SummonAllLife()
        {
            if (initialSpawnDone) return;
            initialSpawnDone = true;
            TryCreateMobs(MasterThread.CurrentTime, true);
        }

        public void AddArea(MapArea area)
        {
            Areas.Add(area.ID, area);
        }

        public void AddPortal(Portal PT)
        {
            if (PT.Name == "sp")
            {
                SpawnPoints.Add(PT);
            }
            else if (PT.Name == "tp")
            {
                DoorPoints.Add(PT);
            }
            else if (Portals.ContainsKey(PT.Name))
            {
                Trace.WriteLine($"Duplicate portal, Name: {PT.Name} MapID: {ID}");
            }
            else
            {
                Portals.Add(PT.Name, PT);
            }
        }

        public void RemovePortal(Portal PT)
        {
            if (PT.Name == "sp")
            {
                SpawnPoints.Remove(PT);
            }
            else if (PT.Name == "tp")
            {
                // TownPortal: Mystic Door
            }
            else if (Portals.ContainsKey(PT.Name))
            {
                Trace.WriteLine($"Duplicate portal, Name: {PT.Name} MapID: {ID}");
            }
            else
            {
                Portals.Remove(PT.Name);
            }
        }


        public Portal GetRandomStartPoint()
        {
            var spawnPortalIndex = Rand32.Next() % SpawnPoints.Count;
            return SpawnPoints[(int)spawnPortalIndex];
        }

        public Portal GetClosestStartPoint(Pos position)
        {
            return SpawnPoints.OrderBy(x => (new Pos(x.X, x.Y) - position)).FirstOrDefault();
        }

        public virtual void RemovePlayer(GameCharacter chr, bool gmhide = false)
        {
            if (!gmhide && !Characters.Contains(chr)) return;

            if (!gmhide)
            {
                Characters.Remove(chr);
                PetsPacket.RemovePet(chr, PetRemoveReason.None, false);
                OnExit?.Invoke(chr, this);
            }

            if (chr.MapChair != -1)
            {
                UsedSeats.Remove(chr.MapChair);
                chr.MapChair = -1;
                MapPacket.SendCharacterSit(chr, -1);
            }

            RemoveController(chr);
            chr.Summons.RemovePuppet();

            Characters.ForEach(p =>
            {
                if (gmhide)
                {
                    if (p.IsGM) return;
                }
                else
                {
                    if (!chr.IsShownTo(p)) return;
                }
                MapPacket.SendCharacterLeavePacket(chr.ID, p);
            });
        }

        public void LeavePlayer(GameCharacter chr)
        {
            // Player exits entirely
            RemovePlayer(chr);

            int newMap;
            // Make sure it isnt dead
            if (chr.HP == 0)
            {
                chr.ModifyHP(50, false);

                // Remove all buffs
                chr.PrimaryStats.Reset(false);

                newMap = ReturnMap;
                if (newMap == Constants.InvalidMap)
                {
                    // Just to make sure the user gets ported back to town
                    newMap = ForcedReturn;
                    if (newMap == Constants.InvalidMap)
                    {
                        newMap = ID;
                    }
                }
            }
            else
            {
                newMap = ForcedReturn;
                if (newMap == Constants.InvalidMap)
                {
                    newMap = ID;
                }
            }


            var map = GameDataProvider.Maps[newMap];
            chr.Field = map;

            // If you did not get kicked out, this should place you on a portal near you.
            if (ForcedReturn == Constants.InvalidMap)
            {
                // Pick the one closest to the user
                chr.PortalID = map.GetClosestStartPoint(chr.Position).ID;
            }
            else
            {
                chr.PortalID = map.GetRandomStartPoint().ID;
            }
        }

        public virtual void AddPlayer(GameCharacter chr)
        {
            PlayersThatHaveBeenHere[chr.Name] = MasterThread.CurrentTime;

            Characters.Add(chr);
            MapTimer(MasterThread.CurrentTime);

            SummonAllLife();
            ShowObjects(chr);

            if (chr.GMHideEnabled)
                AdminPacket.Hide(chr, true);

            OnEnter?.Invoke(chr, this);

            var shownPlayers = Characters.Where(x => !x.IsGM).ToArray();
            if (chr.IsGM && shownPlayers.Length != 0)
            {
                string playersonline = "Players in map (" + shownPlayers.Length + "): \r\n";
                playersonline += string.Join(
                    ", ",
                    shownPlayers.Select(x => x.Name + (x.IsAFK ? " (AFK)" : ""))
                );
                chr.Notice(playersonline);
            }

            // Nuke the stats
            //var nonActiveBuffs = ~chr.PrimaryStats.AllActiveBuffs();
            //BuffPacket.RemoveBuffs(chr, nonActiveBuffs);
        }

        public GameCharacter FindUser(string Name)
        {
            foreach (var User in Characters)
            {
                if (User.Name == Name)
                    return User;
            }
            return Server.Instance.GetCharacter(Name);
        }

        public void SendPacket(Packet packet, GameCharacter skipme = null, bool log = false)
        {
            Characters.ForEach(p =>
            {
                if (p != skipme)
                    p.SendPacket(packet);
                else if (log) { }
            });
        }

        public void SendPacket(IFieldObj Obj, Packet packet, GameCharacter skipme = null)
        {
            Characters.ForEach(p =>
            {
                if (Obj.IsShownTo(p) && p != skipme)
                    p.SendPacket(packet);
            });
        }

        public virtual void SendCharacterEnter(GameCharacter player, GameCharacter victim)
        {
            var pw = MapPacket.CharacterEnterPacket(player);
            victim.SendPacket(pw);
        }

        public void ShowPlayer(GameCharacter chr, bool gmhide)
        {
            var spawneePet = chr.GetSpawnedPet();

            // GMS actually doesn't care wether its you or not. lol.
            Characters
                .Where(c => c != chr)
                .ForEach(c =>
                {
                    // Do not send a packet when they already know its joined
                    if (gmhide && c.IsGM) return;

                    // Show character to P
                    if (chr.IsShownTo(c))
                    {
                        SendCharacterEnter(chr, c);
                        if (spawneePet != null) PetsPacket.SpawnPet(chr, spawneePet, PetSpawnFlags.ShowRemote, c);
                    }

                    // Show P to character
                    if (c.IsShownTo(chr))
                    {
                        SendCharacterEnter(c, chr);

                        var petItem = c.GetSpawnedPet();
                        if (petItem != null) PetsPacket.SpawnPet(c, petItem, PetSpawnFlags.ShowRemote, chr);
                    }
                });

            RedistributeControllers();
        }

        public void ShowObjects(GameCharacter chr)
        {
            if (HasClock)
            {
                var cd = MasterThread.CurrentDate;
                MapPacket.SendMapClock(chr, cd.Hour, cd.Minute, cd.Second);
            }

            SendMapTimer(chr);

            // Reset pet position
            var petItem = chr.GetSpawnedPet();
            if (petItem != null)
            {
                PetSpawnFlags spawnFlags = chr.IsOnline ? PetSpawnFlags.ChangeMap : PetSpawnFlags.Connect;
                PetsPacket.SpawnPet(chr, petItem, spawnFlags, chr);
            }

            NPC.ForEach(n => chr.SendPacket(MapPacket.NpcEnterField(n)));

            Mobs.Values.Where(x => x.HP > 0).ForEach(m =>
            {
                MobPacket.SendMobSpawn(chr, m);
            });

            DropPool.OnEnter(chr);

            // ShowPlayer also redistibutes mobs, we want this
            ShowPlayer(chr, false);

            BalloonRooms.Where(i => i.Value.Opened).ForEach(ps => MiniRoomBalloonPacket.Send(ps.Value.Owner, ps.Value));

            Kites.ForEach(kite => MapPacket.Kite(chr, kite));

            if (WeatherID != 0) MapPacket.SendWeatherEffect(this, chr);
            if (JukeboxID != -1)
            {
                MapPacket.SendJukebox(this, chr);
            }

            ReactorPool.ShowReactorsTo(chr);

            SpawnedMists.Values.ForEach(m => MistPacket.SendMistSpawn(m));

            Summons.ShowAllSummonsTo(chr);

            DoorPool.ShowAllDoorsTo(chr);

            EffectObjects.ForEach((effect) => ShowEffectObjectTo(chr, effect));
        }

        /// <summary>
        /// Send the Map Timer packet to either everybody in the map (chr == null) or the character.
        /// </summary>
        /// <param name="chr">The character to send it to. Can be null to send it to everybody in the map.</param>
        public void SendMapTimer(GameCharacter chr)
        {
            var currentTime = MasterThread.CurrentTime;
            if (currentTime < TimerEndTime)
            {
                var secondsLeft = (TimerEndTime - currentTime) / 1000;
                if (chr != null)
                    MapPacket.ShowMapTimerForCharacter(chr, (int)secondsLeft);
                else
                    MapPacket.ShowMapTimerForMap(this, (int)secondsLeft);
            }
        }

        public bool MakeJukeboxEffect(int itemID, string user, int duration)
        {
            if (JukeboxID != -1) return false;
            JukeboxID = itemID;
            JukeboxUser = user;
            MapPacket.SendJukebox(this, null);

            MasterThread.RepeatingAction.Start(
                "JukeBox Remove Effect " + ID,
                StopJukeboxEffect,
                duration, 0);
            return true;
        }

        public void StopJukeboxEffect()
        {
            JukeboxID = -1;
            JukeboxUser = "";
            MapPacket.SendJukebox(this, null);
        }

        public bool MakeWeatherEffect(int itemID, string message, TimeSpan time, bool admin = false)
        {
            if (WeatherID != 0) return false;
            WeatherID = itemID;
            WeatherMessage = message;
            WeatherIsAdmin = admin;
            MapPacket.SendWeatherEffect(this);

            MasterThread.RepeatingAction.Start(
                "Weather Remove Effect " + ID,
                StopWeatherEffect,
                (long)time.TotalMilliseconds, 0);
            return true;
        }

        public void StopWeatherEffect()
        {
            WeatherID = 0;
            WeatherMessage = "";
            WeatherIsAdmin = false;
            MapPacket.SendWeatherEffect(this);
        }

        public void Reset(bool shuffleReactors)
        {
            // Reset portals
            foreach (var keyValuePair in Portals)
            {
                var portalType = keyValuePair.Value.Type;
                keyValuePair.Value.Enabled = !(portalType == 4 || portalType == 5);
            }

            // Remove mobs
            foreach (var mob in Mobs.Values.ToArray())
                mob.ForceDead();

            initialSpawnDone = false;
            // Create mobs
            SummonAllLife();

            // Remove drops
            DropPool.Clear();

            // Shuffle reactors
            if (shuffleReactors)
            {
                ReactorPool.Shuffle();
            }
        }

        public int DamageAllMobs(GameCharacter chr, int damage)
        {
            int amount = 0;

            try
            {
                var mobs = Mobs.Values.ToArray();

                foreach (var mob in mobs)
                {
                    MobPacket.SendMobDamageOrHeal(this, mob, damage, false, false);
                    if (!mob.GiveDamage(chr, damage))
                    {
                        mob.ForceDead();
                    }
                    amount++;
                }
            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend(ex.ToString());
            }
            return amount;
        }
        public int KillAllMobs(int damage, byte how = 1)
        {
            int amount = 0;

            try
            {
                var mobsBackup = Mobs.Values.ToArray();

                foreach (var mob in mobsBackup)
                {
                    if (damage > 0)
                        MobPacket.SendMobDamageOrHeal(this, mob, damage == 0 ? mob.HP : damage, false, false);

                    mob.ForceDead(how);
                    amount++;
                }
            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend(ex.ToString());
            }
            return amount;
        }

        public void RemoveMob(Mob mob)
        {
            Mobs.Remove(mob.SpawnID);

            if (Mobs.Count == 0)
            {
                ContinentMan.Instance.OnAllSummonedMobRemoved(ID);
            }
        }
        public int SpawnMobWithoutRespawning(
            int mobid,
            Pos position,
            short foothold,
            Mob ownerMob = null,
            SummonType summonType = SummonType.Instant,
            int summonOption = 0,
            bool facesLeft = false
            )
        {
            return SpawnMobWithoutRespawning(mobid, position, foothold, ownerMob, summonType, summonOption, facesLeft, out Mob _);
        }
        public int SpawnMobWithoutRespawning(
            int mobid,
            Pos position,
            short foothold,
            Mob ownerMob,
            SummonType summonType,
            int summonOption,
            bool facesLeft,
            out Mob mob
        ) {
            // Make sure the pos is not through the floor
            position = new Pos(position);
            position.Y -= 2;

            int id = _objectIDs.NextValue();

            mob = new Mob(id, this, mobid, position, foothold, facesLeft);
            mob.Owner = ownerMob;
            if (ownerMob != null)
                ownerMob.Owns[mobid] = mob;
            mob.SummonType = summonType;
            mob.SummonOption = summonOption;
            ownerMob?.SpawnedMobs.Add(id, mob);
            Mobs.Add(mob.SpawnID, mob);

            MobPacket.SendMobSpawn(mob);
            //FindNewController(mob, null);

            //if (mob.SummonType != SummonType.Fake)
            //{
            //    mob.SummonType = SummonType.Fade;
            //    mob.SummonOption = 0;
            //}

            return id;
        }

        public int SpawnMob(
            int mobid,
            MobGenItem mgi,
            Pos position,
            short foothold,
            Mob ownerMob = null,
            SummonType summonType = SummonType.Instant,
            int summonOption = 0)
        {

            if (mobid == 9400505) return -1;

            // Make sure the pos is not through the floor
            position = new Pos(position);
            position.Y -= 2;

            int id = _objectIDs.NextValue();
            Trace.WriteLine($"Spawning mob {mobid} at {position.X} {position.Y}, yes.");
            if (mgi != null && mgi.RegenInterval != 0)
                mgi.MobCount++;

            var mob = new Mob(id, this, mobid, position, foothold);
            mob.Owner = ownerMob;
            mob.MobGenItem = mgi;
            mob.Stance |= (byte)(mgi?.FacesLeft == true ? 1 : 0);
            mob.SummonType = summonType;
            mob.SummonOption = summonOption;

            ownerMob?.SpawnedMobs.Add(id, mob);

            Mobs.Add(mob.SpawnID, mob);

            MobPacket.SendMobSpawn(mob);

            FindNewController(mob, null);

            //if (mob.SummonType != -4)
            //{
            //    mob.SummonType = -1;
            //    mob.SummonOption = 0;
            //}


            return id;
        }

        /// <summary>
        /// Update controllers of mobs
        /// </summary>
        /// <param name="who">When this is NULL, it will find all uncontrolled mobs and allocate them (Same as RedistributeControllers)</param>
        public void RemoveController(GameCharacter who)
        {
            Mobs.Values.Where(x => x.Controller == who).ForEach(x => FindNewController(x, null));
        }

        public void RedistributeControllers()
        {
            Mobs.Values.Where(x => x.IsControlled == false).ForEach(x => FindNewController(x, null));
        }

        public bool FindNewController(Mob mob, GameCharacter wantedCharacter, bool chase = false)
        {
            // This function is not the same as GMS
            // GMS figures out who has the lowest amount of mobs to control

            var currentController = mob.Controller;

            if (wantedCharacter != null)
            {
                // Already the same
                if (currentController == wantedCharacter) return true;
                // Not in current map O.o
                if (!Characters.Contains(wantedCharacter)) return false;

                mob.SetController(wantedCharacter, chase);
                return true;
            }


            // Try to give back the control to the person that did most damage
            var lastHitCharacter = Characters.Find(c =>
                c != currentController &&
                c.IsShownTo(mob) &&
                c.ID == mob.LastHitCharacterID
            );
            if (lastHitCharacter != null)
            {
                mob.SetController(lastHitCharacter, chase);
                return true;
            }

            int maxDistance = 200000;

            // Shuffle the characters so if there are more mobs and players, its better distributed.
            var shuffledPlayers = Characters.ToList();
            shuffledPlayers.Remove(currentController);
            shuffledPlayers.Shuffle();

            // Figure out a player that is in range
            var playerInRange = shuffledPlayers.Find(x =>
                x.IsShownTo(mob) &&
                (x.Position - mob.Position) < maxDistance
            );

            if (playerInRange != null)
            {
                mob.SetController(playerInRange, chase);
                return true;
            }


            // No players found, so deallocate
            mob.RemoveController(true);
            return false;
        }

        public bool IsPointInMBR(int x, int y, bool AsClient)
        {
            Rectangle rc = ((AsClient) ? Rectangle.FromLTRB(this.MBR.Left + 9, this.MBR.Top + 9, this.MBR.Right - 9, this.MBR.Bottom - 9) : MBR);
            return rc.Contains(x, y);
        }

        public IEnumerable<Foothold> SearchFootholds(int x0, int y0, int x1, int y1)
        {
            int Left = Math.Min(x0, x1);
            int Top = Math.Min(y0, y1);
            int Right = Math.Max(x0, x1);
            int Bottom = Math.Max(y0, y1);

            var Area = Rectangle.FromLTRB(Left, Top, Right, Bottom);

            foreach (Foothold Foothold in Footholds)
            {
                if (Foothold.X2 >= Area.Left && Foothold.X1 <= Area.Right && Foothold.Y2 >= Area.Top && Foothold.Y1 <= Area.Bottom)
                    yield return Foothold;
            }
        }

        public Foothold? GetFootholdUnderneath(int X, int Y, out int MaxY)
        {
            Foothold? ClosestFoothold = null;
            int Distance = MaxY = 2147483647;
            var Footholds = SearchFootholds(X - 1, Y - 1, X + 1, 0x7FFFFFFF);

            foreach (Foothold Foothold in Footholds)
            {
                if (Foothold.X1 < Foothold.X2 && Foothold.X1 <= X && Foothold.X2 >= X)
                {
                    Distance = Foothold.Y1 + (X - Foothold.X1) * (Foothold.Y2 - Foothold.Y1) / (Foothold.X2 - Foothold.X1);
                    if (Distance >= Y && Distance < MaxY)
                    {
                        MaxY = Distance;
                        ClosestFoothold = Foothold;
                    }
                }
            }
            return ClosestFoothold;
        }

        public Foothold? GetFootholdClosest(int x, int y, ref int pcx, ref int pcy, int ptHitx)
        {
            Foothold? ClosestFoothold = null;
            int minimum = 2147483647;
            int x2 = 0;

            foreach (Foothold Foothold in Footholds)
            {
                var xDist = 0;
                var yDist = 0;
                if (Foothold.X1 >= Foothold.X2)
                    continue;

                var HitX = ptHitx - x < 0;
                if (ptHitx > x)
                {
                    if (Foothold.X1 < x)
                        continue;
                    HitX = ptHitx - x < 0;
                }

                if (((ptHitx < 0 && HitX) || (ptHitx > 0 && !HitX)) || Foothold.X2 <= x)
                {
                    if (Foothold.Y1 >= y - 100)
                    {
                        if (Foothold.Y2 >= y - 100)
                        {
                            if (ptHitx <= x)
                            {
                                if (ptHitx < x)
                                {
                                    x2 = Foothold.X2;
                                    xDist = x2 - x;
                                }
                                else
                                    xDist = (Foothold.X1 + Foothold.X2) / 2 - x;
                            }
                            else
                            {
                                x2 = Foothold.X1;
                                xDist = x2 - x;
                            }

                            if (ptHitx <= x)
                            {
                                if (ptHitx >= x)
                                    yDist = (Foothold.Y2 + Foothold.Y1) / 2;
                                else
                                    yDist = Foothold.Y2;
                            }
                            else
                                yDist = Foothold.Y1;

                            var dist = xDist * xDist + (yDist - y) * (yDist - y);
                            if (dist < minimum)
                            {
                                var xPos = Foothold.X1;
                                if (x > xPos && (x >= Foothold.X2 || x - (xPos + Foothold.X2) / 2 >= 0))
                                    xPos = Foothold.X2;

                                var yPos = Foothold.Y1 + ((Foothold.Y2 - Foothold.Y1) * (xPos - Foothold.X1) / (Foothold.X2 - Foothold.X1));
                                x2 = MBR.Left + 10;

                                if (xPos <= x2 || xPos >= (x2 = MBR.Right - 10))
                                    xPos = (short)x2;

                                if (IsPointInMBR(xPos, yPos, true))
                                {
                                    pcx = xPos;
                                    pcy = yPos;
                                    minimum = dist;
                                    ClosestFoothold = Foothold;
                                }
                            }
                        }
                    }
                }
            }

            if (ClosestFoothold == null)
            {
                ClosestFoothold = GetFootholdUnderneath(x, y, out pcy);

                x2 = MBR.Left + 10;
                if (x <= x2 || x >= (x2 = MBR.Right - 10))
                    x = x2;
                pcx = x;

                foreach (Foothold Foothold in Footholds)
                {
                    int x1 = 0;
                    if (Foothold.X1 < Foothold.X2)
                    {
                        var x3 = Foothold.X2 + Foothold.X1;
                        long y3 = Foothold.Y1 + Foothold.Y2;
                        var MinY = (x3 / 2 - x) * (x3 / 2 - x) + (((y3 - (int)((y3 >> 32) & 0xffffffff)) >> 1) - y) * (((y3 - (int)((y3 >> 32) & 0xffffffff)) >> 1) - y);
                        if (MinY < minimum)
                        {
                            if (x > Foothold.X1)
                            {
                                if (x < Foothold.X2)
                                {
                                    x2 = Foothold.X2;
                                    if (x - x3 / 2 < 0)
                                        x2 = Foothold.X1;
                                    x1 = x2;
                                }
                                else
                                    x1 = Foothold.X2;
                            }
                            else
                                x1 = Foothold.X1;

                            var Distance = (x1 - Foothold.X1) * (Foothold.Y2 - Foothold.Y1);
                            var y2 = Foothold.Y1 + (Distance / (Foothold.X2 - Foothold.X1));
                            x2 = MBR.Left + 10;

                            if (x1 <= x2 || (x1 >= (x2 = MBR.Right - 10)))
                                x1 = x2;

                            if (IsPointInMBR(x1, y2, true))
                            {
                                pcx = x1;
                                pcy = y2;
                                minimum = (int)MinY;
                                ClosestFoothold = Foothold;
                            }
                        }
                    }
                }
            }

            return ClosestFoothold;
        }

        public Pos FindFloor(Pos mainPos)
        {
            short x = mainPos.X;
            short y = (short)(mainPos.Y - 100);
            short maxy = mainPos.Y;

            bool firstCheck = true;

            foreach (var fh in Footholds)
            {
                if ((x > fh.X1 && x <= fh.X2) || (x > fh.X2 && x <= fh.X1))
                {
                    short cmax = (short)((float)(fh.Y1 - fh.Y2) / (fh.X1 - fh.X2) * (x - fh.X1) + fh.Y1);

                    if ((cmax <= maxy || (maxy == mainPos.Y && firstCheck)) && cmax >= y)
                    {
                        maxy = cmax;
                        firstCheck = false;
                    }
                }
            }

            return new Pos(x, maxy);
        }

        public bool IsCharacterInArea(GameCharacter chr, string areaId)
        {
            if (!Areas.TryGetValue(areaId, out MapArea area)) return false;
            return IsCharacterInArea(chr, area);
        }
        public bool IsCharacterInArea(GameCharacter chr, MapArea area)
        {
            return area.Contains(chr);
        }

        #region Script helpers
        public int CountUserInArea(string areaId)
        {
            if (!Areas.TryGetValue(areaId, out MapArea area)) return 0;
            return Characters.Where(chr => area.Contains(chr)).Count();
        }
        public int IsItemInArea(string areaId, int itemid)
        {
            if (!Areas.TryGetValue(areaId, out MapArea area)) return 0;
            var items = DropPool.Drops.Select(i => i.Value).Where(i => area.Contains(i.Pt1)).ToList();
            if (items.Count > 1) return -1;
            else if (items.Any(i => i.Reward.ItemID == itemid)) return 1;
            else return 0;
        }
        public bool CheckArea(int areas, int minOccupiedAreas, out string answer)
        {
            int occupiedAreas = 0;
            answer = "";
            for (int areaId = 0; areaId < areas; areaId++)
            {
                int cur = CountUserInArea(areaId.ToString());
                occupiedAreas += cur;
                answer += cur.ToString();
            }
            return occupiedAreas == minOccupiedAreas;
        }
        public void SummonMob(short x, short y, int summoningsack)
        {
            if (!GameDataProvider.Items.TryGetValue(summoningsack, out ItemData sack))
            {
                Program.MainForm.LogAppend("Summoning sack {0} not found", summoningsack);
                return;
            }
            var pos = new Pos(x, y);
            var fh = GetFootholdUnderneath(pos.X, pos.Y, out int maxY);
            foreach (var isi in sack.Summons)
            {
                if (GameDataProvider.Mobs.ContainsKey(isi.MobID))
                {
                    if (Rand32.Next() % 100 < isi.Chance)
                    {
                        SpawnMobWithoutRespawning(isi.MobID, pos, (short)(fh?.ID ?? 0), summonType: (SummonType)sack.Type);
                    }
                }
                else
                {
                    Program.MainForm.LogAppend("Summon sack {0} has mobid that doesn't exist: {1}", summoningsack, isi.MobID);
                }
            }
        }
        public void EffectScreen(string screen)
        {
            SendPacket(FieldEffectPacket.EffectScreen(screen));
        }
        public void EffectSound(string sound)
        {
            SendPacket(FieldEffectPacket.EffectSound(sound));
        }
        public void EffectMusic(string music)
        {
            SendPacket(FieldEffectPacket.EffectMusic(music));
        }
        private void ShowEffectObjectTo(GameCharacter to, string effect)
        {
            to.SendPacket(FieldEffectPacket.EffectObject(effect));
        }
        public void EffectObject(string effect)
        {
            EffectObjects.Add(effect);
            SendPacket(FieldEffectPacket.EffectObject(effect));
        }
        public void EnablePortal(string portal, byte enabled)
        {
            Portals[portal].Enabled = enabled == 1;
        }
        public void EffectPartyWrong()
        {
            EffectScreen("quest/party/wrong_kor");
            EffectSound("Party1/Failed");
        }
        public void EffectPartyClear()
        {
            EffectScreen("quest/party/clear");
            EffectSound("Party1/Clear");
        }
        public void Message(string text, BroadcastMessageType type = BroadcastMessageType.RedText)
        {
            ChatPacket.SendBroadcastMessageToMap(this, text, type);
        }
        public void Notice(int type, string text)
        {
            ChatPacket.SendBroadcastMessageToMap(this, text, (BroadcastMessageType)type);
        }
        #endregion
    }
}
