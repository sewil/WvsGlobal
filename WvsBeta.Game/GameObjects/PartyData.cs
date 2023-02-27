using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Game.GameObjects
{
    public class PartyData
    {
        public readonly int PartyID;
        public readonly int Leader;
        public int MemberCount => Members.Where(i => i > 0).Count();
        private int[] _members;
        public int[] Members
        {
            get => _members;
            set
            {
                _members = value;
                Characters = _members.Select(m => Server.Instance.GetCharacter(m)).Where(c => c != null);
            }
        }
        public IEnumerable<GameCharacter> Characters { get; private set; }

        public PartyData(int ldr, int[] pt, int id)
        {
            Leader = ldr;
            Members = pt;
            PartyID = id;
        }

        public static void EncodeForTransfer(Packet packet)
        {
            packet.WriteInt(Parties.Count);
            foreach (var kvp in Parties)
            {
                var party = kvp.Value;
                packet.WriteInt(party.PartyID);
                packet.WriteInt(party.Leader);

                for (var i = 0; i < Constants.MaxPartyMembers; i++)
                    packet.WriteInt(party.Members[i]);
            }
        }

        public static void DecodeForTransfer(Packet packet)
        {
            var amount = packet.ReadInt();
            Parties = new Dictionary<int, PartyData>(amount);
            for (int i = 0; i < amount; i++)
            {
                var id = packet.ReadInt();
                var leader = packet.ReadInt();
                var memberList = new int[Constants.MaxPartyMembers];
                for (int j = 0; j < memberList.Length; j++)
                {
                    memberList[i] = packet.ReadInt();
                }

                Parties[id] = new PartyData(leader, memberList, id);
            }
        }

        /*****************************************************************/
        public static Dictionary<int, PartyData> Parties { get; private set; } = new Dictionary<int, PartyData>();

        public static byte? GetMemberIdx(int charid)
        {
            foreach (var keyValuePair in Parties)
            {
                var members = keyValuePair.Value.Members;
                for (var i = 0; i < Constants.MaxPartyMembers; i++)
                {
                    if (members[i] == charid) return (byte)i;
                }
            }
            return null;
        }
    }
}
