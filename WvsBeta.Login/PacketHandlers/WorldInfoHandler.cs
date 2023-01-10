using log4net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login.PacketHandlers
{
    internal class WorldInfoHandler
    {
        public WorldInfoHandler(ClientSession session, ILog log)
        {
            if (log.AssertWarning(session.Player.State != Player.LoginState.WorldSelect,
                        "Tried to get the world information while not in worldselect")) return;

            foreach (var kvp in Server.Instance.Worlds)
            {
                var world = kvp.Value;

                Packet worldInfo = new Packet(ServerMessages.WORLD_INFORMATION);
                worldInfo.WriteByte(world.ID);
                worldInfo.WriteString(world.Name);
                worldInfo.WriteByte(world.Channels); //last channel

                for (byte i = 0; i < world.Channels; i++)
                {
                    worldInfo.WriteString(world.Name + "-" + (i + 1));
                    worldInfo.WriteInt(world.UserNo[i] * 10);
                    worldInfo.WriteByte(world.ID);
                    worldInfo.WriteByte(i);
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
