using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using WvsBeta.Common.Crypto;

namespace WvsBeta.Common.Sessions
{
    public class Session
    {
        private void DoAction(Action<long> pAction, string name)
        {
            MasterThread.Instance.AddCallback(pAction, "IP: " + IP + ":" + Port + " " + name);
        }


        /// <summary>
        /// Socket we use
        /// </summary>
        private Socket _socket;

        #region Data and encryption

        /// <summary>
        /// IV used for header generation and AES decryption
        /// </summary>
        protected byte[] _decryptIV;

        /// <summary>
        /// IV used for header generation and AES encryption
        /// </summary>
        protected byte[] _encryptIV;


        /// <summary>
        /// Buffer used for receiving packets.
        /// </summary>
        private byte[] _buffer = new byte[64];

        /// <summary>
        /// Position for receiving data.
        /// </summary>
        private int _bufferpos;

        /// <summary>
        /// Lenght of packet to receive.
        /// </summary>
        private int _bufferlen;

        private bool _header;
        private bool _encryption = false;
        private readonly bool _receivingFromServer;

        private ushort _mapleVersion;
        private string _maplePatchLocation;
        private byte _mapleLocale;

        public ushort MapleVersion => _mapleVersion;
        public string MaplePatchLocation => _maplePatchLocation;
        public byte MapleLocale => _mapleLocale;

        public bool Disconnected { get; private set; }
        public bool PreventConnectFromSucceeding { get; set; } = false;

        public string TypeName { get; private set; }

        public string IP { get; private set; }
        public ushort Port { get; private set; }
        
        protected byte[] previousDecryptIV = new byte[4];

        #endregion

        /// <summary>
        /// Creates a new instance of Session.
        /// </summary>
        /// <param name="pSocket">The socket we use</param>
        public Session(Socket pSocket, string tn)
        {
            TypeName = tn;
            Disconnected = false;
            _socket = pSocket;
            _receivingFromServer = false;

            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 5000);

            IPEndPoint remoteIpEndPoint = _socket.RemoteEndPoint as IPEndPoint;
            IP = remoteIpEndPoint.Address.ToString();
            Port = (ushort)remoteIpEndPoint.Port;

            StartReading(4, true);
        }

        /// <summary>
        /// Connects to the server with the given IP and Port
        /// </summary>
        /// <param name="pIP">IP address to connect to.</param>
        /// <param name="pPort">Port to connect to.</param>
        public Session(string pIP, ushort pPort, string tn)
        {
            TypeName = tn;
            IP = pIP;
            Port = pPort;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Disconnected = true;
            _receivingFromServer = true;
            _mapleVersion = 0;
            _socket.BeginConnect(pIP, pPort, EndConnect, null);
        }

