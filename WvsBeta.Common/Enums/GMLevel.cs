using System;

namespace WvsBeta.Common.Enums
{
    [Flags]
    public enum GMLevel : byte
    {
        None = 0,
        Tester = 0x10,
        GMIntern = 0x20,
        GM = 0x40,
        Admin = 0x80
    }
}
