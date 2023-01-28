using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common;
using WvsBeta.Game.Events.PartyQuests;

namespace WvsBeta.Game
{
    public class MysticDoor
    {
        public readonly int OwnerId;
        public int OwnerPartyId { get; set; }
        public readonly short X;
        public readonly short Y;
        public readonly int FieldId;
        public long EndTime { get; set; }

        public MysticDoor(int ownerId, int ownPtId, short x, short y, int fieldId, long tEnd)
        {
            OwnerId = ownerId;
            OwnerPartyId = ownPtId;
            X = x;
            Y = y;
            FieldId = fieldId;
            EndTime = tEnd;
        }

        public bool CanEnterDoor(GameCharacter chr)
        {
            if (chr.PartyID > 0 && OwnerPartyId == chr.PartyID) return true;
            if (chr.ID == OwnerId) return true;
            return false;
        }
    }
    public enum DoorEnterType
    {
        Drop = 0,
        Fade = 1
    }
    public class DoorManager
    {

        public readonly Dictionary<int, MysticDoor> TownDoors;
        private readonly Dictionary<int, MysticDoor> Doors;
        private readonly Map Field;

        public DoorManager(Map field)
        {
            Doors = new Dictionary<int, MysticDoor>();
            TownDoors = new Dictionary<int, MysticDoor>();
            Field = field;
        }
        public void TryRemoveDoor(int ownerId)
        {
            if (!Doors.ContainsKey(ownerId)) return;
            var door = Doors[ownerId];
            Doors.Remove(door.OwnerId);
            Field.SendPacket(MapPacket.RemoveDoor(ownerId, 0));
            var town = DataProvider.Maps[Field.ReturnMap];
            town.DoorPool.TownDoors.Remove(door.OwnerId);
            var owner = Server.Instance.GetCharacter(door.OwnerId);
            if (owner != null && owner.Field.ID == Field.ReturnMap)
            {
                town.SendPacket(MapPacket.RemovePortal());
                owner.DoorMapId = Constants.InvalidMap;
            }
            if (door.OwnerPartyId != 0)
            {
                Server.Instance.CenterConnection.PartyDoorRemoved(door.OwnerId);
            }
        }

        public void CreateDoor(GameCharacter chr, short x, short y, long endTime)
        {
            var door = new MysticDoor(chr.ID, chr.PartyID, x, y, Field.ID, endTime);
            Doors.Add(chr.ID, door);
            DataProvider.Maps[Field.ReturnMap].DoorPool.TownDoors.Add(chr.ID, door);
            foreach (var c in UpdatableChars(door))
            {
                c.SendPacket(MapPacket.ShowDoor(door, DoorEnterType.Drop));
                c.SendPacket(MapPacket.SpawnPortal(door.FieldId, Field.ReturnMap, door.X, door.Y));
            }

            if (door.OwnerPartyId != 0)
            {
                Server.Instance.CenterConnection.PartyDoorCreated(door);
            }
        }

        public void ShowAllDoorsTo(GameCharacter victim, bool ignoreMine = false)
        {
            foreach (var door in Doors.Values)
            {
                bool mine = door.OwnerId == victim.ID;
                if (ignoreMine && mine) continue;
                if (door.CanEnterDoor(victim))
                {
                    victim.SendPacket(MapPacket.ShowDoor(door, DoorEnterType.Fade));
                }
            }

            foreach (var townDoor in TownDoors.Values)
            {
                bool mine = townDoor.OwnerId == victim.ID;
                if (ignoreMine && mine) continue;
                if (townDoor.CanEnterDoor(victim))
                {
                    victim.SendPacket(MapPacket.SpawnPortal(townDoor.FieldId, Field.ReturnMap, townDoor.X, townDoor.Y));
                }
            }
        }

        public IEnumerable<GameCharacter> UpdatableChars(MysticDoor door)
        {
            return Field.Characters.Where(c => door.CanEnterDoor(c));
        }
        public void HideAllDoorsFrom(GameCharacter c, bool ignoreMine = false)
        {
            foreach (var door in Doors.Values)
            {
                if (ignoreMine && door.OwnerId == c.ID) continue;
                if (!door.CanEnterDoor(c))
                {
                    c.SendPacket(MapPacket.RemoveDoor(door.OwnerId, 0));
                }
            }
            //foreach (var door in TownDoors.Values)
            //{
            //    if (!door.CanEnterDoor(c))
            //    {
            //        c.SendPacket(MapPacket.RemovePortal());
            //    }
            //}
        }
        
        public void Update(long pNow)
        {
            foreach (var door in Doors.Values.ToList())
            {
                if (door.EndTime < pNow)
                    TryRemoveDoor(door.OwnerId);
            }
        }

        public bool TryGetDoor(int charId, out MysticDoor door)
        {
            return Doors.TryGetValue(charId, out door);
        }
        
    }
}
