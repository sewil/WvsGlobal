using System.Net.Sockets;
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
            new ClientSession(pSocket);
        }
    }
}