using System;
using System.Collections.Generic;
using System.Diagnostics;
using WvsBeta.Common.Sessions;

namespace WvsBeta.Common
{
    public class Pinger
    {
        public static int CurrentLoggingConnections => connections.Count;

        private static readonly List<ConnectionSession> connections = new List<ConnectionSession>();
        private static long lastPingTime = 0;
        private static readonly object lockobj = 1;

        public static void Add(ConnectionSession conn)
        {
            Trace.WriteLine("Adding connection " + conn.IP + ":" + conn.Port);
            lock (lockobj)
            {
                connections.Add(conn);
            }
        }

        public static void Remove(ConnectionSession conn)
        {
            Trace.WriteLine("Removing connection " + conn.IP + ":" + conn.Port);
            lock (lockobj)
            {
                connections.Remove(conn);
            }
        }

        public static void Init(Action<string> dcCallback = null)
        {
            var pAction = new MasterThread.RepeatingAction("Pinger", time => {
                if (lastPingTime != 0 && (time - lastPingTime) < Config.Instance.PingInterval)
                {
                    Trace.WriteLine($"Ignoring ping (too much!): {(time - lastPingTime)}");
                    return;
                }
                lastPingTime = time;
                ConnectionSession[] _connections;

                lock (lockobj)
                {
                    _connections = connections.ToArray();
                }

                foreach (ConnectionSession session in _connections)
                {
                    string sessionInfo = $"{session}";
                    try
                    {
                        if (session.gotPong || !session.sentSecondPing)
                        {
                            //if (session.gotPong)
                            //{
                            //    Trace.WriteLine($"[{sessionInfo}] Got pong");
                            //}
                            if (session.sentPing)
                            {
                                //Trace.WriteLine($"[{sessionInfo}] Sent second ping");
                                session.sentSecondPing = true;
                            }
                            else
                            {
                                session.sentPing = true;
                            }
                            session.gotPong = false;
                            session.pings = 0;
                        }
                        else if ((time - session.pingSentDateTime) > Config.Instance.PingInterval)
                        {
                            session.pings++;
                            //Trace.WriteLine($"[{session}] Ping failed, retrying ({session.pings}/8)");

                            #if !DEBUG
                            if (session.pings > 8)
                            {
                                throw new SessionDisconnectException("Too many retries, killing connection.");
                            }
                            #endif
                        }
                        //Trace.WriteLine($"[{sessionInfo}] Send ping");
                        session.SendPing();
                    }
                    catch (SessionDisconnectException e)
                    {
                        dcCallback?.Invoke($"[{sessionInfo}] Pinger disconnected! {e.Message}");

                        if (session.Disconnect())
                        {
                            // Killed
                            dcCallback?.Invoke($"[{sessionInfo}] Session is now disconnected.");
                        }
                        else
                        {
                            dcCallback?.Invoke($"[{sessionInfo}] Connection was already dead?! Getting rid of it.");
                            Remove(session);
                        }
                    }
                }
            }, Config.Instance.PingInterval, Config.Instance.PingInterval);
            pAction.Start();
        }
    }
}
