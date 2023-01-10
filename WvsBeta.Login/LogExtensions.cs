using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WvsBeta.Login
{
    internal static class LogExtensions
    {
        public static bool AssertWarning(this ILog log, bool assertion, string msg)
        {
            if (assertion)
            {
                log.Warn(msg);
                Server.Instance.ServerTraceDiscordReporter.Enqueue($"AssertWarning: {msg}");
            }
            return assertion;
        }

        public static bool AssertError(this ILog log, bool assertion, string msg)
        {
            if (assertion)
            {
                log.Error(msg);
                Program.MainForm.LogAppend(msg);
                Server.Instance.ServerTraceDiscordReporter.Enqueue($"AssertError: {msg}");
            }
            return assertion;
        }
    }
}
