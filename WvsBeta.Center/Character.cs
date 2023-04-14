using System.Linq;
using WvsBeta.Common.Character;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Objects;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Center
{
    public class Character : Common.CharacterBase
    {
        public byte ChannelID { get; set; }
        public bool isCCing { get; set; }
        public bool isConnectingFromLogin { get; set; }
        public bool InCashShop { get; set; }
        public byte LastChannel { get; set; }
        public BuddyList FriendsList { get; set; }

        public Messenger Messenger { get; set; }
        public byte MessengerSlot { get; set; }
        public new CharacterInventory Inventory => (CharacterInventory)base.Inventory;

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

        public Character(string name, int id, bool isCCing, byte gmLevel, bool isOnline)
        {
            Name = name;
            ID = id;
            this.isCCing = isCCing;
            GMLevel = gmLevel;
            IsOnline = isOnline;
            base.Inventory = new CharacterInventory(0, id);
            CharacterMemos.Load(this);
        }

        public Character(Packet pr)
        {
            ChannelID = pr.ReadByte();
            LastChannel = pr.ReadByte();
            FriendsList = new BuddyList(pr);
            base.DecodeForTransfer(pr);
            base.Inventory = new CharacterInventory(UserID, ID);
        }

        public void SetFromAvatarLook(AvatarLook avatar)
        {
            Inventory.Equipped[EquippedType.Cash] = avatar.CashEquips.Select(i => new EquipItem(i)).ToArray();
            Inventory.Equipped[EquippedType.Normal] = avatar.NormalEquips.Select(i => new EquipItem(i)).ToArray();

            Gender = avatar.Gender;
            Skin = avatar.Skin;
            Face = avatar.Face;
            Hair = avatar.CashEquips[0];
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
