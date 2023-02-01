using System;
using System.Collections.Generic;
using log4net;
using WvsBeta.Common;
using WvsBeta.Game.Packets;

namespace WvsBeta.Game
{
    public partial class GameCharacter
    {
        private static ILog _levelLog = LogManager.GetLogger("LevelLog");

        // Do not remove: is used by NPCs
        public byte GetGender() => Gender;

        public void SetJob(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "job", add = false });
            CharacterStat.Job = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Job, value);

            this.FlushDamageLog();
            Server.Instance.CenterConnection.UpdatePlayerJobLevel(this);
        }

        public void SetEXP(int value)
        {
            CharacterStat.EXP = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Exp, value);
        }

        public void SetHPAndMaxHP(short value, bool sendPacket = true)
        {
            if (value <= 0)
            {
                value = 1;
            }

            SetMaxHP(value);
            HP = value;

            if (sendPacket == true)
            {
                CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Hp, value);
            }
        }

        public void ModifyHP(short value, bool sendPacket = true)
        {
            var startValue = HP;

            if ((HP + value) < 0)
            {
                HP = 0;
            }
            else if ((HP + value) > PrimaryStats.GetMaxHP(false))
            {
                HP = PrimaryStats.GetMaxHP(false);
            }
            else
            {
                HP = (short)(HP + value);
            }


            if (sendPacket)
            {
                CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Hp, HP);
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
            if (value < 0)
            {
                value = 0;
            }

            SetMaxMP(value);
            CharacterStat.MP = value;

            if (sendPacket == true)
            {
                CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Mp, value, isBySelf);
            }
        }

        public void ModifyMP(short value, bool sendPacket = true)
        {
            if ((CharacterStat.MP + value) < 0)
            {
                CharacterStat.MP = 0;
            }
            else if ((CharacterStat.MP + value) > PrimaryStats.GetMaxMP(false))
            {
                CharacterStat.MP = PrimaryStats.GetMaxMP(false);
            }
            else
            {
                CharacterStat.MP = (short)(CharacterStat.MP + value);
            }
            if (sendPacket)
            {
                CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Mp, CharacterStat.MP);

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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Mp, CharacterStat.MP, false);
        }

        public void ModifyMaxMP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "maxmp", add = true });
            CharacterStat.MaxMP = (short)(((CharacterStat.MaxMP + value) > Constants.MaxMaxMp) ? Constants.MaxMaxMp : (CharacterStat.MaxMP + value));
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.MaxMp, CharacterStat.MaxMP);
        }

        public void ModifyMaxHP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "maxhp", add = true });
            CharacterStat.MaxHP = (short)(((CharacterStat.MaxHP + value) > Constants.MaxMaxHp) ? Constants.MaxMaxHp : (CharacterStat.MaxHP + value));
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.MaxHp, CharacterStat.MaxHP);
        }

        public void SetMaxHP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "maxhp", add = false });
            if (value > Constants.MaxMaxHp) value = Constants.MaxMaxHp;
            else if (value < Constants.MinMaxHp) value = Constants.MinMaxHp;
            CharacterStat.MaxHP = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.MaxHp, value);
        }

        public void SetMaxMP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "maxmp", add = false });
            if (value > Constants.MaxMaxMp) value = Constants.MaxMaxMp;
            else if (value < Constants.MinMaxMp) value = Constants.MinMaxMp;
            CharacterStat.MaxMP = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.MaxMp, value);
        }

        public void SetLevel(byte value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "level", add = false });
            CharacterStat.Level = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Level, value);
            MapPacket.SendPlayerLevelupAnim(this);

            this.FlushDamageLog();
            Server.Instance.CenterConnection.UpdatePlayerJobLevel(this);
        }

        public void AddFame(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "fame", add = true });
            if (CharacterStat.Fame + value > Int16.MaxValue)
            {
                SetFame(Int16.MaxValue);
            }
            else
            {
                SetFame((short)(CharacterStat.Fame + value));
            }
        }

        public void SetFame(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "fame", add = false });
            CharacterStat.Fame = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Fame, value);
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

        public void AddEXP(double value, bool Gray = false, bool Quest = false) { AddEXP((uint)value, Gray, Quest); }

        public void AddEXP(uint value, bool IsLastHit = false, bool Quest = false)
        {
            if (value == 0 || PrimaryStats.Level >= 200 || HP <= 0) return;

            var amount = (int)(value > Int32.MaxValue ? Int32.MaxValue : value);
            var amnt = (uint)(CharacterStat.EXP + amount);

            CharacterStatsPacket.SendGainEXP(this, amount, IsLastHit, Quest);
            var level = PrimaryStats.Level;

            var save = false;
            var expRequired = Constants.GetLevelEXP(PrimaryStats.Level);

            if (amnt >= expRequired)
            {
                short apgain = 0;
                short spgain = 0;
                short mpgain = 0;
                short hpgain = 0;
                var job = (short)(CharacterStat.Job / 100);

                var intt = PrimaryStats.GetIntAddition(true);

                amnt -= (uint)expRequired;

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
                    hpgain += CharacterSkills.GetSkillLevelData(Constants.Swordsman.Skills.ImprovedMaxHpIncrease, improvedMaxHpIncreaseLvl).XValue;
                }

                var improvedMaxMpIncreaseLvl = Skills.GetSkillLevel(Constants.Magician.Skills.ImprovedMaxMpIncrease);
                if (improvedMaxMpIncreaseLvl > 0)
                {
                    mpgain += CharacterSkills.GetSkillLevelData(Constants.Magician.Skills.ImprovedMaxMpIncrease, improvedMaxMpIncreaseLvl).XValue;
                }

                if (CharacterStat.Job != 0)
                {
                    spgain = Constants.SpPerLevel;
                }

                if (level >= 200)
                {
                    amnt = 0;
                    // TODO: Announce max level!
                }

                // Overflow? lets reduce it
                if (amnt >= expRequired)
                {
                    amnt = (uint)(expRequired - 1);
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
                ModifyHP(PrimaryStats.GetMaxHP(false));
                ModifyMP(PrimaryStats.GetMaxMP(false));
                save = true;
            }

            CharacterStat.EXP = (int)amnt;

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

            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Exp, CharacterStat.EXP);
        }

        public void IncreaseBuddySlots()
        {
            Server.Instance.CenterConnection.BuddyListExpand(this);
        }

        public bool TryExchange(int mesos, int itemId, short itemAmount)
        {
            if (mesos < 0 && Inventory.Mesos < Math.Abs(mesos)) return false;
            if (Inventory.AddNewItem(itemId, itemAmount) == 0) return false;
            AddMesos(mesos);
            return true;
        }
        public bool TryExchange(int mesos)
        {
            return TryAddMesos(mesos);
        }
        public bool TryAddMesos(int value, bool isSelf = false)
        {
            if (value < 0 && Inventory.Mesos < Math.Abs(value)) return false;
            AddMesos(value, isSelf, out int _);
            return true;
        }
        public void AddMesos(int value, bool isSelf = false)
        {
            AddMesos(value, isSelf, out int _);
        }
        public void AddMesos(int value, bool isSelf, out int mesosDiff)
        {
            int newMesos = Math.Max(0, Math.Min(int.MaxValue, Inventory.Mesos + value));
            mesosDiff = newMesos - Inventory.Mesos;
            Inventory.Mesos = newMesos;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Mesos, Inventory.Mesos, isSelf);
        }

        public void AddMaplePoints(int value, GameCharacter chr)
        {
            throw new NotImplementedException();
        }

        public void SetMesos(int value, bool isSelf = false)
        {
            if (value < 0) value = 0;
            Inventory.Mesos = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Mesos, Inventory.Mesos, isSelf);
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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Ap, CharacterStat.AP, isSelf);
        }

        public void SetAP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "ap", add = false });
            CharacterStat.AP = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Ap, CharacterStat.AP);
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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Sp, CharacterStat.SP);
        }

        public void SetSP(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "sp", add = false });
            CharacterStat.SP = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Sp, CharacterStat.SP);
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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Str, CharacterStat.Str);

            this.FlushDamageLog();
        }

        public void SetStr(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "str", add = false });
            CharacterStat.Str = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Str, CharacterStat.Str);

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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Dex, CharacterStat.Dex);

            this.FlushDamageLog();
        }

        public void SetDex(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "dex", add = false });
            CharacterStat.Dex = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Dex, CharacterStat.Dex);

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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Int, CharacterStat.Int);

            this.FlushDamageLog();
        }

        public void SetInt(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "int", add = false });
            CharacterStat.Int = value;

            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Int, CharacterStat.Int);

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
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Luk, CharacterStat.Luk);

            this.FlushDamageLog();
        }

        public void SetLuk(short value)
        {
            _characterLog.Info(new StatChangeLogRecord { value = value, type = "luk", add = false });
            CharacterStat.Luk = value;
            CharacterStatsPacket.SendStatChange(this, (uint)CharacterStatsPacket.StatFlags.Luk, CharacterStat.Luk);

            this.FlushDamageLog();
        }

        public void loseEXP()
        {
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

        public void SetHide(bool Hidden, bool init)
        {
            AdminPacket.Hide(this, Hidden);

            if (Hidden)
            {
                GMHideEnabled = true;
                if (!init) Field.RemovePlayer(this, true);
            }
            else
            {
                if (GMLevel == 1)
                {
                    ChatPacket.SendNotice("GM interns cannot leave GM Hide.", this);
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
            CleanupInstances();

            prevMap.RemovePlayer(this);

            PortalCount++;
            Field = newMap;
        }

        private void FinishChangeMap(Map prevMap, Map newMap)
        {
            TryActivateHide();
            MapPacket.SendChangeMap(this);

            if (newMap.PQPortalOpen)
            {
                Field.SendPacket(FieldEffectPacket.QuestEffect());
            }
            newMap.AddPlayer(this);
            Summons.MigrateSummons(prevMap, newMap);
            Server.Instance.CenterConnection.PlayerUpdateMap(this);
            PartyHPUpdate();
        }

        // Change map, but take random spawn
        public void ChangeMap(int mapid)
        {
            ChangeMap(mapid, DataProvider.Maps[mapid].GetRandomStartPoint());
        }

        // Change map, but go to a specific portal
        public void ChangeMap(int toMapId, string toPortalName)
        {
            var newMap = DataProvider.Maps[toMapId];
            if (newMap.Portals.TryGetValue(toPortalName, out var portal))
                ChangeMap(toMapId, portal);
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
            Map toMap = DataProvider.Maps[toMapId];
            ChangeMap(toMap, toPortal);
        }

        public void ChangeMap(int mapid, byte partyMemberIdx, MysticDoor door)
        {
            var prevmap = Field;
            var newMap = DataProvider.Maps[mapid];

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
            CharacterStatsPacket.SendStatChange(this, (int)CharacterStatsPacket.StatFlags.Hair, id);
            MapPacket.SendAvatarModified(this, MapPacket.AvatarModFlag.AvatarLook);//Because hair is a equip I guess
        }

        public void SetFace(int id)
        {
            _characterLog.Info(new StatChangeLogRecord { value = id, type = "face", add = false });
            Face = id;
            CharacterStatsPacket.SendStatChange(this, (int)CharacterStatsPacket.StatFlags.Eyes, id);
            MapPacket.SendAvatarModified(this, MapPacket.AvatarModFlag.AvatarLook);
        }

        public void SetSkin(byte id)
        {
            _characterLog.Info(new StatChangeLogRecord { value = id, type = "skin", add = false });
            Skin = id;
            CharacterStatsPacket.SendStatChange(this, (byte)CharacterStatsPacket.StatFlags.Skin, id);
            MapPacket.SendAvatarModified(this, MapPacket.AvatarModFlag.AvatarLook);
        }

        private byte ParseGenderString(string input)
        {
            switch (input.ToLower())
            {
                case "2":
                case "unisex":
                case "u": return 2;
                case "1":
                case "female":
                case "f": return 1;
                default: return 0;
            }
        }

        public void OnVarset(GameCharacter Sent, string Var, object Value, object Value2 = null, object Value3 = null)
        {
            if (this != Sent && Sent.IsGM && !Sent.IsAdmin) //Todo Admin levels
            {
                ChatPacket.SendNotice("You don't have the premission to edit other players stats!", Sent);
                //$"{Sent.Name} tried to edit another players stats without premission"
            }
            else
            {
                try
                {
                    MapPacket.AvatarModFlag AvatarMod = 0;
                    CharacterStatsPacket.StatFlags dwFlag = 0;

                    switch (Var.ToLower())
                    {
                        case "hp":
                            dwFlag |= CharacterStatsPacket.StatFlags.Hp;
                            HP = Convert.ToInt16(Value);
                            break;
                        case "mp":
                            dwFlag |= CharacterStatsPacket.StatFlags.Mp;
                            CharacterStat.MP = Convert.ToInt16(Value);
                            break;
                        case "exp":
                            dwFlag |= CharacterStatsPacket.StatFlags.Exp;
                            CharacterStat.EXP = Convert.ToInt32(Value);
                            break;
                        case "maxhp":
                            dwFlag |= CharacterStatsPacket.StatFlags.MaxHp;
                            if (Value.ToString() == "0")
                                Value = "1";
                            CharacterStat.MaxHP = Convert.ToInt16(Value);
                            break;
                        case "maxmp":
                            dwFlag |= CharacterStatsPacket.StatFlags.MaxMp;
                            if (Value.ToString() == "0")
                                Value = "1";
                            CharacterStat.MaxMP = Convert.ToInt16(Value);
                            break;
                        case "ap":
                            dwFlag |= CharacterStatsPacket.StatFlags.Ap;
                            CharacterStat.AP = Convert.ToInt16(Value);
                            break;
                        case "sp":
                            dwFlag |= CharacterStatsPacket.StatFlags.Sp;
                            CharacterStat.SP = Convert.ToInt16(Value);
                            break;
                        case "str":
                            dwFlag |= CharacterStatsPacket.StatFlags.Str;
                            CharacterStat.Str = Convert.ToInt16(Value);
                            break;
                        case "dex":
                            dwFlag |= CharacterStatsPacket.StatFlags.Dex;
                            CharacterStat.Dex = Convert.ToInt16(Value);
                            break;
                        case "int":
                            dwFlag |= CharacterStatsPacket.StatFlags.Int;
                            CharacterStat.Int = Convert.ToInt16(Value);
                            break;
                        case "luk":
                            dwFlag |= CharacterStatsPacket.StatFlags.Luk;
                            CharacterStat.Luk = Convert.ToInt16(Value);
                            break;
                        case "fame":
                        case "pop":
                            dwFlag |= CharacterStatsPacket.StatFlags.Fame;
                            CharacterStat.Fame = Convert.ToInt16(Value);
                            break;
                        case "mesos":
                            dwFlag |= CharacterStatsPacket.StatFlags.Mesos;
                            Inventory.Mesos = Convert.ToInt32(Value);
                            break;
                        case "job":
                            {
                                var Job = Convert.ToInt16(Value);
                                if (DataProvider.HasJob(Job) || Job == 0)
                                {
                                    dwFlag |= CharacterStatsPacket.StatFlags.Job;
                                    CharacterStat.Job = Job;
                                }
                                else
                                    ChatPacket.SendNotice($"Job {Job} does not exist.", Sent);
                                break;
                            }
                        case "skill":
                            {
                                var SkillID = Convert.ToInt32(Value);
                                if (DataProvider.Skills.TryGetValue(SkillID, out var Skill))
                                {
                                    if (Value2 == null)
                                        Value2 = Skill.MaxLevel;
                                    Skills.SetSkillPoint(SkillID, Convert.ToByte(Value2), true);
                                }
                                else
                                    ChatPacket.SendNotice($"Skill {SkillID} does not exist.", Sent);
                                break;
                            }
                        case "level":
                            dwFlag |= CharacterStatsPacket.StatFlags.Level;
                            Level = Convert.ToByte(Value);
                            MapPacket.SendPlayerLevelupAnim(this);
                            break;
                        case "skin":
                            {
                                var SkinID = Convert.ToByte(Value);
                                if (SkinID >= 0 && SkinID < 6)
                                {
                                    AvatarMod |= MapPacket.AvatarModFlag.AvatarLook;
                                    dwFlag |= CharacterStatsPacket.StatFlags.Skin;
                                    Skin = SkinID;
                                }
                                else
                                    ChatPacket.SendNotice($"Skin {SkinID} does not exist.", Sent);
                                break;
                            }
                        case "face":
                            {
                                var FaceID = Convert.ToInt32(Value);
                                if (DataProvider.Equips.ContainsKey(FaceID))
                                {
                                    AvatarMod |= MapPacket.AvatarModFlag.AvatarLook;
                                    dwFlag |= CharacterStatsPacket.StatFlags.Eyes;
                                    Face = FaceID;
                                }
                                else
                                    ChatPacket.SendNotice($"Face {FaceID} does not exist.", Sent);
                                break;
                            }
                        case "hair":
                            {
                                var HairID = Convert.ToInt32(Value);
                                if (DataProvider.Equips.ContainsKey(HairID))
                                {
                                    AvatarMod |= MapPacket.AvatarModFlag.AvatarLook;
                                    dwFlag |= CharacterStatsPacket.StatFlags.Hair;
                                    Hair = HairID;
                                }
                                else
                                    ChatPacket.SendNotice($"Hair {HairID} does not exist.", Sent);
                                break;
                            }
                        case "gender":
                            {
                                Gender = ParseGenderString(Value.ToString());
                                Server.Instance.CharacterDatabase.RunQuery(
                                    "UPDATE characters SET gender = @gender WHERE id = @id",
                                    "@gender", Gender,
                                    "@id", ID
                                );

                                ChatPacket.SendNotice($"Gender set to {(Gender == 0 ? "male" : (Gender == 2 ? "Unisex" : "female"))}. Please relog.", this);
                                break;
                            }
                        case "accgender":
                            {
                                var gender = ParseGenderString(Value.ToString());
                                Server.Instance.CharacterDatabase.RunQuery(
                                    "UPDATE users SET gender = @gender WHERE ID = @id",
                                    "@gender", gender,
                                    "@id", UserID
                                );

                                ChatPacket.SendNotice($"Account gender set to {(gender == 0 ? "male" : (gender == 2 ? "Unisex" : "female"))}", this);
                                break;
                            }
                        case "map":
                        case "field":
                            {
                                var FieldID = Convert.ToInt32(Value);
                                if (DataProvider.Maps.ContainsKey(FieldID))
                                    ChangeMap(FieldID);
                                else
                                    ChatPacket.SendText(ChatPacket.MessageTypes.RedText, "Map not found.", this, ChatPacket.MessageMode.ToPlayer);
                                break;
                            }
                        default:
                            ChatPacket.SendNotice($"{Var} is not a valid Variable!", Sent);
                            return;
                    }

                    if (dwFlag != 0)
                        CharacterStatsPacket.SendUpdateStat(this, true, dwFlag);

                    if (AvatarMod != 0)
                        MapPacket.SendAvatarModified(this, AvatarMod);
                }
                catch (Exception ex)
                {
                    ChatPacket.SendNotice(ex.Message, Sent);
                }
            }
        }

        public void OnPetVarset(string Var, string Value, bool Me)
        {
            ChatPacket.SendNotice("Did you hear a cat just now? That damn thing haunts me.", this);
        }
    }
}