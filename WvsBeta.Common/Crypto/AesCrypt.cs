using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

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

        private static byte[] Iv32(byte[] iv)
        {
            byte[] qiv = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                qiv[i] = iv[0];
            }
            return qiv;
        }

        public void Crypt(byte[] data, byte[] iv)
        {
            int remaining = data.Length;
            int blockSize = 0x5B0;
            int bufferPos = 0;

            while (remaining > 0)
            {
                if (remaining < blockSize)
                {
                    blockSize = remaining;
                }
                ProcessBlock(bufferPos, data, blockSize, iv, remaining == blockSize);
                bufferPos += blockSize;
                remaining -= blockSize;
                blockSize = 0x5B4;
            }
        }

        private void ProcessBlock(int bufferPos, byte[] buffer, int blockSize, byte[] iv, bool isFinal)
        {
            byte[] qiv = Iv32(iv);

            var streamCipher = new BufferedBlockCipher(new OfbBlockCipher(_cipher, _cipher.GetBlockSize() * 8));
            streamCipher.Init(true, new ParametersWithIV(null, qiv));

            if (isFinal) streamCipher.DoFinal(buffer, bufferPos, blockSize, buffer, bufferPos);
            else streamCipher.ProcessBytes(buffer, bufferPos, blockSize, buffer, bufferPos);
        }
    }
}
