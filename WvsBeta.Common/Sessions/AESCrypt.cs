using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WvsBeta.Common.Crypto;

namespace WvsBeta.Common.Sessions
{
    internal class AESCrypt
    {
        const int BLOCK_SIZE = 16;
        const int KEY_SIZE = 32;
        const int IV_SIE = 16;

        private static readonly byte[] aesKey = {
            0x13, 0x00, 0x00, 0x00,
            0x08, 0x00, 0x00, 0x00,
            0x06, 0x00, 0x00, 0x00,
            0xB4, 0x00, 0x00, 0x00,
            0x1B, 0x00, 0x00, 0x00,
            0x0F, 0x00, 0x00, 0x00,
            0x33, 0x00, 0x00, 0x00,
            0x52, 0x00, 0x00, 0x00
        };
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

        // Taken from kagami
        public static void Process(byte[] buffer, int bufferLength, byte[] iv, bool decrypt)
        {
            try
            {
                var qiv = QuadIv(iv);
                int pLength = (int)Math.Ceiling((double)bufferLength / BLOCK_SIZE) * BLOCK_SIZE;
                byte[] pBuffer = new byte[pLength];
                Array.Copy(buffer, 0, pBuffer, 0, bufferLength);
                byte[] output;
                if (decrypt)
                {

                    output = AesCryptographyService.Decrypt(pBuffer, aesKey, qiv);
                }
                else
                {
                    output = AesCryptographyService.Encrypt(pBuffer, aesKey, qiv);
                }
                Array.Copy(output, buffer, bufferLength);
            }
            catch (Exception e)
            {

            }

        }
    }
}
