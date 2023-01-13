using log4net;
using WvsBeta.Common.Enums;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class WorldInfoHandler
    {
        public WorldInfoHandler(ClientSession session, ILog log)
        {
            if (log.AssertWarning(session.Player.State != GameState.WorldSelect,
                        "Tried to get the world information while not in worldselect")) return;

            foreach (var kvp in Server.Instance.Worlds)
            {
                var world = kvp.Value;

                Packet worldInfo = new Packet(ServerMessages.WORLD_INFORMATION);
                worldInfo.WriteByte(world.ID);
                worldInfo.WriteString(world.Name);
                worldInfo.WriteByte((byte)world.Ribbon);
                worldInfo.WriteString(world.Message); // World message
                worldInfo.WriteByte(1);          // ? exp event notification?
                worldInfo.WriteByte(world.Channels); // last channel

                for (byte channelID = 0; channelID < world.Channels; channelID++)
                {
                    worldInfo.WriteString(world.Name + "-" + (channelID + 1));
                    worldInfo.WriteInt(world.UserNo[channelID] * 1200);
                    worldInfo.WriteByte(world.ID);
                    worldInfo.WriteByte(channelID);
                    worldInfo.WriteBool(world.BlockCharacterCreation);
                }

                session.SendPacket(worldInfo);
            }

            Packet endWorldInfo = new Packet(ServerMessages.WORLD_INFORMATION);
            endWorldInfo.WriteSByte(-1);

            session.SendPacket(endWorldInfo);
        }
    }
}
