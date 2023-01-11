using System.Net;
using WvsBeta.Common.Objects;

namespace WvsBeta.Login
{
    public class Center
    {
        public ushort Port { get; set; }
        public IPAddress IP { get; set; }
        public byte ID { get; set; }
        public byte Channels { get; set; }
        public byte State { get; set; }
        public bool BlockCharacterCreation { get; set; }
        public bool AdultWorld { get; set; }
        public string Name { get; set; }
        public World.Ribbon Ribbon { get; set; }
        public string Message { get; set; }

        public int[] UserNo { get; set; }

        public CenterSession Connection { get; private set; }

        public bool IsConnected => Connection != null && !Connection.Disconnected;

        public void Connect()
        {
            Connection = new CenterSession(IP.ToString(), Port, this);
        }
    }

}
