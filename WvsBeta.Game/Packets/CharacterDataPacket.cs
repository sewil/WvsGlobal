using System;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.Packets
{
    internal class CharacterDataPacket
    {
        enum DataFlag : short
        {
            None = 0,
            All = -1,
            StatsAndSkills = 1,
            MesosAndQuests = 2,
            Equip = 4,
            Use = 8,
            SetupAndTeleRock = 16,
            Etc = 64,
            PetAndRPSGames = 32,
            Skill = 128
        }
        Character chr;
        public CharacterDataPacket(Character chr)
        {
            this.chr = chr;
        }
        public void Encode(Packet packet)
        {
            DataFlag flag = DataFlag.All;
            //DataFlag flag = DataFlag.Stats|DataFlag.Mesos|DataFlag.Equip;
            packet.WriteShort((short)flag);

            if ((flag & DataFlag.StatsAndSkills) != 0)
            {
                chr.ToGWStat().Encode(packet);
                packet.WriteByte((byte)chr.PrimaryStats.BuddyListCapacity);
            }
            if ((flag & DataFlag.MesosAndQuests) != 0)
            {
                packet.WriteInt(chr.Inventory.Mesos);
            }

            // Write max slots
            for (int i = 0; i < 5; i++)
            {
                var invFlag = (DataFlag)Math.Pow(2, i + 2); // Inventory flag
                if ((flag & invFlag) != 0)
                {
                    packet.WriteByte(chr.Inventory.MaxSlots[i]);
                }
            }

            if ((flag & DataFlag.Equip) != 0)
            {
                // Equip window
                foreach (var item in chr.Inventory.Equips[0]) // Normal equips
                {
                    if (item == null) continue;
                    item.Encode(packet, false);
                }

                packet.WriteByte(0);

                foreach (var item in chr.Inventory.Equips[1]) // Cash equips
                {
                    if (item == null) continue;
                    item.Encode(packet, false);
                }

                packet.WriteByte(0);
            }


            // Inventory window
            for (int i = 0; i < 5; i++)
            {
                var invFlag = (DataFlag)Math.Pow(2, i + 2); // Check inventory flag
                if ((flag & invFlag) == 0)
                {
                    continue;
                }

                foreach (BaseItem item in chr.Inventory.Items[i])
                {
                    if (item != null && item.InventorySlot > 0)
                    {
                        item.Encode(packet, false);
                    }
                }

                packet.WriteByte(0);
            }

            // Skills
            if ((flag & DataFlag.StatsAndSkills) != 0)
            {
                chr.Skills.AddSkills(packet);
            }

            if ((flag & DataFlag.MesosAndQuests) != 0)
            {
                var questsWithData = chr.Quests.Quests;
                packet.WriteShort((short)questsWithData.Count); // Running quests
                foreach (var kvp in questsWithData)
                {
                    packet.WriteInt(kvp.Key);
                    packet.WriteString(kvp.Value.Data);
                }
            }

            if ((flag & DataFlag.PetAndRPSGames) != 0)
            {
                packet.WriteShort(0); // RPS Game(s)
            }


            packet.WriteShort(0); // ?
            packet.WriteShort(0); // ?, decodes 33u buffer wut

            if ((flag & DataFlag.SetupAndTeleRock) != 0)
            {
                chr.Inventory.AddRockPacket(packet);
            }
        }
    }
}
