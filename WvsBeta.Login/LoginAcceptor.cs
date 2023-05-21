using System.Net.Sockets;
using System.Net;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Login
{
    class LoginAcceptor : Acceptor
    {
        public LoginAcceptor() : base(Server.Instance.Port)
        {

        }

        public override void OnAccept(Socket pSocket)
        {
            if (Server.Instance.DisableMultiIP)
            {
                var ip = (pSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                if (Server.Instance.HasPlayerWithIP(ip)) return;
            }
            new ClientSession(pSocket);
        }
    }
}