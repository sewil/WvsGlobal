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
                if (world.Tespia && !session.Player.IsTester) continue;

                Packet worldInfo = new Packet(ServerMessages.WORLD_INFORMATION);
                worldInfo.WriteByte(world.ID);
                worldInfo.WriteString(world.Name);
                worldInfo.WriteByte((byte)world.Ribbon);
                worldInfo.WriteString(world.Message); // World message
                worldInfo.WriteBool(world.BlockCharacterCreation); // block char creation
                worldInfo.WriteByte(world.Channels); // last channel

                for (byte channelID = 0; channelID < world.Channels; channelID++)
                {
                    worldInfo.WriteString(world.Name + "-" + (channelID + 1));
                    if (world.UserLimit == 0)
                    {
                        worldInfo.WriteInt(0);
                    }
                    else
                    {
                        int userNo = (int)(1200 * (world.UserNo[channelID] / (float)world.UserLimit));
                        worldInfo.WriteInt(userNo);
                    }
                    worldInfo.WriteByte(world.ID);
                    worldInfo.WriteByte(channelID);
                    worldInfo.WriteByte(0); // ? not block char creation anymore
                }

                session.SendPacket(worldInfo);
            }

            Packet endWorldInfo = new Packet(ServerMessages.WORLD_INFORMATION);
            endWorldInfo.WriteSByte(-1);

            session.SendPacket(endWorldInfo);
        }
    }
}
