using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using System.Globalization;

namespace WvsBeta
{
    public partial class MasterThread
    {
        private static ILog _log = LogManager.GetLogger("MasterThread");

        public static MasterThread Instance { get; private set; }

        private static DateTime _currentDate;
        public static DateTime CurrentDate
        {
            get => _currentDate;
            private set
            {
                _currentDate = value;
                CurrentTimeStr = value.ToString(DATESTR_FORMAT, CultureInfo.InvariantCulture);
            }
        }

        public static long CurrentTime => DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        public static long UnixTime => DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public static long FileTime => GetFileTime(UnixTime);
        public static long GetFileTime(long unix, int tzOffset = 0)
        {
            long offset = tzOffset * TimeSpan.TicksPerHour;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unix);
            return dateTimeOffset.ToFileTime() + offset;
        }
        public bool Stop { get; set; }
        public string ServerName { get; private set; }

        private Thread _masterThread;

        private AutoResetEvent _masterThreadResetEvent = new AutoResetEvent(false);

        private ConcurrentQueue<Tuple<String, Action<long>>> _callbacks = new ConcurrentQueue<Tuple<string, Action<long>>>();

        public int RegisteredRepeatingActions => RepeatingAction.RunningTasks;
        public int CurrentCallbackQueueLength => _callbacks.Count;

        private MasterThread(string pServerName)
        {
            ServerName = pServerName;
            Stop = false;
            _masterThread = new Thread(RunMasterThread)
            {
                Name = "MasterThread",
                IsBackground = true
            };
            _masterThread.Start();
        }

        public static void Load(string pServerName)
        {
            Instance = new MasterThread(pServerName);
        }

        [Obsolete("Just run pAction.Start()")]
        public void AddRepeatingAction(RepeatingAction pAction)
        {
            pAction.Start();
        }

        public bool IsNotMasterThread()
        {
            return Thread.CurrentThread != _masterThread;
        }

        /// <summary>
        /// This function removes a repeating action from the list
        /// </summary>
        /// <param name="pAction">The Repeating Action</param>
        /// <param name="pOnRemoved">Callback when the removal is performed.</param>
        public void RemoveRepeatingAction(RepeatingAction pAction, Action<DateTime, string, bool> pOnRemoved = null)
        {
            var isRemoved = pAction.Stop();
            pOnRemoved?.Invoke(CurrentDate, pAction.Name, isRemoved);
        }

        public void AddCallback(Action<long> pAction, string name)
        {
            _callbacks.Enqueue(new Tuple<string, Action<long>>(name, pAction));
            _masterThreadResetEvent.Set();
        }

        private void RunMasterThread()
        {
            Task.Factory.StartNew(() =>
            {
                while (!Stop)
                {
                    CurrentDate = DateTime.UtcNow;
                    Thread.Sleep(5);
                }
            }, TaskCreationOptions.LongRunning);


            Thread.BeginThreadAffinity();
            Stopwatch sw = new Stopwatch();
            do
            {

                while (_callbacks.TryDequeue(out var action))
                {
                    sw.Restart();
                    try
                    {
                        action.Item2(CurrentTime);
                    }
                    catch (Exception ex)
                    {
                        ////Console.WriteLine("Caught an exception inside the MainThread thread while running an action. Please, handle the exceptions yourself!\r\n{0}", ex.ToString());
                        _log.Error(
                            "Caught an exception inside the MainThread thread while running an action. Please, handle the exceptions yourself! Action: " + action.Item1,
                            ex);
                    }
                    sw.Stop();
                    if (sw.ElapsedMilliseconds > 100)
                    {
                        _log.Warn($"Slow callback! {sw.Elapsed} secs, check {action.Item1} {action.Item2}");
                    }
                }
                _masterThreadResetEvent.WaitOne();
            } while (!Stop);

            Thread.EndThreadAffinity();
        }
        #region Script helpers
        public static string CurrentTimeStr { get; private set; }
        public const string DATESTR_FORMAT = "yy/MM/dd/HH/mm";
        public static int CompareTime(string date1, string date2, string format = DATESTR_FORMAT)
        {
            if (
                !DateTime.TryParseExact(date1, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt1) ||
                !DateTime.TryParseExact(date2, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt2)
            )
            {
                return 0;
            }
            return (int)(dt1 - dt2).TotalMinutes;
        }
        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
        #endregion
    }
}