using System;

namespace WvsBeta.Common
{
    public class ControlledException : Exception
    {
        public ControlledException(string message) : base(message)
        {
        }
    }
}
