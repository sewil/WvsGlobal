using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Linq;
using static WvsBeta.Login.ClientSession;
using WvsBeta.Common;
using WvsBeta.Login.Packets;
using WvsBeta.Common.Sessions;
using log4net;
using static WvsBeta.Login.Packets.CheckPasswordResultPacket;
using WvsBeta.Common.Extensions;
using WvsBeta.Common.Enums;

namespace WvsBeta.Login.PacketHandlers
{
    internal class CheckPasswordHandler
    {
        private ILog log;
        private string username;
        private string password;
        private int localUserId;
        private int uniqueID;
        private short patchVersion;
        public CheckPasswordHandler(ClientSession session, ILog log, Packet packet, ref string crashLogTmp)
        {
            this.log = log;
            if (log.AssertWarning(session.Player.State != GameState.LoginScreen, "Player tried to login while not in loginscreen."))
            {
                Program.MainForm.LogAppend("Disconnected client (4)");
                session.Disconnect();
                return;
            }

            username = packet.ReadString();
            password = packet.ReadString();

            if (log.AssertWarning(username.Length < 4 || username.Length > 12, "Username length wrong (len: " + username.Length + "): " + username) ||
                log.AssertWarning(password.Length < 4 || password.Length > 12, "Password length wrong (len: " + password.Length + ")"))
            {
                session.Disconnect();
                return;
            }

            string machineID = string.Join("", packet.ReadBytes(6).Select(x => x.ToString("X2")));
            int startupThingy = packet.ReadInt();

            localUserId = packet.ReadInt();
            uniqueID = packet.ReadInt();
            patchVersion = packet.ReadShort();

            if (Server.Instance.CurrentPatchVersion > patchVersion)
            {
                WriteLoginInfo();

                // Figure out how to patch
                if (Server.Instance.PatchNextVersion.TryGetValue(patchVersion, out var toVersion))
                {
                    var p = new Packet(0xC2);
                    p.WriteShort(toVersion);
                    session.SendPacket(p);
                    log.Info($"Sent patchexception packet ({patchVersion} -> {toVersion})");
                    return;
                }
                else
                {
                    log.AssertError(true, $"No patch strategy to go from {patchVersion} to {Server.Instance.CurrentPatchVersion}");
                    session.Disconnect();
                    return;
                }
            }

            LoginState loginState = LoginState.SYSTEM_ERROR;
            string dbpass = String.Empty;
            bool updateDBPass = false;
            BanReason banReason = BanReason.Unknown;
            long banExpire = 0;
            int userId = 0;

            using (var data = Server.Instance.UsersDatabase.RunQuery(
                "SELECT * FROM users WHERE username = @username",
                "@username", username
            ) as MySqlDataReader)
            {
                if (!data.Read())
                {
                    log.Warn($"[{username}] account does not exist");
                    loginState = LoginState.NOT_REGISTERED;
                }
                else
                {
                    username = data.GetString("username");
                    userId = data.GetInt32("ID");
                    dbpass = data.GetString("password");
                    banReason = (BanReason)data.GetByte("ban_reason");
                    banExpire = data.GetMySqlDateTime("ban_expire").Value.ToFileTimeMillis();
                    string pin = data.IsDBNull(5) ? null : data.GetString("pin");

                    if (RedisBackend.Instance.IsPlayerOnline(userId))
                    {
                        log.AssertWarning(true, $"[{username}][{userId}] already online");
                        loginState = LoginState.ALREADY_ONLINE;
                    }
                    else if (Server.Instance.DisableMultiIP && RedisBackend.Instance.IsPlayerOnline(session.IP))
                    {
                        loginState = LoginState.SYSTEM_ERROR;
                        log.AssertWarning(true, $"[{username}][{userId}] tried logging in on the same IP ({session.IP})");
                    }
                    else if (banExpire > MasterThread.CurrentTime)
                    {
                        log.AssertWarning(true, $"[{username}][{userId}] banned until " + data.GetDateTime("ban_expire"));
                        loginState = LoginState.BANNED;
                    }
                    else if (dbpass.Length > 1 && dbpass[0] != '$')
                    {
                        // Unencrypted
                        if (dbpass == password)
                        {
                            loginState = LoginState.SUCCESS;
                            dbpass = BCrypt.HashPassword(password, BCrypt.GenerateSalt());
                            updateDBPass = true;
                        }
                        else
                        {
                            loginState = LoginState.INVALID_PASSWORD;
                        }
                    }
                    else if (BCrypt.CheckPassword(password, dbpass))
                    {
                        loginState = LoginState.SUCCESS;
                    }
                    else
                    {
                        loginState = LoginState.INVALID_PASSWORD;
                    }

                    if (loginState == LoginState.SUCCESS)
                    {
                        session.Player.ID = userId;
                        if (Server.Instance.RequiresEULA && data.GetBoolean("confirmed_eula") == false)
                        {
                            loginState = LoginState.EULA;
                        }
                        else
                        {
                            session.Player.PIN = pin;
                            session.Player.GMLevel = data.GetByte("admin");
                            session.Player.Gender = (PlayerGender)data.GetByte("gender");
                            session.Player.DateOfBirth = data.GetInt32("char_delete_password");
                            session.Player.Username = username;
                        }
                    }
                    else if (loginState == LoginState.INVALID_PASSWORD)
                    {
                        log.Warn($"[{username}][{userId}] invalid password");
                    }
                }
            }

            int machineBanCount = 0, uniqueBanCount = 0, ipBanCount = 0;

            if (loginState == LoginState.SUCCESS)
            {
                session.Loaded = true;
                session.StartLogging();

                WriteLoginInfo();

                bool macBanned = false;
                using (var mdr = Server.Instance.UsersDatabase.RunQuery(
                    "SELECT 1 FROM machine_ban WHERE machineid = @machineId OR machineid = @uniqueId",
                    "@machineId", machineID,
                    "@uniqueId", uniqueID) as MySqlDataReader)
                {
                    if (mdr.HasRows)
                    {
                        macBanned = true;
                    }
                }

                // Outside of using statement because of secondary query
                if (log.AssertWarning(macBanned,
                    $"[{username}][{userId}] tried to login on a machine-banned account for machineid {machineID}."))
                {
                    session.Disconnect();

                    Server.Instance.UsersDatabase.RunQuery(
                        "UPDATE machine_ban SET last_try = CURRENT_TIMESTAMP, last_username = @username, last_unique_id = @uniqueId, last_ip = @ip WHERE machineid = @machineId OR machineid = @uniqueId",
                        "@ip", session.IP,
                        "@username", username,
                        "@machineId", machineID,
                        "@uniqueId", uniqueID
                    );
                    return;
                }

                using (var mdr =
                    Server.Instance.UsersDatabase.RunQuery("SELECT 1 FROM ipbans WHERE ip = @ip", "@ip", session.IP) as
                        MySqlDataReader)
                {
                    if (mdr.HasRows)
                    {
                        log.AssertError(true, $"[{username}][{userId}] tried to login on a ip-banned account for ip {session.IP}.");
                        session.Disconnect();
                        return;
                    }
                }

                var (maxMachineBanCount, maxUniqueBanCount, maxIpBanCount) =
                    Server.Instance.UsersDatabase.GetUserBanRecordLimit(session.Player.ID);
                (machineBanCount, uniqueBanCount, ipBanCount) =
                    Server.Instance.UsersDatabase.GetBanRecord(machineID, uniqueID.ToString(), session.IP);

                // Do not use MachineID banning, as its not unique enough
                if (ipBanCount >= maxIpBanCount ||
                    uniqueBanCount >= maxUniqueBanCount)
                {
                    log.AssertError(true,
                        $"[{username}][{userId}] tried to log in an account where a machineid, uniqueid and/or ip has already been banned for " +
                        $"{machineBanCount}/{uniqueBanCount}/{ipBanCount} times. " +
                        $"(Max values: {maxMachineBanCount}/{maxUniqueBanCount}/{maxIpBanCount})");

                    if (ipBanCount >= maxIpBanCount)
                    {
                        loginState = LoginState.MASTER_CANNOT_LOGIN_ON_THIS_IP; // rip.
                    }
                    else
                    {
                        session.Disconnect();
                        return;
                    }
                }
            }
            else
            {
                WriteLoginInfo();
            }

            var pack = new CheckPasswordResultPacket(loginState, session.Player, username, banReason, banExpire);
            session.SendPacket(pack);

            if (loginState != LoginState.SUCCESS)
            {
                session.Loaded = false;
                if (loginState == LoginState.EULA)
                {
                    session.Player.State = GameState.ConfirmEULA;
                }
                return;
            }

            // Player logged in
            session.TryRegisterHackDetection();
            RedisBackend.Instance.SetPlayerOnline(session.Player.ID, 1, session.IP);

            if (crashLogTmp != null)
            {
                var crashlogName = session.IP + "-" + username + ".txt";
                FileWriter.WriteLine(Path.Combine("ClientCrashes", crashlogName), crashLogTmp);
                crashLogTmp = null;
                Server.Instance.ServerTraceDiscordReporter.Enqueue($"Saving crashlog to {crashlogName}");
            }

            session.Player.LoggedOn = true;
            session.Player.State = session.Player.Gender == PlayerGender.Unset ? GameState.SetupGender : GameState.PinCheck;

            Program.MainForm.LogAppend($"Account {username} ({session.Player.ID}) logged on. Machine ID: {machineID}, Unique ID: {uniqueID}, IP: {session.IP}, Ban counts: {machineBanCount}/{uniqueBanCount}/{ipBanCount}");
            Program.MainForm.ChangeLoad(true);

            // Update database
            Server.Instance.UsersDatabase.RunQuery(
                @"
                    UPDATE users SET 
                    last_login = NOW(), 
                    last_ip = @ip, 
                    last_machine_id = @machineId, 
                    last_unique_id = @uniqueId 
                    WHERE ID = @id",
                "@id", session.Player.ID,
                "@ip", session.IP,
                "@machineId", machineID,
                "@uniqueId", uniqueID
            );

            if (updateDBPass)
            {
                Server.Instance.UsersDatabase.RunQuery(
                    "UPDATE users SET password = @password WHERE ID = @id",
                    "@id", session.Player.ID,
                    "@password", dbpass
                );
            }
        }

        private void WriteLoginInfo()
        {
            log.Info(new LoginLoggingStruct
            {
                localUserId = localUserId,
                uniqueId = uniqueID,
                username = username
            });
        }
    }
}
