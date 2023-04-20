using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using WvsBeta.Common.Character;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Center.CharacterPackets
{

    enum MessengerAction : byte
    {
        Enter = 0,
        SelfEnterResult = 1,
        Leave = 2,
        Invite = 3,
        InviteResult = 4,
        Blocked = 5,
        Chat = 6,
        Avatar = 7,
        Migrated = 8,
    }

    public static class MessengerPacket
    {
        //Used to inform the client which slot it's going to enter
        public static Packet SelfEnter(byte slot)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerAction.SelfEnterResult);
            packet.WriteByte(slot);
            return packet;
        }

        // Used for visually displaying Characters in messenger
        public static Packet Enter(Character chr)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerAction.Enter);
            packet.WriteByte(chr.MessengerSlot);
            new AvatarLook(chr, true).Encode(packet);
            packet.WriteString(chr.Name);
            packet.WriteByte(chr.ChannelID);
            packet.WriteBool(true); //Announce in chat
            return packet;
        }

        public static Packet Leave(byte slot)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerAction.Leave);
            packet.WriteByte(slot);
            return packet;
        }

        public static Packet Invite(String sender, int messengerId)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerAction.Invite);
            packet.WriteString(sender);
            packet.WriteByte(0);
            packet.WriteInt(messengerId);
            packet.WriteByte(0);
            return packet;
        }

        public static Packet InviteResult(String recipient, bool success)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerAction.InviteResult);
            packet.WriteString(recipient);
            packet.WriteBool(success); // False : '%' can't be found. True : you have sent invite to '%'.
            return packet;
        }

        public static Packet Blocked(int deliverto, string receiver, byte mode)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerFunction.Blocked);
            packet.WriteString(receiver);
            packet.WriteByte(mode); // 0 : % denied the request. 1 : '%' is currently not accepting chat.
            return packet;
        }

        public static Packet Chat(string message)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerFunction.Chat);
            packet.WriteString(message);
            return packet;
        }

        public static Packet Avatar(Character chr)
        {
            Packet packet = new Packet(ServerMessages.MESSENGER);
            packet.WriteByte((byte)MessengerFunction.Avatar);
            packet.WriteByte(chr.MessengerSlot);
            new AvatarLook(chr, true).Encode(packet);
            return packet;
        }
    }
}
