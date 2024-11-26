using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WvsBeta.Center.CharacterPackets;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Center
{
    public enum MessengerFunction : byte
    {
        SelfEnterResult = 0x00,
        Enter = 0x01,
        Leave = 0x02,
        Invite = 0x03,
        InviteResult = 0x04,
        Blocked = 0x05,
        Chat = 0x06,
        Avatar = 0x07,
        Migrated = 0x08,
    }

    public class MessengerMember
    {
        public CenterCharacter Character { get; }
        public AvatarLook Avatar { get; }
        public MessengerMember(CenterCharacter character, AvatarLook avatar)
        {
            Character = character;
            Avatar = avatar;
        }
        public void Encode(Packet pw)
        {
            pw.WriteInt(Character.ID);
            Avatar.Encode(pw);
        }
        public static MessengerMember Decode(Packet pr)
        {
            int cid = pr.ReadInt();
            var avatar = new AvatarLook(pr);
            var character = CenterServer.Instance.FindCharacter(cid);
            return new MessengerMember(character, avatar);
        }
    }
    public class Messenger
    {
        public static List<Messenger> Messengers = new List<Messenger>();

        public int ID { get; set; }
        public MessengerMember Owner { get; set; }
        public MessengerMember[] Members { get; } = new MessengerMember[MAX_MEMBERS];
        private int MemberCount => Members.Where(i => i != null).Count();
        public const int MAX_MEMBERS = 3;

        public Messenger(MessengerMember pOwner)
        {
            Owner = pOwner;
            ID = pOwner.Character.ID;
            AddMember(pOwner);
            Messengers.Add(this);
        }

        public static void EncodeForMigration(Packet pw)
        {
            pw.WriteInt(Messengers.Count);
            foreach (var messenger in Messengers)
            {
                messenger.Owner.Encode(pw);
                for (byte slot = 0; slot < MAX_MEMBERS; slot++)
                {
                    var member = messenger.Members[slot];
                    pw.WriteBool(member == null);
                    if (member == null) continue;
                    member.Encode(pw);
                }
            }
        }

        public static void DecodeForMigration(Packet pr)
        {
            var amount = pr.ReadInt();

            for (var i = 0; i < amount; i++)
            {
                var owner = MessengerMember.Decode(pr);
                if (owner.Character == null) continue;
                var messenger = new Messenger(owner);

                for (byte slot = 0; slot < MAX_MEMBERS; slot++)
                {
                    var isNull = pr.ReadBool();
                    if (isNull) continue;
                    var member = MessengerMember.Decode(pr);
                    if (member.Character == null) continue;
                    messenger.AddMember(member, slot);
                }
            }
        }

        public static void JoinMessenger(Packet packet)
        {
            int messengerID = packet.ReadInt();
            var member = DecodeMember(packet);

            if (messengerID > 0 && Messengers.Exists(m => m.ID == messengerID))
            {
                JoinExistingMessenger(messengerID, member);
            }
            else
            {
                CreateMessenger(member);
            }
        }

        private static void CreateMessenger(MessengerMember pOwner)
        {
            new Messenger(pOwner);
            pOwner.Character.SendPacket(MessengerPacket.SelfEnter(pOwner.Character.MessengerSlot));
        }

        private static void JoinExistingMessenger(int messengerID, MessengerMember joining)
        {
            Messenger messenger = Messengers.First(m => m.ID == messengerID);
            if (messenger == null) // This should already be confirmed when joining, but just to make sure.
            {
                return;
            }
            if (messenger.AddMember(joining)) // No action if messenger is full afaik.
            {
                foreach (var mMember in messenger.Members)
                {
                    if (mMember == null) continue;

                    if (mMember.Character.ID == joining.Character.ID)
                    {
                        joining.Character.SendPacket(MessengerPacket.SelfEnter(joining.Character.MessengerSlot));
                    }
                    else
                    {
                        joining.Character.SendPacket(MessengerPacket.Enter(mMember));
                        mMember.Character.SendPacket(MessengerPacket.Enter(joining));
                    }
                }
            }
        }

        public static void LeaveMessenger(int cid)
        {
            CenterCharacter chr = CenterServer.Instance.FindCharacter(cid);
            Messenger messenger = chr.Messenger;

            if (messenger == null) return;

            byte slot = chr.MessengerSlot;
            bool empty = true;

            foreach (var member in messenger.Members)
            {
                if (member == null) continue;
                if (member.Character.ID != chr.ID) empty = false;
                member.Character.SendPacket(MessengerPacket.Leave(slot));
            }

            messenger.Members[slot] = null;
            chr.Messenger = null;
            chr.MessengerSlot = 0;

            if (empty)
            {
                Messengers.Remove(messenger);
            }
        }

        public static void SendInvite(int senderID, string recipientName)
        {
            CenterCharacter recipient = CenterServer.Instance.FindCharacter(recipientName);
            CenterCharacter sender = CenterServer.Instance.FindCharacter(senderID);
            Messenger messenger = sender.Messenger;

            if (sender == null || messenger == null || messenger.MemberCount >= MAX_MEMBERS)
            {
                return;
            }
            else if (recipient != null)
            {
                recipient.SendPacket(MessengerPacket.Invite(sender.Name, messenger.ID));
            }

            sender.SendPacket(MessengerPacket.InviteResult(recipientName, recipient != null));
        }

        public static void Chat(int cid, string message)
        {
            CenterCharacter chr = CenterServer.Instance.FindCharacter(cid);

            if (chr.Messenger == null)
            {
                return;
            }

            foreach (var member in chr.Messenger.Members.Where(i => i != null && i.Character.ID != cid))
            {
                member.Character.SendPacket(MessengerPacket.Chat(message));
            }
        }

        private static MessengerMember DecodeMember(Packet packet)
        {
            CenterCharacter pCharacter = CenterServer.Instance.FindCharacter(packet.ReadInt());

            pCharacter.Name = packet.ReadString();
            var avatar = new AvatarLook(packet);
            pCharacter.SetFromAvatarLook(avatar);
            var member = new MessengerMember(pCharacter, avatar);
            return member;
        }

        public bool AddMember(MessengerMember member, byte slot)
        {
            if (Members[slot] != null) return false;
            Members[slot] = member;
            member.Character.Messenger = this;
            member.Character.MessengerSlot = slot;
            return true;
        }
        public bool AddMember(MessengerMember member)
        {
            for (byte slot = 0; slot < MAX_MEMBERS; slot++)
            {
                if (AddMember(member, slot))
                    return true;
            }
            return false;
        }

        public static void Block(Packet packet)
        {
            //TODO
        }

        public static void OnAvatar(Packet packet)
        {
            var member = DecodeMember(packet); //, chr, false);
            Messenger messenger = member?.Character?.Messenger;
            if (messenger == null) return;

            foreach (var c in messenger.Members.Where(u => u != null && u != member))
            {
                c.Character.SendPacket(MessengerPacket.Avatar(member));
            }
        }
    }
}
