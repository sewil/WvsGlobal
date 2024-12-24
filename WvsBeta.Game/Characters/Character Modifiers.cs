using System;
using log4net;
using WvsBeta.Common;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Objects;
using WvsBeta.Game.Handlers.Guild;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public partial class GameCharacter
    {
        private static ILog _levelLog = LogManager.GetLogger("LevelLog");

        // Do not remove: is used by NPCs
        public byte GetGender() => (byte)Gender;

        public void SetJob(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "job", add = false });
            CharacterStat.Job = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Job);

            this.FlushDamageLog();
            Server.Instance.CenterConnection.UpdatePlayerJobLevel(this);
            if (IsGuildMember)
                GuildHandler.SendUpdatePlayerJob(this);
        }

        public void SetEXP(int value)
        {
            CharacterStat.EXP = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Exp);
        }

        public void SetHPAndMaxHP(short value, bool sendPacket = true)
        {
            if (value <= 0)
            {
                value = 1;
            }

            SetMaxHP(value);
            HP = PrimaryStats.TotalMaxHP;

            if (sendPacket == true)
            {
                CharacterStatsPacket.SendStatChanged(this, StatFlags.Hp);
            }
        }
        public void SetMP(short value)
        {
            if (value > CharacterStat.MaxMP) SetMPAndMaxMP(value);
            else ModifyMP((short)(value - CharacterStat.MP));
        }
        public void SetHP(short value)
        {
            if (value > CharacterStat.MaxHP) SetHPAndMaxHP(value);
            else ModifyHP((short)(value - HP));
        }
        public void ModifyHP(short value, bool sendPacket = true)
        {
            var startValue = HP;
            HP = (short)IntExtensions.Clamp(HP + value, 0, PrimaryStats.TotalMaxHP);

            if (sendPacket)
            {
                CharacterStatsPacket.SendStatChanged(this, StatFlags.Hp);
            }

            if (startValue == HP)
            {
                // Doesn't matter
                return;
            }
            ModifiedHP();
        }

        public override void DamageHP(short amount) => ModifyHP((short)-amount);

        public void ModifiedHP()
        {
            if (HP == 0)
            {
                loseEXP();
                PrimaryStats.Reset(true);
                Summons.RemoveAllSummons();
            }
        }

        public void SetMPAndMaxMP(short value, bool isBySelf = false, bool sendPacket = true)
        {
            SetMaxMP(value);
            CharacterStat.MP = PrimaryStats.TotalMaxMP;

            if (sendPacket == true)
            {
                CharacterStatsPacket.SendStatChanged(this, StatFlags.Mp, isBySelf);
            }
        }

        public void ModifyMP(short value, bool sendPacket = true)
        {
            if ((CharacterStat.MP + value) < 0)
            {
                CharacterStat.MP = 0;
            }
            else if ((CharacterStat.MP + value) > PrimaryStats.TotalMaxMP)
            {
                CharacterStat.MP = PrimaryStats.TotalMaxMP;
            }
            else
            {
                CharacterStat.MP = (short)(CharacterStat.MP + value);
            }
            if (sendPacket)
            {
                CharacterStatsPacket.SendStatChanged(this, StatFlags.Mp);

                /*if (this.PartyID != -1)
                {
                    MapPacket.UpdatePartyMemberMP(this);
                    MapPacket.ReceivePartyMemberMP(this);
                }*/
            }
        }

        public void DamageMP(short amount)
        {
            CharacterStat.MP = (short)(amount > CharacterStat.MP ? 0 : CharacterStat.MP - amount);
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Mp);
        }

        public void ModifyMaxMP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "maxmp", add = true });
            CharacterStat.MaxMP = (short)(((CharacterStat.MaxMP + value) > Constants.MaxMaxMp) ? Constants.MaxMaxMp : (CharacterStat.MaxMP + value));
            CharacterStatsPacket.SendStatChanged(this, StatFlags.MaxMp);
        }

        public void ModifyMaxHP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "maxhp", add = true });
            CharacterStat.MaxHP = (short)(((CharacterStat.MaxHP + value) > Constants.MaxMaxHp) ? Constants.MaxMaxHp : (CharacterStat.MaxHP + value));
            CharacterStatsPacket.SendStatChanged(this, StatFlags.MaxHp);
        }

        public void SetMaxHP(short value)
        {
            var clampedValue = (short)IntExtensions.Clamp(value, Constants.MinMaxHp, Constants.MaxMaxHp);
            _characterLog.Info(new StatChangeLogRecord { value = clampedValue, type = "maxhp", add = false });
            CharacterStat.MaxHP = clampedValue;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.MaxHp);
        }

        public void SetMaxMP(short value)
        {
            var clampedValue = (short)IntExtensions.Clamp(value, Constants.MinMaxMp, Constants.MaxMaxMp);
            _characterLog.Info(new StatChangeLogRecord { value = clampedValue, type = "maxmp", add = false });
            CharacterStat.MaxMP = clampedValue;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.MaxMp);
        }

        public bool LevelUP()
        {
            if (CharacterStat.Level == 200) return false;
            SetLevel((byte)(CharacterStat.Level + 1));
            return true;
        }
        public void SetLevel(byte value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "level", add = false });
            CharacterStat.Level = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Level);
            MapPacket.SendPlayerLevelupAnim(this);

            this.FlushDamageLog();
            Server.Instance.CenterConnection.UpdatePlayerJobLevel(this);
            if (IsGuildMember)
                GuildHandler.SendUpdatePlayerJob(this);
        }

        public void AddFame(short value, bool sendMessage = false)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "fame", add = true });
            var fame = (short)IntExtensions.Clamp(CharacterStat.Fame + value, Constants.MinFame, Constants.MaxFame);
            if (fame == CharacterStat.Fame) return; // Unchanged
            SetFame(fame);
            if (sendMessage)
            {
                SendPacket(MessagePacket.GainFame(value));
            }
        }

        public void SetFame(short value)
        {
            short fame = (short)IntExtensions.Clamp(value, Constants.MinFame, Constants.MaxFame);
            _characterLog.Info(new StatChangeLogRecord { value = fame, type = "fame", add = false });
            CharacterStat.Fame = fame;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Fame);
        }

        private byte lastSaveStep = 0;

        private static byte GetPercentagePerLevelToSave(byte level)
        {
            if (level >= 50) return 2; // Every 2 percent for lvl 50+ peeps
            // Savepoints per level. High levels (>=25) have every 4%. Low levels have 50, 33, 25, 20, etc..
            return (byte)(100 / Math.Min(level, (byte)25));
        }

        private byte CalculateSaveStep()
        {
            var expRequired = Constants.GetLevelEXP(PrimaryStats.Level);
            if (expRequired == 0) return 0;

            var percentage = (byte)(((ulong)CharacterStat.EXP * 100) / (ulong)expRequired);

            var percentagePerLevel = GetPercentagePerLevelToSave(PrimaryStats.Level);

            return (byte)(percentage / percentagePerLevel);
        }

        public void AddEXP(double value, MessageAppearType appearType) { AddEXP((uint)value, appearType); }

        public void AddEXP(uint value, MessageAppearType appearType, bool isSelf = false)
        {
            if (value == 0 || PrimaryStats.Level >= 200 || HP <= 0) return;

            var amount = (int)(value > Int32.MaxValue ? Int32.MaxValue : value);
            var currentExp = (uint)(CharacterStat.EXP + amount);

            SendPacket(MessagePacket.GainEXP(amount, appearType));
            var level = PrimaryStats.Level;

            var save = false;
            var expRequired = Constants.GetLevelEXP(PrimaryStats.Level);

            if (currentExp >= expRequired)
            {
                short apgain = 0;
                short spgain = 0;
                short mpgain = 0;
                short hpgain = 0;
                var job = (short)(CharacterStat.Job / 100);

                var intt = PrimaryStats.GetIntAddition(true);

                currentExp -= (uint)expRequired;

                level++;

                // Update EXP required...
                expRequired = Constants.GetLevelEXP(level);

                apgain += Constants.ApPerLevel;
                hpgain += RNG.Range.generate(
                    Constants.HpMpFormulaArguments[job, 0, (int)Constants.HpMpFormulaFields.HPMin],
                    Constants.HpMpFormulaArguments[job, 0, (int)Constants.HpMpFormulaFields.HPMax],
                    true
                );

                mpgain += RNG.Range.generate(
                    Constants.HpMpFormulaArguments[job, 0, (int)Constants.HpMpFormulaFields.MPMin],
                    Constants.HpMpFormulaArguments[job, 0, (int)Constants.HpMpFormulaFields.MPMax],
                    true
                );

                // Additional buffing through INT stats
                mpgain += (short)(
                    intt *
                    Constants.HpMpFormulaArguments[job, 0, (int)Constants.HpMpFormulaFields.MPIntStatMultiplier] /
                    200
                );

                var improvedMaxHpIncreaseLvl = Skills.GetSkillLevel(Constants.Swordsman.Skills.ImprovedMaxHpIncrease);
                if (improvedMaxHpIncreaseLvl > 0)
                {
                    hpgain += GameCharacterSkills.GetSkillLevelData(Constants.Swordsman.Skills.ImprovedMaxHpIncrease, improvedMaxHpIncreaseLvl).XValue;
                }

                var improvedMaxMpIncreaseLvl = Skills.GetSkillLevel(Constants.Magician.Skills.ImprovedMaxMpIncrease);
                if (improvedMaxMpIncreaseLvl > 0)
                {
                    mpgain += GameCharacterSkills.GetSkillLevelData(Constants.Magician.Skills.ImprovedMaxMpIncrease, improvedMaxMpIncreaseLvl).XValue;
                }

                if (CharacterStat.Job != 0)
                {
                    spgain = Constants.SpPerLevel;
                }

                if (level >= 200)
                {
                    currentExp = 0;
                    // TODO: Announce max level!
                }
                // Overflow? lets reduce it
                else if (currentExp >= expRequired)
                {
                    currentExp = (uint)(expRequired - 1);
                }

                _levelLog.Info(new LevelLogRecord
                {
                    level = level,
                    posX = Position.X,
                    posY = Position.Y,
                });

                ModifyMaxHP(hpgain);
                ModifyMaxMP(mpgain);
                SetLevel(level);
                AddAP(apgain);
                AddSP(spgain);
                ModifyHP(PrimaryStats.TotalMaxHP);
                ModifyMP(PrimaryStats.TotalMaxMP);
                save = true;
            }

            CharacterStat.EXP = (int)currentExp;

            // Calculate savepoints

            var stepOfSave = CalculateSaveStep();
            var curDateTime = MasterThread.CurrentDate;
            if (!save)
            {
                if (lastSaveStep != stepOfSave)
                {
                    var levelTimeSpan = curDateTime - LastSavepoint;

                    if (levelTimeSpan.TotalSeconds >= 30)
                    {
                        _characterLog.Debug(
                            $"Saving because user reached save threshold. Current {stepOfSave} last {lastSaveStep}");
                        save = true;
                        LastSavepoint = curDateTime;
                    }
                    else
                    {
                        AssertForHack(
                            levelTimeSpan.TotalSeconds < 20,
                            $"Getting fast EXP ({levelTimeSpan.TotalSeconds} seconds since last savepoint)",
                            levelTimeSpan.TotalSeconds < 15
                        );
                    }
                    _characterLog.Debug(
                        new SavepointLogRecord
                        {
                            level = PrimaryStats.Level,
                            posX = Position.X,
                            posY = Position.Y,
                            totalMillisBetween = (int)levelTimeSpan.TotalMilliseconds,
                            blocked = save == false
                        }
                    );

                    lastSaveStep = stepOfSave;
                }
            }
            else
            {
                lastSaveStep = stepOfSave;
            }


            if (save)
            {
                LastSavepoint = curDateTime;
                Save();
            }

            CharacterStatsPacket.SendStatChanged(this, StatFlags.Exp, isSelf);
        }

        public bool IncreaseBuddySlots(byte inc)
        {
            if (BuddyListCapacity >= Constants.MaxBuddySlots) return false;
            BuddyListCapacity += inc;
            Server.Instance.CenterConnection.BuddyListExpand(this, inc);
            return true;
        }

        public void AddMaplePoints(int value, GameCharacter chr)
        {
            throw new NotImplementedException();
        }

        public void SetMesos(int value, bool isSelf = false)
        {
            if (value < 0) value = 0;
            Inventory.Mesos = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Mesos, isSelf);
        }

        public void AddAP(short value, bool isSelf = false)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "ap", add = true });
            if (value + CharacterStat.AP > Int16.MaxValue)
            {
                CharacterStat.AP = Int16.MaxValue;
            }
            else
            {
                CharacterStat.AP += value;
            }
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Ap, isSelf);
        }

        public void SetAP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "ap", add = false });
            CharacterStat.AP = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Ap);
        }

        public void AddSP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "sp", add = true });
            if (value + CharacterStat.SP > Int16.MaxValue)
            {
                CharacterStat.SP = Int16.MaxValue;
            }
            else
            {
                CharacterStat.SP += value;
            }
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Sp);
        }

        public void SetSP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "sp", add = false });
            CharacterStat.SP = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Sp);
        }

        public void AddStr(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "str", add = true });
            if (value + CharacterStat.Str > Int16.MaxValue)
            {
                CharacterStat.Str = Int16.MaxValue;
            }
            else
            {
                CharacterStat.Str += value;
            }
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Str);

            this.FlushDamageLog();
        }

        public void SetStr(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "str", add = false });
            CharacterStat.Str = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Str);

            this.FlushDamageLog();
        }

        public void AddDex(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "dex", add = true });
            if (value + CharacterStat.Dex > Int16.MaxValue)
            {
                CharacterStat.Dex = Int16.MaxValue;
            }
            else
            {
                CharacterStat.Dex += value;
            }
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Dex);

            this.FlushDamageLog();
        }

        public void SetDex(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "dex", add = false });
            CharacterStat.Dex = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Dex);

            this.FlushDamageLog();
        }

        public void AddInt(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "int", add = true });
            if (value + CharacterStat.Int > Int16.MaxValue)
            {
                CharacterStat.Int = Int16.MaxValue;
            }
            else
            {
                CharacterStat.Int += value;
            }
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Int);

            this.FlushDamageLog();
        }

        public void SetInt(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "int", add = false });
            CharacterStat.Int = value;

            CharacterStatsPacket.SendStatChanged(this, StatFlags.Int);

            this.FlushDamageLog();
        }

        public void AddLuk(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "luk", add = true });
            if (value + CharacterStat.Luk > Int16.MaxValue)
            {
                CharacterStat.Luk = Int16.MaxValue;
            }
            else
            {
                CharacterStat.Luk += value;
            }
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Luk);

            this.FlushDamageLog();
        }

        public void SetLuk(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "luk", add = false });
            CharacterStat.Luk = value;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Luk);

            this.FlushDamageLog();
        }

        public void loseEXP()
        {
            Item charm = Inventory.GetFirstItem(Common.Enums.InventoryType.Etc, ItemEtcIds.SafetyCharm, ItemEtcIds.EasterCharm);
            if (charm != null)
            {
                bool isSafetyCharm = charm.ItemID == ItemEtcIds.SafetyCharm;
                byte daysLeft = 0;
                byte timesLeft = 0;
                Inventory.TakeItemAmountFromSlot(Common.Enums.InventoryType.Etc, charm.InventorySlot, 1, false);
                if (isSafetyCharm)
                {
                    daysLeft = (byte)Math.Max(0, (charm.Expiration - MasterThread.CurrentTime) / TimeExtensions.DayMillis);
                    timesLeft = (byte)Inventory.GetItemAmount(ItemEtcIds.SafetyCharm);
                }
                PlayerEffectPacket.SendUseEXPCharm(this, isSafetyCharm, charm.ItemID, daysLeft, timesLeft);
            }
            if (CharacterStat.Job == 0 || PrimaryStats.Level >= 200) return;

            double lossPercent;
            if (Field.Town || Field.MobGen.Count == 0)
            {
                lossPercent = 0.01;
            }
            else
            {
                if (CharacterStat.Job / 100 == 3)
                {
                    lossPercent = 0.08;
                }
                else
                {
                    lossPercent = 0.2;
                }
                lossPercent = lossPercent / CharacterStat.Luk + 0.05;
            }
            var levelExp = Constants.GetLevelEXP(PrimaryStats.Level);

            var loseAmount = (levelExp * lossPercent);
            _characterLog.Info($"Player is losing {loseAmount} EXP ({lossPercent}) because of dying.");

            var rExp = (int)(CharacterStat.EXP - loseAmount);
            SetEXP(rExp <= 0 ? 0 : rExp);
        }

        /// <summary>
        /// Update GM hide state.
        /// </summary>
        /// <param name="enabled">Whether to enable GM hide.</param>
        /// <param name="init">
        /// Whether this is sent on player load (Change map or login).
        /// If this is false it will send a Field.RemovePlayer packet to the other players in the map.
        /// </param>
        public void SetGMHide(bool enabled, bool init)
        {
            AdminPacket.Hide(this, enabled);

            if (enabled)
            {
                GMHideEnabled = true;
                if (!init) Field.RemovePlayer(this, true);
            }
            else
            {
                if (GMLevel == GMLevel.GMIntern)
                {
                    Message("GM interns cannot leave GM Hide.");
                    AdminPacket.Hide(this, true); //because client unhides you graphically when server rejects it
                }
                else
                {
                    GMHideEnabled = false;
                    if (!init) Field.ShowPlayer(this, true);
                }
            }
        }


        private void StartChangeMap(Map prevMap, Map newMap)
        {
            prevMap.RemovePlayer(this);
            PortalCount++;
            Field = newMap;
        }

        private void FinishChangeMap(Map prevMap, Map newMap)
        {
            TryActivateHide();
            MapPacket.SendSetField(this, false);

            newMap.AddPlayer(this);
            Summons.MigrateSummons(prevMap, newMap);
            Server.Instance.CenterConnection.PlayerUpdateMap(this);
            PartyHPUpdate();
            CleanupInstances();
        }

        // Change map, but take random spawn
        public void ChangeMap(int mapid)
        {
            ChangeMap(GameDataProvider.Maps[mapid]);
        }
        public void ChangeMap(Map map)
        {
            ChangeMap(map, map.GetRandomStartPoint());
        }

        // Change map, but go to a specific portal
        public void ChangeMap(int toMapId, string toPortalName)
        {
            if (string.IsNullOrWhiteSpace(toPortalName))
            {
                ChangeMap(toMapId);
                return;
            }
            bool isSP = toPortalName == "sp";
            if (!GameDataProvider.Maps.TryGetValue(toMapId, out Map newMap))
            {
                Program.MainForm.LogAppend("Change map failed for unknown map {0}.", toMapId);
            }
            else if (
                (isSP && newMap.SpawnPoints.TryFind(sp => sp.Name == toPortalName, out Portal portal))
                ||
                (!isSP && newMap.Portals.TryGetValue(toPortalName, out portal))
            ) {
                ChangeMap(toMapId, portal);
            }
            else
                Program.MainForm.LogAppend("Did not find portal {0} for mapid {1}", toPortalName, toMapId);
        }

        public void ChangeMap(Map toMap, Portal toPortal)
        {
            var prevmap = Field;
            StartChangeMap(prevmap, toMap);
            {
                PortalID = toPortal.ID;

                Position = new Pos(toPortal.X, (short)(toPortal.Y - 40));
                Stance = 0;
                Foothold = 0;
            }
            FinishChangeMap(prevmap, toMap);
        }
        public void ChangeMap(int toMapId, Portal toPortal)
        {
            Map toMap = GameDataProvider.Maps[toMapId];
            ChangeMap(toMap, toPortal);
        }

        public void ChangeMap(int mapid, byte partyMemberIdx, MysticDoor door)
        {
            var prevmap = Field;
            var newMap = GameDataProvider.Maps[mapid];

            StartChangeMap(prevmap, newMap);
            {
                // Something magical happens I suppose.
                // However, the server doesnt update the position
                // When a random portal is assigned, which would trigger a hack check.
                // So we are a bit clueless...
                PortalID = (byte)(partyMemberIdx | (1 << 7));
                if (newMap.Town)
                {
                    Portal endingAt;
                    if (newMap.DoorPoints.Count > partyMemberIdx)
                    {
                        // Pick the one for the index
                        endingAt = newMap.DoorPoints[partyMemberIdx];
                    }
                    else
                    {
                        // Random.
                        endingAt = newMap.GetRandomStartPoint();
                    }
                    Position = new Pos(endingAt.X, endingAt.Y);
                }
                else
                {
                    Position = new Pos(door.X, door.Y);
                }

                Stance = 0;
                Foothold = 0;
            }
            FinishChangeMap(prevmap, newMap);
        }

        public void SetHair(int id)
        {
            _characterLog.Info(new StatChangeLogRecord { value = id, type = "hair", add = false });
            Hair = id;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Hair);
            MapPacket.SendAvatarModified(this, MapPacket.AvatarModFlag.AvatarLook);//Because hair is a equip I guess
        }

        public void SetFace(int id)
        {
            _characterLog.Info(new StatChangeLogRecord { value = id, type = "face", add = false });
            Face = id;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Eyes);
            MapPacket.SendAvatarModified(this, MapPacket.AvatarModFlag.AvatarLook);
        }

        public void SetSkin(byte id)
        {
            _characterLog.Info(new StatChangeLogRecord { value = id, type = "skin", add = false });
            Skin = id;
            CharacterStatsPacket.SendStatChanged(this, StatFlags.Skin);
            MapPacket.SendAvatarModified(this, MapPacket.AvatarModFlag.AvatarLook);
        }
    }
}