using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using log4net;

namespace WvsBeta.Common.Sessions
{
    public abstract class ConnectionSession : Session
    {
        private static ILog log = LogManager.GetLogger("AbstractConnection");

        public bool gotPong = false;
        public bool sentPing = false;
        public bool sentSecondPing = false;

        public long pingSentDateTime { get; private set; }
        public int PingMS { get; private set; }

        public bool UseMemoryCRC { get; protected set; }
        private bool _ignoredWrongCRC = false;

        private readonly bool isConnectedAsClient;

        public int pings { get; set; }

        protected ConnectionSession(System.Net.Sockets.Socket pSocket, bool isConnectedAsClient, string tn)
            : base(pSocket, tn)
        {
            pings = 0;
            this.isConnectedAsClient = isConnectedAsClient;
        }

        protected ConnectionSession(string pIP, ushort pPort, bool isConnectedAsClient, string tn)
            : base(pIP, pPort, tn)
        {
            pings = 0;
            this.isConnectedAsClient = isConnectedAsClient;
        }

        public virtual void StartLogging()
        {
            log4net.ThreadContext.Properties["RemoteEndpoint"] = IP + ":" + Port;
        }

        public virtual void EndLogging()
        {
            log4net.ThreadContext.Properties.Remove("RemoteEndpoint");
        }

        private int _memoryOffset = 0;

        public static ConcurrentDictionary<Tuple<uint, uint, uint>, uint> IvCrcMapping = new ConcurrentDictionary<Tuple<uint, uint, uint>, uint>();

        private bool ValidateCRC(byte[] iv, uint clientCRC, out uint expectedCRC)
        {
            expectedCRC = CRC32.CalcCrc32(iv, 2);

            if (UseMemoryCRC)
            {
                foreach (var region in MemoryRegions.Instance.Regions)
                {
                    expectedCRC = CRC32.CalcCrc32(
                        region.Data,
                        region.Length,
                        expectedCRC ^ (region.Address + (uint)_memoryOffset),
                        _memoryOffset
                    );
                }
            }

            var isValid = expectedCRC == clientCRC;

            if (!isValid)
            {
                if (_ignoredWrongCRC)
                {
                    log.Error($"Disconnecting client because CRC didnt match _again_");
                }
                else
                {
                    log.Warn($"Accepting wrong CRC for _once_...");
                    _ignoredWrongCRC = true;
                    isValid = true;
                }
            }

            return isValid;
        }

        private static Random rnd = new Random();
        protected void SendMemoryRegions()
        {
            if (isConnectedAsClient || !Config.Instance.MemoryCRCEnabled) return;

            var packet = new Packet(ServerMessages.SEND_MEMORY_REGIONS);
            packet.WriteByte(0);
            var regions = MemoryRegions.Instance.Regions;

            _memoryOffset = rnd.Next(0, MemoryRegions.Instance.MaxRandomMemoryOffset);

            packet.WriteShort((short)regions.Count);
            foreach (var region in regions)
            {
                packet.WriteUInt(region.Address + (uint)_memoryOffset);
                packet.WriteInt(region.Length - _memoryOffset);
            }

            SendPacket(packet);


            UseMemoryCRC = true;
        }


        public RedisBackend.HackKind? HackDetected { get; protected set; } = null;

        public virtual void OnHackDetected() { }

        protected void TryRegisterHackDetection(int userId)
        {
            if (!HackDetected.HasValue) return;

            // Okay, register.
            RedisBackend.Instance.RegisterNonGameHackDetection(userId, HackDetected.Value);
            log.Warn($"Registered hack by userid {userId}: {HackDetected}. Waiting for him to go to a channel.");
        }


        private bool dcEnqueued = false;
        public void ScheduleDisconnect()
        {
            if (dcEnqueued) return;

            dcEnqueued = true;
            MasterThread.RepeatingAction.Start("DC " + IP + ":" + Port, Disconnect, 5000, 0);
        }

        public override void OnPacketInbound(Packet pPacket)
        {
            if (pPacket.Length == 0)
                return;
            StartLogging();
            try
            {
                byte header = pPacket.ReadByte();

                if (isConnectedAsClient)
                {
                    if (header == (byte)ServerMessages.PING)
                    {
                        SendPong();
                    }
                }
                else
                {
                    if (header == (byte)ClientMessages.PONG)
                    {
                        gotPong = true;
                        PingMS = (int)(MasterThread.CurrentTime - pingSentDateTime);
                    }
                    else if (header == (byte)ClientMessages.__CUSTOM_DC_ME__)
                    {
                        ScheduleDisconnect();
                        return;
                    }
                    else if (!isConnectedAsClient && Config.Instance.MemoryCRCEnabled && header == (byte)ClientMessages.CLIENT_HASH && (BitConverter.ToUInt16(previousDecryptIV, 0) % 31) == 0)
                    {
                        // Check for expected CRC packet
                        bool disconnect = true;
                        byte mode = pPacket.ReadByte();
                        if (mode == 1)
                        {
                            uint clientCRC = pPacket.ReadUInt();
                            bool validCRC = ValidateCRC(previousDecryptIV, clientCRC, out uint expectedCRC);
                            if (validCRC)
                            {
                                disconnect = false;
                            }
                        }

                        if (disconnect)
                        {
                            Console.WriteLine("DC");
                            Disconnect();
                            return;
                        }
                    }
                }

                pPacket.Reset(0);

                AC_OnPacketInbound(pPacket);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EndLogging();
            }
        }

        public abstract void AC_OnPacketInbound(Packet pPacket);

        private static Packet _pingPacket = new Packet(ServerMessages.PING);
        private static Packet _pongPacket = new Packet((byte)ClientMessages.PONG);

        public void SendPing()
        {
            pingSentDateTime = MasterThread.CurrentTime;
            SendPacket(_pingPacket);
        }

        public void SendPong()
        {
            SendPacket(_pongPacket);
        }
    }
}