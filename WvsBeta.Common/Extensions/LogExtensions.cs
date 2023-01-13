using log4net;
using System.Diagnostics;

namespace WvsBeta.Common.Extensions
{
    public static class LogExtensions
    {
        public static bool AssertWarning(this ILog log, bool assertion, string msg)
        {
            if (assertion)
            {
                log.Warn(msg);
                Trace.WriteLine(msg);
            }
            return assertion;
        }

        public static bool AssertError(this ILog log, bool assertion, string msg)
        {
            if (assertion)
            {
                log.Error(msg);
                Trace.WriteLine(msg);
            }
            return assertion;
        }
    }
}
