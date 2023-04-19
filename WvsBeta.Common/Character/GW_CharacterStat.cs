using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using WvsBeta.Common.Sessions;
using WvsBeta.Database;

namespace WvsBeta.Common.Character
{
    public class GW_CharacterStat
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public byte Gender { get; set; }
        public byte Skin { get; set; }
        public int Face { get; set; }
        public int Hair { get; set; }

        public long PetCashId { get; set; }

        public byte Level { get; set; }
        public short Job { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Luk { get; set; }
        public short HP { get; set; }
        public short MaxHP { get; set; }
        public short MP { get; set; }
        public short MaxMP { get; set; }
        public short AP { get; set; }
        public short SP { get; set; }
        public int EXP { get; set; }
        public short Fame { get; set; }

        public int MapID { get; set; }
        public byte MapPosition { get; set; }

        public void LoadFromReader(MySqlDataReader data)
        {
            ID = data.GetInt32("id");
            Name = data.GetString("name");
            Gender = data.GetByte("gender");
            Skin = data.GetByte("skin");
            Hair = data.GetInt32("hair");
            Face = data.GetInt32("eyes");

            Level = data.GetByte("level");
            Job = data.GetInt16("job");
            Str = data.GetInt16("str");
            Dex = data.GetInt16("dex");
            Int = data.GetInt16("int");
            Luk = data.GetInt16("luk");
            HP = data.GetInt16("chp");
            MaxHP = data.GetInt16("mhp");
            MP = data.GetInt16("cmp");
            MaxMP = data.GetInt16("mmp");
            AP = data.GetInt16("ap");
            SP = data.GetInt16("sp");
            EXP = data.GetInt32("exp");
            Fame = data.GetInt16("fame");
            MapID = data.GetInt32("map");
            MapPosition = (byte)data.GetInt16("pos");
            PetCashId = data.GetInt64("pet_cash_id");
        }

        public void Encode(Packet pPacket)
        {
            pPacket.WriteInt(ID);
            pPacket.WriteString(Name, 13);

            pPacket.WriteByte(Gender); // Gender
            pPacket.WriteByte(Skin); // Skin
            pPacket.WriteInt(Face); // Face
            pPacket.WriteInt(Hair); // Hair

            pPacket.WriteLong(PetCashId);

            pPacket.WriteByte(Level);
            pPacket.WriteShort(Job);
            pPacket.WriteShort(Str);
            pPacket.WriteShort(Dex);
            pPacket.WriteShort(Int);
            pPacket.WriteShort(Luk);
            pPacket.WriteShort(HP);
            pPacket.WriteShort(MaxHP);
            pPacket.WriteShort(MP);
            pPacket.WriteShort(MaxMP);
            pPacket.WriteShort(AP);
            pPacket.WriteShort(SP);
            pPacket.WriteInt(EXP);
            pPacket.WriteShort(Fame);

            pPacket.WriteInt(MapID);
            pPacket.WriteByte(MapPosition);
        }

        public void Decode(Packet pPacket)
        {
            ID = pPacket.ReadInt();
            Name = pPacket.ReadString(13);

            Gender = pPacket.ReadByte();
            Skin = pPacket.ReadByte();
            Face = pPacket.ReadInt();
            Hair = pPacket.ReadInt();

            PetCashId = pPacket.ReadLong();

            Level = pPacket.ReadByte();
            Job = pPacket.ReadShort();
            Str = pPacket.ReadShort();
            Dex = pPacket.ReadShort();
            Int = pPacket.ReadShort();
            Luk = pPacket.ReadShort();
            HP = pPacket.ReadShort();
            MaxHP = pPacket.ReadShort();
            MP = pPacket.ReadShort();
            MaxMP = pPacket.ReadShort();
            AP = pPacket.ReadShort();
            SP = pPacket.ReadShort();
            EXP = pPacket.ReadInt();
            Fame = pPacket.ReadShort();

            MapID = pPacket.ReadInt();
            MapPosition = pPacket.ReadByte();
        }
    }
}
