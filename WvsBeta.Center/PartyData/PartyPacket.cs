using System.Diagnostics;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Center
{
    public enum PartyFunctionError : byte
    {
        CREATE_NEW_ALREADY_JOINED = 0x08,
        CREATE_NEW_BEGINNER_DISALLOWED = 0x09,
        CREATE_NEW_UNK_ERR = 0xA,
        WITHDRAW_NOT_JOINED = 0xC,
        JOIN_ALREADY_JOINED = 0xF,
        JOIN_ALREADY_FULL = 0x10,
        JOIN_PARTY_UNK_USER = 0x11,
        INVITE_BLOCKED = 0x13,
        INVITE_USER_ALREADY_HAS_INVITE = 0x14,
        INVITE_REJECTED = 0x15,
        ADMIN_CANNOT_INVITE = 0x17,
        ADMIN_CANNOT_CREATE = 0x18,
        UNABLE_TO_FIND_PLAYER = 0x19,
    }
    public enum PartyFunction : byte
    {
        INVITE_DONE = 0x04,
        LOAD_DONE = 0x06,
        CREATE_NEW_DONE = 0x07,
        WITHDRAW_DONE = 0xB,
        WITHDRAW_UNK = 0xD,
        JOIN_DONE = 0xE,
        TOWN_PORTAL_CHANGED_UNK = 0x1A,
        TOWN_PORTAL_CHANGED = 0x1B,
    }

    public class PartyPacket : Packet
    {
        public const int CHANNEL_ID_OFFLINE = -2;

        private PartyPacket(PartyFunction function, int pid) : base(ServerMessages.PARTY_RESULT)
        {
            WriteByte((byte)function);
            WriteInt(pid);
        }
        private PartyPacket(PartyFunction function, byte idx) : base(ServerMessages.PARTY_RESULT)
        {
            WriteByte((byte)function);
            WriteByte(idx);
        }
        private PartyPacket(PartyFunctionError function) : base(ServerMessages.PARTY_RESULT)
        {
            WriteByte((byte)function);
        }
        public static PartyPacket PartyCreated(int charid, int partyid)
        {
            var pw = new PartyPacket(PartyFunction.CREATE_NEW_DONE, partyid);
            pw.WriteInt(Constants.InvalidMap); // Mystic Door's town ID
            pw.WriteInt(Constants.InvalidMap);  // Mystic Door's Target ID
            pw.WriteShort(0); // Mystic Door's X Position
            pw.WriteShort(0); // Mystic Door's Y Position
            return pw;
        }

        public static PartyPacket Error(PartyFunctionError error, string name = null)
        {
            var pw = new PartyPacket(error);
            if (!string.IsNullOrWhiteSpace(name))
            {
                pw.WriteString(name);
            }
            return pw;
        }

        public static PartyPacket PartyInvite(Party pt)
        {
            var pw = new PartyPacket(PartyFunction.INVITE_DONE, pt.partyId);
            pw.WriteString(pt.leader.name);
            return pw;
        }

        public static PartyPacket TownPortalChanged(DoorInformation door, byte idx)
        {
            var pw = new PartyPacket(PartyFunction.TOWN_PORTAL_CHANGED, idx);
            Trace.WriteLine($"PartyData dest: {door.DstMap} src: {door.SrcMap} x:{door.X} y: {door.Y}");
            pw.WriteInt(door.DstMap);
            pw.WriteInt(door.SrcMap);
            pw.WriteShort(door.X);
            pw.WriteShort(door.Y);
            return pw;
        }
        public static PartyPacket Join(Party pt, PartyMember joined)
        {
            var pw = new PartyPacket(PartyFunction.JOIN_DONE, pt.partyId);
            pw.WriteString(joined.name);
            EncodePartyData(pw, joined, pt);
            return pw;
        }
        public static PartyPacket Load(Party pt, PartyMember update, int disconnecting = -1)
        {
            var pw = new PartyPacket(PartyFunction.LOAD_DONE, pt.partyId);
            EncodePartyData(pw, update, pt, null, disconnecting);
            return pw;
        }

        public static Packet MemberLeft(PartyMember sendTo, PartyMember leaving, Party pt, bool disband, bool expel)
        {
            var pw = new PartyPacket(PartyFunction.WITHDRAW_DONE, pt.partyId);
            pw.WriteInt(leaving.id);
            pw.WriteBool(!disband);
            if (!disband)
            {
                pw.WriteBool(expel);
                pw.WriteString(leaving.name);
                EncodePartyData(pw, sendTo, pt, leaving);
            }
            return pw;
        }
        private static void EncodePartyData(Packet packet, PartyMember member, Party pt, PartyMember remove = null, int disconnect = -1)
        {
            //294
            var ids = pt.members.Select(e => e == null ? 0 : e.id);
            var names = pt.members.Select(e => e == null ? "" : e.name);
            var jobs = pt.members.Select(e => e == null ? 0 : e.job);
            var levels = pt.members.Select(e => e == null ? 0 : e.level);
            var channels = pt.members.Select(e => e == null ? 0 : e.GetChannel());
            var maps = pt.members.Select(e => (e == null || e.id == disconnect || e.GetChannel() != member.GetChannel()) ? -1 : e.GetMap());
            var doorTowns = pt.members.Select(e => e == null ? Constants.InvalidMap : e.door.DstMap);

            ids.ForEach(packet.WriteInt);
            names.ForEach(packet.WriteString13);
            jobs.ForEach(packet.WriteInt);
            levels.ForEach(packet.WriteInt);
            channels.ForEach(packet.WriteInt);
            maps.ForEach(packet.WriteInt);
            packet.WriteInt(pt.leader.id);
            foreach (var m in pt.members)
            {
                if (remove == null && m != null && m.door != null && m.GetChannel() == member.GetChannel())
                {
                    var door = m.door;
                    Trace.WriteLine($"PartyData dest: {door.DstMap} src: {door.SrcMap} x:{door.X} y: {door.Y}");
                    packet.WriteInt(door.DstMap);
                    packet.WriteInt(door.SrcMap);
                    packet.WriteInt(door.X);
                    packet.WriteInt(door.Y);
                }
                else
                {
                    packet.WriteInt(Constants.InvalidMap);
                    packet.WriteInt(Constants.InvalidMap);
                    packet.WriteInt(0);
                    packet.WriteInt(0);
                }
            }
        }

        // Actual hack; this should be client-sided
        public static Packet NoneOnline() 
        {
            // Red text packet
            Packet pw = new Packet(ServerMessages.BROADCAST_MSG);
            pw.WriteByte(0x05);
            pw.WriteString("Either the party doeesn't exist or no member of your party is logged on.");
            return pw;
        }

        public static Packet PartyChat(string fromName, string text, byte group)
        {
            Packet pw = new Packet(ServerMessages.GROUP_MESSAGE);
            pw.WriteByte(group);
            pw.WriteString(fromName);
            pw.WriteString(text);
            return pw;
        }

        public static Packet RequestHpUpdate(int id)
        {
            Packet pw = new Packet(ISServerMessages.UpdateHpParty);
            pw.WriteInt(id);
            return pw;
        }
    }
}
