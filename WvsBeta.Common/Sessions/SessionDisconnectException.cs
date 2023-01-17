using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Common.Sessions
{
    public class SessionDisconnectException : Exception
    {
        public SessionDisconnectException(string message): base(message)
        {

        }
    }
}
