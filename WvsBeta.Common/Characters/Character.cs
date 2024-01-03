using log4net;
using System;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common.Characters
{
    public class Character : MovableLife
    {
        public int UserID { get; set; }
        public string Name { get => CharacterStat.Name; set => CharacterStat.Name = value; }
        public int ID { get => CharacterStat.ID; set => CharacterStat.ID = value; }
        public short Job { get => CharacterStat.Job; set => CharacterStat.Job = value; }
        public byte Level { get => CharacterStat.Level; set => CharacterStat.Level = value; }

        public byte Gender { get => CharacterStat.Gender; set => CharacterStat.Gender = value; }
        public byte Skin { get => CharacterStat.Skin; set => CharacterStat.Skin = value; }
        public int Face { get => CharacterStat.Face; set => CharacterStat.Face = value; }
        public int Hair { get => CharacterStat.Hair; set => CharacterStat.Hair = value; }
        public int MapID { get => CharacterStat.MapID; set => CharacterStat.MapID = value; }
        public byte PortalID { get; set; }
        public virtual int PartyID { get; set; }

        public bool IsOnline { get; set; }

        public GMLevel GMLevel { get; set; }
        public bool IsTester { get => GMLevel > GMLevel.None; }
        public bool IsGM { get => GMLevel > GMLevel.Tester; }
        public bool IsAdmin { get => GMLevel >= GMLevel.Admin; }
        public Inventory Inventory { get; set; }
        public CharacterSkills Skills { get; protected set; }
        public virtual CharacterPrimaryStats PrimaryStats { get; protected set; }
        public virtual void DamageHP(short amount) => throw new NotImplementedException();
        public GW_CharacterStat CharacterStat { get; } = new GW_CharacterStat();
        public byte BuddyListCapacity { get; set; }
        public void EncodeForTransfer(Packet pw)
        {
            pw.WriteString(CharacterStat.Name);
            pw.WriteInt(CharacterStat.ID);
            pw.WriteShort(CharacterStat.Job);
            pw.WriteByte(CharacterStat.Level);

            pw.WriteByte(CharacterStat.Gender);
            pw.WriteByte(CharacterStat.Skin);
            pw.WriteInt(CharacterStat.Face);
            pw.WriteInt(CharacterStat.Hair);

            pw.WriteInt(CharacterStat.MapID);
            pw.WriteInt(PartyID);
            pw.WriteBool(IsOnline);
            pw.WriteByte((byte)GMLevel);
        }

        public void DecodeForTransfer(Packet pr)
        {
            CharacterStat.Name = pr.ReadString();
            CharacterStat.ID = pr.ReadInt();
            CharacterStat.Job = pr.ReadShort();
            CharacterStat.Level = pr.ReadByte();

            CharacterStat.Gender = pr.ReadByte();
            CharacterStat.Skin = pr.ReadByte();
            CharacterStat.Face = pr.ReadInt();
            CharacterStat.Hair = pr.ReadInt();

            CharacterStat.MapID = pr.ReadInt();
            PartyID = pr.ReadInt();
            IsOnline = pr.ReadBool();
            GMLevel = (GMLevel)pr.ReadByte();
        }

        public PetItem GetSpawnedPet()
        {
            if (CharacterStat.PetCashId == 0) return null;
            return Inventory.GetItemByCashID(CharacterStat.PetCashId, Common.Enums.InventoryType.Cash) as PetItem;
        }
        public bool InPartyWith(Character chr)
        {
            return PartyID > 0 && PartyID == chr.PartyID;
        }

        #region Character Hack Logic
        public static ILog HackLog = LogManager.GetLogger("HackLog");
        public long LastAttackPacket { get; set; }
        public byte FastAttackHackCount { get; set; }
        public DateTime HacklogMuted { get; set; }
        public int DesyncedSoulArrows { get; set; }
        public byte OutOfMBRCount { get; set; }
        public int MoveTraceCount { get; set; }
        public MovePath.MovementSource MoveTraceSource { get; set; }

        public void TryTraceMovement(MovePath path)
        {
            if (MoveTraceCount <= 0 || MoveTraceSource != path.Source) return;
            path.Dump();
            MoveTraceCount--;
        }
        #endregion
    }
}