        void EndConnect(IAsyncResult pIAR)
        {
            try
            {
                _socket.EndConnect(pIAR);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(TypeName + " [ERROR] Could not connect to server: {0}", ex.Message);
                return;
            }
            if (PreventConnectFromSucceeding)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                try
                {
                    _socket.Close();
                }
                catch
                {
                }
                return;
            }
            Console.WriteLine(TypeName + " Connected with server!");
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 5000);
            Disconnected = false;
            StartReading(2, true);
        }

        public bool Disconnect()
        {
            if (Disconnected) return false;
            try
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
            }
            try
            {
                _socket.Close();
            }
            catch
            {
            }
            Trace.WriteLine(TypeName + " Manual disconnection!");
            OnDisconnectINTERNAL("Forced disconnect");
            return true;
        }

        /// <summary>
        /// Starts the reading mechanism.
        /// </summary>
        /// <param name="pLength">Amount of bytes to receive</param>
        /// <param name="pHeader">Do we receive a header?</param>
        private void StartReading(int pLength, bool pHeader = false)
        {
            if (Disconnected) return;
            _header = pHeader;

            if (_buffer.Length < pLength)
            {
                const int PageSize = 128;

                int newSize = pLength;
                if ((pLength % PageSize) != 0) newSize += (PageSize - (pLength % PageSize));
                Array.Resize(ref _buffer, newSize);
            }
            _bufferlen = pLength;
            _bufferpos = 0;
            ContinueReading();
        }

        /// <summary>
        /// Calls Socket.BeginReceive to receive more data.
        /// </summary>
        private void ContinueReading()
        {
            if (Disconnected) return;
            try
            {
                _socket.BeginReceive(_buffer, _bufferpos, _bufferlen - _bufferpos, SocketFlags.None, EndReading, null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(TypeName + " [ERROR] ContinueReading(): {0}", ex.ToString());
                OnDisconnectINTERNAL("BeginReceive exception");
            }
        }

        /// <summary>
        /// Used as IAsyncResult parser for ContinueReading().
        /// </summary>
        /// <param name="pIAR">The result AsyncCallback makes</param>
        private void EndReading(IAsyncResult pIAR)
        {
            int amountReceived = 0;
            try
            {
                amountReceived = _socket.EndReceive(pIAR);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("EXCEPT" + " : " + ex.ToString());
                amountReceived = 0;
            }
            if (amountReceived == 0)
            {
                OnDisconnectINTERNAL("No data received");
                return;
            }

            // Add amount of bytes received to _bufferpos so we know if we got everything.
            _bufferpos += amountReceived;

            try
            {
                // Check if we got all data. There is _no_ way we would have received more bytes than needed. Period.
                if (_bufferpos == _bufferlen)
                {

                    // It seems we have all data we need
                    // Now check if we got a header
                    if (_header)
                    {
                        if (!_encryption && _receivingFromServer)
                        {
                            // Unencrypted packets have a short header with plain length.
                            ushort length = (ushort)(_buffer[0] | _buffer[1] << 8);
                            StartReading(length);
                        }
                        else
                        {
                            int length = GetHeaderLength(_buffer, _bufferlen, _decryptIV, _mapleVersion, _receivingFromServer);
                            if (length == HEADER_ERROR_MORE_DATA)
                            {
                                _bufferlen += 4;
                                ContinueReading();
                            }
                            else
                            {
                                StartReading(length);
                            }
                        }
                    }
                    else
                    {
                        Packet packet;
                        if (_encryption)
                        {
                            // Small scope hack; this will be on the stack until the
                            // callback in DoAction leaves scope
                            byte[] tmpIV = new byte[4];
                            Array.Copy(_decryptIV, 0, tmpIV, 0, 4);

                            // Make a copy of the data because it will be transformed

                            var tempBuff = new byte[_bufferlen];
                            Array.Copy(_buffer, 0, tempBuff, 0, _bufferlen);
                            var data = Decrypt(tempBuff, _bufferlen, _decryptIV);

                            packet = new Packet(data, _bufferlen);

                            byte opcode = data[0];

                            DoAction((date) =>
                            {
                                if (Disconnected) return;
                                try
                                {
                                    Array.Copy(tmpIV, 0, previousDecryptIV, 0, 4);
                                    OnPacketInbound(packet);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("Handling Packet Error: {0}", ex.ToString());
                                }
                            }, "Packet handling opcode: " + opcode);
                        }
                        else
                        {
                            _encryption = true; // First packet received or sent is unencrypted. All others are.
                            packet = new Packet(_buffer, _bufferlen);

                            _mapleVersion = packet.ReadUShort();
                            _maplePatchLocation = packet.ReadString();
                            _encryptIV = packet.ReadBytes(4);
                            _decryptIV = packet.ReadBytes(4);
                            _mapleLocale = packet.ReadByte();

                            StartSendAndEncryptLoop();
                            packet.Reset();

                            DoAction((date) =>
                            {
                                try
                                {
                                    OnHandshakeInbound(packet);
                                }
                                catch (Exception ex)
                                {
                                    Trace.WriteLine($"Handling Packet Error: {ex}");
                                }
                            }, "Handshake handling");
                        }

                        StartReading(4, true);
                    }
                }
                else
                {
                    ContinueReading();
                }

            }
            catch (SocketException socketException)
            {
                Trace.WriteLine(TypeName + " Socket Exception while receiving data: " + socketException);
                OnDisconnectINTERNAL("EndRead socket exception");
            }
            catch (ObjectDisposedException)
            {
                OnDisconnectINTERNAL("EndRead object disposed");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(TypeName + " [ERROR] EndReading(): " + ex);
                OnDisconnectINTERNAL("EndRead exception");
            }
        }

        public virtual void SendPacket(Packet pPacket)
        {
            SendData(pPacket.ToArray());
        }

        /// <summary>
        /// Sends bytes to the other side
        /// </summary>
        /// <param name="pData">Data to encrypt and send</param>
        public void SendData(byte[] pData)
        {
            if (Disconnected) return;

            _dataToEncrypt.Enqueue(pData);
            _dataReadyToSend.Set();
        }

        static Random rnd = new Random();
        public void SendHandshake(ushort pVersion, string pPatchLocation, byte pLocale)
        {
            Trace.WriteLine($"[{this}] Got connection, sending handshake");

            _encryptIV = new byte[4];
            _decryptIV = new byte[4];
            rnd.NextBytes(_encryptIV);
            rnd.NextBytes(_decryptIV);

            _mapleVersion = pVersion;
            _maplePatchLocation = pPatchLocation;
            _mapleLocale = pLocale;

            StartSendAndEncryptLoop();

            Packet packet = new Packet();
            packet.WriteUShort(pVersion);
            packet.WriteString(pPatchLocation);
            packet.WriteBytes(_decryptIV);
            packet.WriteBytes(_encryptIV);
            packet.WriteByte(pLocale);
            SendData(packet.ToArray());
        }

        public virtual void OnPacketInbound(Packet pPacket)
        {
            Console.WriteLine(TypeName + " No Handler for 0x{0:X4}", pPacket.ReadUShort());
        }

        public virtual void OnHandshakeInbound(Packet pPacket)
        {
            Console.WriteLine(TypeName + " No Handshake Handler.");
        }

        private void OnDisconnectINTERNAL(string reason)
        {
            if (Disconnected) return;
            Disconnected = true;
            StopSendAndEncryptLoop();
            Trace.WriteLine($"OnDisconnectINTERNAL: {reason}");

            ////Console.WriteLine(TypeName + " Called by:");
            ////Console.WriteLine(Environment.StackTrace);
            DoAction((date) =>
            {
                OnDisconnect();
            }, "OnDisconnectINTERNAL " + reason);
        }

        public virtual void OnDisconnect()
        {
            if (Disconnected) return;
            Disconnected = true;
            ////Console.WriteLine(TypeName + " Called by:");
            ////Console.WriteLine(Environment.StackTrace);
            ////Console.WriteLine(TypeName + " No Disconnect Handler.");
        }

        #region Send Thread

        private Thread _encryptAndSendThread;
        private readonly ConcurrentQueue<byte[]> _dataToEncrypt = new ConcurrentQueue<byte[]>();
        private readonly AutoResetEvent _dataReadyToSend = new AutoResetEvent(false);


        private void StopSendAndEncryptLoop()
        {
            if (_encryptAndSendThread == null) return;
            _encryptAndSendThread = null;
            // Make the thread stop
            _dataReadyToSend.Set();
        }

        private void StartSendAndEncryptLoop()
        {
            _encryptAndSendThread = new Thread(x =>
            {
                Trace.WriteLine("Starting encryption loop");
                while (!Disconnected)
                {
                    _dataReadyToSend.WaitOne();
                    while (
                        !Disconnected &&
                        _dataToEncrypt.TryDequeue(out var data)
                    )
                    {
                        DoSendData(data);
                    }
                }
                Trace.WriteLine("Stopped encryption loop");

            });
            _encryptAndSendThread.IsBackground = true;
            _encryptAndSendThread.Start();
        }

        private void DoSendData(byte[] data)
        {
            if (Disconnected) return;

            var toSend = new ArraySegment<byte>[2];
            var len = data.Length;

            if (_encryption)
            {
                toSend[0] = new ArraySegment<byte>(GenerateHeader(_encryptIV, len, _mapleVersion, _receivingFromServer));
                toSend[1] = new ArraySegment<byte>(Encrypt(data, len, _encryptIV));
            }
            else
            {
                toSend[0] = new ArraySegment<byte>(new byte[2] { (byte)len, (byte)((len >> 8) & 0xFF) });
                toSend[1] = new ArraySegment<byte>(data);
                _encryption = true; // First packet received or sent is unencrypted. All others are.
            }

            try
            {
                _socket.Send(toSend, SocketFlags.None);
            }
            catch (ObjectDisposedException)
            {
                // Socket is gone
                OnDisconnectINTERNAL("ObjectDisposed");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(TypeName + " [ERROR] Failed sending: {0}", ex.ToString());
                OnDisconnectINTERNAL("General Exception");
            }

            if (Disconnected) return;
        }

        #endregion

        #region Encryption Stuff
        /// <summary>
        /// Encrypts the given data, and updates the Encrypt IV
        /// </summary>
        /// <param name="pData">Data to be encrypted (without header!)</param>
        /// <returns>Encrypted data (with header!)</returns>
        private byte[] Encrypt(byte[] pData, int pLength, byte[] iv)
        {
            if (Config.Instance.ShandaEnabled)
                Shanda.Encrypt(pData, pLength);

            if (Config.Instance.AesEnabled)
                AesCrypt.Instance.Crypt(pData, iv);
            
            //Trace.WriteLine("Encrypted: " + BitConverter.ToString(cfgEncrypted));

            NextIV(iv);
            return pData;
        }

        /// <summary>
        /// Decrypts given data, and updates the Decrypt IV
        /// </summary>
        /// <param name="pData">Data to be decrypted</param>
        /// <returns>Decrypted data</returns>
        private byte[] Decrypt(byte[] pData, int pLength, byte[] iv)
        {
            if (Config.Instance.AesEnabled)
                AesCrypt.Instance.Crypt(pData, iv);

            if (Config.Instance.ShandaEnabled)
                Shanda.Decrypt(pData, pLength);

            NextIV(iv);
            return pData;
        }

        /// <summary>
        /// Generates a new IV code for AES and header generation. It will reset the oldIV with the newIV automatically.
        /// </summary>
        /// <param name="pOldIV">The old IV that is used already.</param>
        private static void NextIV(byte[] pOldIV)
        {
            byte[] newIV = new byte[] { 0xF2, 0x53, 0x50, 0xC6 };
            for (var i = 0; i < 4; i++)
            {
                byte input = pOldIV[i];
                byte tableInput = Config.Instance.IvShiftKey[input];
                newIV[0] += (byte)(Config.Instance.IvShiftKey[newIV[1]] - input);
                newIV[1] -= (byte)(newIV[2] ^ tableInput);
                newIV[2] ^= (byte)(Config.Instance.IvShiftKey[newIV[3]] + input);
                newIV[3] -= (byte)(newIV[0] - tableInput);

                uint val = BitConverter.ToUInt32(newIV, 0);
                uint val2 = val >> 0x1D;
                val <<= 0x03;
                val2 |= val;
                newIV[0] = (byte)(val2 & 0xFF);
                newIV[1] = (byte)((val2 >> 8) & 0xFF);
                newIV[2] = (byte)((val2 >> 16) & 0xFF);
                newIV[3] = (byte)((val2 >> 24) & 0xFF);
            }
            Buffer.BlockCopy(newIV, 0, pOldIV, 0, 4);
        }

        private const ushort IVAN_HEADER_SIZE = 0x7FFF;
        private const int HEADER_ERROR_MORE_DATA = -1;

        /// <summary>
        /// Retrieves length of content from the header
        /// </summary>
        /// <param name="pBuffer">Buffer containing the header</param>
        /// <returns>Length of buffer</returns>
        private static int GetHeaderLength(byte[] pBuffer, int pBufferLen, byte[] pIV, ushort pVersion, bool pToServer)
        {
            pVersion = Config.Instance.MapleCryptoVersion;
            ushort a = (ushort)(pBuffer[0] | (pBuffer[1] << 8));
            ushort b = (ushort)(pBuffer[2] | (pBuffer[3] << 8));
            ushort expectedIvPart = (ushort)((pIV[3] << 8) | pIV[2]);
            ushort expectedVersionPart = (ushort)(!pToServer ? pVersion : -(pVersion + 1));

            if ((a ^ expectedIvPart) != expectedVersionPart)
                throw new Exception($"Version mismatch {(a ^ expectedIvPart)} {expectedVersionPart} {(a ^ expectedVersionPart)} {expectedIvPart}");
            if ((a ^ expectedVersionPart) != expectedIvPart)
                throw new Exception($"IV mismatch {(a ^ expectedIvPart)} {expectedVersionPart} {(a ^ expectedVersionPart)} {expectedIvPart}");


            ushort len = (ushort)(a ^ b);

            if (len == IVAN_HEADER_SIZE)
            {
                if (pBufferLen == 4) return HEADER_ERROR_MORE_DATA;
                return pBuffer[4] |
                       pBuffer[5] << 8 |
                       pBuffer[6] << 16 |
                       pBuffer[7] << 24;
            }

            return len;
        }

        /// <summary>
        /// Generates header for packets
        /// </summary>
        /// <param name="pIV">IV</param>
        /// <param name="pLength">Packet Length - Header Length</param>
        /// <param name="pVersion">MapleStory Version</param>
        /// <param name="pToServer">Is to server?</param>
        private static byte[] GenerateHeader(byte[] pIV, int pLength, ushort pVersion, bool pToServer)
        {
            pVersion = Config.Instance.MapleCryptoVersion;
            ushort a = (ushort)((pIV[3] << 8) | pIV[2]);
            a ^= (ushort)(pToServer ? pVersion : -(pVersion + 1));

            byte[] headerCode;

            ushort b = a;

            if (pLength < IVAN_HEADER_SIZE)
            {
                b ^= (ushort)pLength;

                headerCode = new byte[4]
                {
                    (byte) (a & 0xFF),
                    (byte) ((a >> 8) & 0xFF),
                    (byte) (b & 0xFF),
                    (byte) ((b >> 8) & 0xFF),
                };
            }
            else
            {
                b ^= IVAN_HEADER_SIZE;

                headerCode = new byte[8]
                {
                    (byte) (a & 0xFF),
                    (byte) ((a >> 8) & 0xFF),
                    (byte) (b & 0xFF),
                    (byte) ((b >> 8) & 0xFF),

                    (byte) (pLength & 0xFF),
                    (byte) ((pLength >> 8) & 0xFF),
                    (byte) ((pLength >> 16) & 0xFF),
                    (byte) ((pLength >> 24) & 0xFF),
                };
            }

            return headerCode;
        }
        #endregion
    }
}