using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Common.Sessions
{
    internal class Shanda
    {
        /// <summary>
        /// Rolls the value left. Port from NLS (C++) _rotl8
        /// </summary>
        /// <param name="value">Value to be shifted</param>
        /// <param name="shift">Position to shift to</param>
        /// <returns>Shifted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte RollLeft(byte value, int shift)
        {
            uint overflow = ((uint)value) << (shift % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }

        /// <summary>
        /// Rolls the value right. Port from NLS (C++) _rotr8
        /// </summary>
        /// <param name="value">Value to be shifted</param>
        /// <param name="shift">Position to shift to</param>
        /// <returns>Shifted value</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte RollRight(byte value, int shift)
        {
            uint overflow = (((uint)value) << 8) >> (shift % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }
        public static void Encrypt(byte[] pData, int pLength)
        {
            int length = pLength, j;
            byte a, c;
            for (var i = 0; i < 3; i++)
            {
                a = 0;
                for (j = length; j > 0; j--)
                {
                    c = pData[length - j];
                    c = RollLeft(c, 3);
                    c = (byte)(c + j);
                    c ^= a;
                    a = c;
                    c = RollRight(a, j);
                    c ^= 0xFF;
                    c += 0x48;
                    pData[length - j] = c;
                }
                a = 0;
                for (j = length; j > 0; j--)
                {
                    c = pData[j - 1];
                    c = RollLeft(c, 4);
                    c = (byte)(c + j);
                    c ^= a;
                    a = c;
                    c ^= 0x13;
                    c = RollRight(c, 3);
                    pData[j - 1] = c;
                }
            }
        }
        public static void Decrypt(byte[] pData, int pLength)
        {
            int length = pLength, j;
            byte a, b, c;
            for (var i = 0; i < 3; i++)
            {
                a = 0;
                b = 0;
                for (j = length; j > 0; j--)
                {
                    c = pData[j - 1];
                    c = RollLeft(c, 3);
                    c ^= 0x13;
                    a = c;
                    c ^= b;
                    c = (byte)(c - j);
                    c = RollRight(c, 4);
                    b = a;
                    pData[j - 1] = c;
                }
                a = 0;
                b = 0;
                for (j = length; j > 0; j--)
                {
                    c = pData[length - j];
                    c -= 0x48;
                    c ^= 0xFF;
                    c = RollLeft(c, j);
                    a = c;
                    c ^= b;
                    c = (byte)(c - j);
                    c = RollRight(c, 3);
                    b = a;
                    pData[length - j] = c;
                }
            }
        }
    }
}
