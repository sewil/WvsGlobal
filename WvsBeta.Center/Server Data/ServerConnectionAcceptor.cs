using System.Net.Sockets;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Center
{
    public class ServerConnectionAcceptor : Acceptor
    {
        public ServerConnectionAcceptor() : base(CenterServer.Instance.Port) {}

        public override void OnAccept(Socket pSocket)
        {
            LocalSession connection = new LocalSession(pSocket);
            connection.Init();
        }
    }
}
