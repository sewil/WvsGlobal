using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Linq;

namespace WvsBeta.Common.Crypto
{
    public class AesCrypt
    {
        private readonly AesEngine _cipher;
        private static readonly AesCrypt _instance;
        public static AesCrypt Instance => _instance ?? new AesCrypt();

        private AesCrypt()
        {
            _cipher = new AesEngine();
            _cipher.Init(true, ParameterUtilities.CreateKeyParameter("AES", Config.Instance.AesUserKey));
        }

        private static byte[] QuadIV(byte[] iv)
        {
            return Enumerable.Repeat(iv[0], 16).ToArray();
        }

        public void Crypt(byte[] buffer, byte[] iv)
        {
            int remaining = buffer.Length;
            int blockSize = Math.Min(0x5B0, remaining);
            int bufferPos = 0;
            byte[] qiv = QuadIV(iv);

            while (remaining > 0)
            {
                ProcessBlock(buffer, bufferPos, blockSize, qiv, remaining == blockSize);
                bufferPos += blockSize;
                remaining -= blockSize;
                blockSize = Math.Min(0x5B4, remaining);
            }
        }

        private void ProcessBlock(byte[] buffer, int bufferPos, int blockSize, byte[] iv, bool isFinal)
        {
            var streamCipher = new BufferedBlockCipher(new OfbBlockCipher(_cipher, _cipher.GetBlockSize() * 8));
            streamCipher.Init(true, new ParametersWithIV(null, iv));

            if (isFinal) streamCipher.DoFinal(buffer, bufferPos, blockSize, buffer, bufferPos);
            else streamCipher.ProcessBytes(buffer, bufferPos, blockSize, buffer, bufferPos);
        }
    }
}
