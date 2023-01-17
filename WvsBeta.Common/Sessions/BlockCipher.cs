using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using WvsBeta.Common.Crypto.Cryptography.Engines;

namespace WvsBeta.Common.Sessions
{
    internal class BlockCipher
    {
        private static readonly KeyParameter _keyParameter = new KeyParameter(new byte[16]
        {
            Session.sShiftKey[0], Session.sShiftKey[1], Session.sShiftKey[2], Session.sShiftKey[3],
            Session.sShiftKey[4], Session.sShiftKey[5], Session.sShiftKey[6], Session.sShiftKey[7],
            Session.sShiftKey[8], Session.sShiftKey[9], Session.sShiftKey[10], Session.sShiftKey[11],
            Session.sShiftKey[12], Session.sShiftKey[13], Session.sShiftKey[14], Session.sShiftKey[15]
        });

        public IBufferedCipher encryptCipher;
        public IBufferedCipher decryptCipher;
        public BlockCipher()
        {
            encryptCipher = GetCipher();
            encryptCipher.Init(true, new ParametersWithIV(_keyParameter, new byte[16]));
            decryptCipher = GetCipher();
            decryptCipher.Init(false, new ParametersWithIV(_keyParameter, new byte[16]));
        }
        private static IBufferedCipher GetCipher()
        {
            return new CtsBlockCipher(new CbcBlockCipher(new NopEngine()));
        }
        private static byte[] QuadIv(byte[] iv)
        {
            return new byte[16]
            {
                iv[0], iv[1], iv[2], iv[3],
                iv[0], iv[1], iv[2], iv[3],
                iv[0], iv[1], iv[2], iv[3],
                iv[0], iv[1], iv[2], iv[3],
            };
        }
        private static void MakeBufferList(int length, Action<(int Offset, int Length)> handler)
        {
            int chunkSize = 1456;
            int offset = 0;
            do
            {
                handler((offset, Math.Min(length - offset, chunkSize)));
                offset += chunkSize;
                chunkSize = 1460;
            } while (offset < length);
        }
        public void Encrypt(byte[] pData, int pLength, byte[] iv)
        {
            byte[] qiv = QuadIv(iv);
            MakeBufferList(pLength, b =>
            {
                var blobLen = b.Length;
                if (blobLen > 16)
                {
                    encryptCipher.Init(true, new ParametersWithIV(null, qiv));
                    encryptCipher.DoFinal(pData, b.Offset, blobLen, pData, b.Offset);
                }
                else
                {
                    for (var i = 0; i < blobLen; i++)
                    {
                        pData[b.Offset + i] ^= qiv[i % 16];
                    }
                }
            });
        }
        public void Decrypt(byte[] pData, int pLength, byte[] iv)
        {
            byte[] qiv = QuadIv(iv);
            MakeBufferList(pLength, b =>
            {
                var blobLen = b.Length;

                if (blobLen > 16)
                {
                    decryptCipher.Init(false, new ParametersWithIV(null, qiv));
                    decryptCipher.DoFinal(pData, b.Offset, blobLen, pData, b.Offset);
                }
                else
                {
                    for (var i = 0; i < blobLen; i++)
                    {
                        pData[b.Offset + i] ^= qiv[i % 16];
                    }
                }
            });
        }
    }
}
