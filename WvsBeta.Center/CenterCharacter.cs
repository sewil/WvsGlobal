using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Characters;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;
using static WvsBeta.Common.Constants.EquipSlots;

namespace WvsBeta.Center
{
    public class CenterCharacter : Character
    {
        public byte ChannelID { get; set; }
        public bool isCCing { get; set; }
        public bool isConnectingFromLogin { get; set; }
        public bool InCashShop { get; set; }
        public byte LastChannel { get; set; }
        public BuddyList FriendsList { get; set; }

        public Messenger Messenger { get; set; }
        public byte MessengerSlot { get; set; }

        public CharacterMemos Memos;

        private int _PartyID;
        public override int PartyID
        {
            get
            {
                return _PartyID;
            }
            set
            {
                _PartyID = value;
                if (IsOnline)
                {
                    Packet packet = new Packet(ISServerMessages.ChangeParty);
                    packet.WriteInt(ID);
                    packet.WriteInt(_PartyID);
                    CenterServer.Instance.SendPacketToServer(packet, ChannelID);
                }
            }
        }

        public CenterCharacter(string name, int id, bool isCCing, byte gmLevel, bool isOnline)
        {
            Name = name;
            ID = id;
            this.isCCing = isCCing;
            GMLevel = gmLevel;
            IsOnline = isOnline;
            base.Inventory = new Common.Characters.Inventory(0, id);
            CharacterMemos.Load(this);
        }

        public CenterCharacter(Packet pr)
        {
            ChannelID = pr.ReadByte();
            LastChannel = pr.ReadByte();
            FriendsList = new BuddyList(pr);
            base.DecodeForTransfer(pr);
            base.Inventory = new Common.Characters.Inventory(UserID, ID);
        }

        public void SetFromAvatarLook(AvatarLook avatar)
        {
            Gender = avatar.Gender;
            Skin = avatar.Skin;
            Face = avatar.Face;
            Hair = avatar.Hair;
        }

        public new void EncodeForTransfer(Packet pw)
        {
            pw.WriteByte(ChannelID);
            pw.WriteByte(LastChannel);
            FriendsList.EncodeForTransfer(pw);

            base.EncodeForTransfer(pw);
        }

        public void SendPacket(Packet pPacket)
        {
            Packet toserver = new Packet(ISServerMessages.PlayerSendPacket);
            toserver.WriteInt(base.ID);
            toserver.WriteBytes(pPacket.ToArray());
            CenterServer.Instance.SendPacketToServer(toserver, ChannelID);
        }
    }
}
