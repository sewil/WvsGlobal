using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Common.Extensions
{
    public static class BitExtensions
    {
        public static uint RotateLeft(this uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
    }

        public static uint RotateRight(this uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
    }
}
