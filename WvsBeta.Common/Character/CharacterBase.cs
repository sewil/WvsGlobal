using System;
using WvsBeta.Common.Character;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common
{
    public class CharacterBase : MovableLife
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public short Job { get; set; } // Center is the only server that uses this for jobs. Game uses PrimaryStats for job.
        public byte Level { get; set; } // Center is the only server that uses this for level. Game uses PrimaryStats for level.

        public byte Gender { get; set; }
        public byte Skin { get; set; }
        public int Face { get; set; }
        public int Hair { get; set; }
        public long PetCashId { get; set; }

        public virtual int MapID { get; set; }
        public byte MapPosition { get; set; }
        public virtual int PartyID { get; set; }

        public bool IsOnline { get; set; }

        public byte GMLevel { get; set; }
        public bool IsGM { get => GMLevel > 0; }
        public bool IsAdmin { get => GMLevel >= 3; }
        public virtual BaseCharacterInventory Inventory { get; set; }
        public virtual BaseCharacterSkills Skills { get; protected set; }
        public virtual BaseCharacterPrimaryStats PrimaryStats { get; protected set; }
        public virtual BaseCharacterQuests Quests { get; protected set; }
        public virtual void DamageHP(short amount) => throw new NotImplementedException();

        public void EncodeForTransfer(Packet pw)
        {
            pw.WriteString(Name);
            pw.WriteInt(ID);
            pw.WriteShort(Job);
            pw.WriteByte(Level);

            pw.WriteByte(Gender);
            pw.WriteByte(Skin);
            pw.WriteInt(Face);
            pw.WriteInt(Hair);

            pw.WriteInt(MapID);
            pw.WriteInt(PartyID);
            pw.WriteBool(IsOnline);
            pw.WriteByte(GMLevel);
        }


        public void DecodeForTransfer(Packet pr)
        {
            Name = pr.ReadString();
            ID = pr.ReadInt();
            Job = pr.ReadShort();
            Level = pr.ReadByte();

            Gender = pr.ReadByte();
            Skin = pr.ReadByte();
            Face = pr.ReadInt();
            Hair = pr.ReadInt();

            MapID = pr.ReadInt();
            PartyID = pr.ReadInt();
            IsOnline = pr.ReadBool();
            GMLevel = pr.ReadByte();
        }

        public GW_CharacterStat ToGWStat()
        {
            return new GW_CharacterStat
            {
                ID = ID,
                Name = Name,
                Gender = Gender,
                Skin = Skin,
                Face = Face,
                Hair = Hair,

                PetCashId = PetCashId,

                Level = Level,
                Job = Job,
                Str = PrimaryStats.Str,
                Dex = PrimaryStats.Dex,
                Int = PrimaryStats.Int,
                Luk = PrimaryStats.Luk,
                HP = PrimaryStats.HP,
                MaxHP = PrimaryStats.GetMaxHP(true),
                MP = PrimaryStats.MP,
                MaxMP = PrimaryStats.GetMaxMP(true),
                AP = PrimaryStats.AP,
                SP = PrimaryStats.SP,
                EXP = PrimaryStats.EXP,
                Fame = PrimaryStats.Fame,

                MapID = MapID,
                MapPosition = MapPosition
            };
        }
    }
}
