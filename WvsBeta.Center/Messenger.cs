using System;
using System.Collections.Generic;
using System.Linq;
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

    public class Messenger
    {
        public static List<Messenger> Messengers = new List<Messenger>();

        public int ID { get; set; }
        public CenterCharacter Owner { get; set; }
        public CenterCharacter[] Members { get; set; }
        public const int MAX_MEMBERS = 3;

        public Messenger(CenterCharacter pOwner)
        {
            Members = new CenterCharacter[MAX_MEMBERS];
            Owner = pOwner;
            ID = pOwner.ID;
            AddMember(pOwner);
            Messengers.Add(this);
            pOwner.Messenger = this;
        }

        public static void EncodeForMigration(Packet pw)
        {
            pw.WriteInt(Messengers.Count);
            foreach (var messenger in Messengers)
            {
                pw.WriteInt(messenger.ID);
                for (var i = 0; i < MAX_MEMBERS; i++)
                {
                    pw.WriteInt(messenger.Members[i]?.ID ?? -1);
                }
            }
        }

        public static void DecodeForMigration(Packet pr)
        {
            var amount = pr.ReadInt();

            var charids = new int[MAX_MEMBERS];
            for (var i = 0; i < amount; i++)
            {
                var ownerId = pr.ReadInt();
                for (var j = 0; j < MAX_MEMBERS; j++)
                {
                    charids[j] = pr.ReadInt();
                }

                var owner = CenterServer.Instance.FindCharacter(ownerId);
                if (owner != null)
                {
                    var messenger = new Messenger(owner);
                    for (byte j = 0; j < MAX_MEMBERS; j++)
                    {
                        var character = messenger.Members[j] = CenterServer.Instance.FindCharacter(charids[j]);
                        // Re-assign user
                        if (character != null)
                        {
                            character.Messenger = messenger;
                            character.MessengerSlot = j;
                        }
                    }
                }
            }
        }

        public static void JoinMessenger(Packet packet)
        {
            int messengerID = packet.ReadInt();
            CenterCharacter chr = ParseMessengerCharacter(packet);

            if (messengerID > 0 && Messengers.Exists(m => m.ID == messengerID))
            {
                JoinExistingMessenger(messengerID, chr);
            }
            else
            {
                CreateMessenger(chr);
            }
        }


        private static void CreateMessenger(CenterCharacter pOwner)
        {
            new Messenger(pOwner);
            pOwner.SendPacket(MessengerPacket.SelfEnter(pOwner.MessengerSlot));
        }

        private static void JoinExistingMessenger(int messengerID, CenterCharacter joining)
        {
            Messenger messenger = Messengers.First(m => m.ID == messengerID);
            if (messenger == null) // This should already be confirmed when joining, but just to make sure.
            {
                return;
            }
            if (messenger.AddMember(joining)) // No action if messenger is full afaik.
            {
                joining.Messenger = messenger;
                foreach (CenterCharacter mMember in messenger.Members)
                {

                    if (mMember == null) continue;

                    if (mMember.ID == joining.ID)
                    {
                        joining.SendPacket(MessengerPacket.SelfEnter(joining.MessengerSlot));
                    }
                    else
                    {
                        joining.SendPacket(MessengerPacket.Enter(mMember));
                        mMember.SendPacket(MessengerPacket.Enter(joining));
                    }
                }
            }
        }

        public static void LeaveMessenger(int cid)
        {
            CenterCharacter chr = CenterServer.Instance.FindCharacter(cid);
            Messenger messenger = chr.Messenger;

            if (messenger == null)
            {
                return;
            }

            byte slot = chr.MessengerSlot;
            bool empty = true;

            foreach (CenterCharacter mChr in messenger.Members)
            {
                if (mChr != null)
                {
                    if (mChr.ID != chr.ID)
                    {
                        empty = false;
                    }
                    mChr.SendPacket(MessengerPacket.Leave(slot));
                }
            }

            messenger.Members[slot] = null;
            chr.Messenger = null;
            chr.MessengerSlot = 0;

            if (empty)
            {
                Messengers.Remove(messenger);
            }
        }

        private int usersInChat()
        {
            return Members.Count(e => e != null); // Max was here
        }

        public static void SendInvite(int senderID, String recipientName)
        {
            CenterCharacter recipient = CenterServer.Instance.FindCharacter(recipientName);
            CenterCharacter sender = CenterServer.Instance.FindCharacter(senderID);
            Messenger messenger = sender.Messenger;

            if (sender == null || messenger == null || messenger.usersInChat() >= MAX_MEMBERS)
            {
                return;
            }
            else if (recipient != null)
            {
                recipient.SendPacket(MessengerPacket.Invite(sender.Name, messenger.ID));
            }

            sender.SendPacket(MessengerPacket.InviteResult(recipientName, recipient != null));
        }

        public static void Chat(int cid, String message)
        {
            CenterCharacter chr = CenterServer.Instance.FindCharacter(cid);

            if (chr.Messenger == null)
            {
                return;
            }

            foreach (CenterCharacter mChr in chr.Messenger.Members)
            {
                if (mChr != null && mChr.ID != cid)
                {
                    mChr.SendPacket(MessengerPacket.Chat(message));
                }
            }
        }

        private static CenterCharacter ParseMessengerCharacter(Packet packet)
        {
            CenterCharacter pCharacter = CenterServer.Instance.FindCharacter(packet.ReadInt());

            pCharacter.Name = packet.ReadString();
            pCharacter.SetFromAvatarLook(new AvatarLook(packet));
            return pCharacter;
        }

        public bool AddMember(CenterCharacter pCharacter)
        {
            byte slot = (byte)Array.IndexOf(Members, null);
            if (slot < MAX_MEMBERS)
            {
                pCharacter.MessengerSlot = slot;
                Members[slot] = pCharacter;
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
            CenterCharacter chr = ParseMessengerCharacter(packet); //, chr, false);
            if (chr == null || chr.Messenger == null)
            {
                return;
            }
            Messenger messenger = chr.Messenger;

            foreach (var c in messenger.Members.Where(u => u != null && u != chr))
            {
                c?.SendPacket(MessengerPacket.Avatar(chr));
            }
        }
    }
}
