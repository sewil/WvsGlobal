using System;
using System.Collections.Generic;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.GameObjects.MiniRoom;

namespace WvsBeta.Game.Handlers.MiniRoom
{
    public abstract class MiniGameRoom : BalloonRoom
    {
        public byte mCurrentTurnIndex { get; set; }
        public MiniGameType GameType { get; }
        private long NextAvailableTie;
        private IDictionary<byte, int> forfeits;
        public MiniGameRoom(GameCharacter owner, byte maxUsers, MiniRoomType type, string title, string password, bool @private, byte boardType, MiniGameType gameType) : base(owner, maxUsers, type, title, password, @private)
        {
            GameBoardType = boardType;
            GameType = gameType;
            forfeits = new Dictionary<byte, int>();
        }

        private IDictionary<int, GameCharacter> pendingLeaves = new Dictionary<int, GameCharacter>();
        public void RequestLeave(GameCharacter chr)
        {
            pendingLeaves[chr.ID] = chr;
            MiniRoomPacket.ChatNotice(this, chr, MiniRoomChatNoticeType.CalledToLeave);
        }

        public void CancelRequestLeave(GameCharacter chr)
        {
            pendingLeaves.Remove(chr.ID);
            MiniRoomPacket.ChatNotice(this, chr, MiniRoomChatNoticeType.LeaveCancelled);
        }

        public override void RemovePlayer(GameCharacter pCharacter, MiniRoomLeaveReason pReason)
        {
            if (GameStarted) Close(reason: MiniRoomLeaveReason.Cancel);
            // Crash if:
            // - Game is in progress
            // - Is Owner (idx == 0) && reason == Cancel
            else base.RemovePlayer(pCharacter, pReason);
        }

        public override void Close(bool sendPacket = true, MiniRoomLeaveReason reason = MiniRoomLeaveReason.Closed)
        {
            if (GameStarted && reason == MiniRoomLeaveReason.Cancel) // Player DC
            {
                //MiniRoomPacket.SendLeaveRoom(Owner, MiniRoomLeaveReason.Cancel);
                MiniRoomPacket.ChatNotice(this, null, MiniRoomChatNoticeType.GameHasEnded);
                MasterThread.RepeatingAction.Start(() => Close(), 10000, 0);
            }
            else
            {
                base.Close(sendPacket, reason);
            }
        }

        public abstract void StartGame(GameCharacter chr);

        protected void SendStartGame(Packet pw)
        {
            BroadcastPacket(pw);
            GameStarted = true;
            SendBalloon(false);
        }

        public virtual void EndGame(GameCharacter winner, GameResult result)
        {
            GameCharacter loser = GetOtherUser(winner.RoomSlotId);
            if (loser == null)
            {
                Program.MainForm.LogAppend("Couldn't find minigame loser from winner slot " + winner.RoomSlotId);
                return;
            }
            var winnerStats = winner.GameStats[GameType];
            var loserStats = loser.GameStats[GameType];
            bool tie = result == GameResult.Tie;
            if (tie)
            {
                winnerStats.ties++;
                loserStats.ties++;
                if (NextAvailableTie < MasterThread.CurrentTime)
                {
                    winnerStats.points += 10;
                    loserStats.points += 10;
                    NextAvailableTie = MasterThread.CurrentTime + (60*5*1000);
                }
            }
            else
            {
                winnerStats.wins++;
                loserStats.losses++;
                int loserForfeits = 0;
                bool forfeit = result == GameResult.Forfeit;
                if (loserForfeits < 4 || !forfeit)
                {
                    winnerStats.points += 50;
                }
                loserStats.points += (15 * (forfeit ? -1 : 1 ));
                if (forfeit)
                {
                    forfeits.TryGetValue(loser.RoomSlotId, out loserForfeits);
                    loserForfeits++;
                    forfeits[loser.RoomSlotId] = loserForfeits;
                }
            }
            MiniGamePacket.EndGame(this, result, winner);

            GameStarted = false;
            SendBalloon(false);
            foreach (var leaveKVP in pendingLeaves.ToList())
            {
                if (leaveKVP.Value == Owner) Close(reason: MiniRoomLeaveReason.Closed);
                else RemovePlayer(leaveKVP.Value, MiniRoomLeaveReason.ForcedLeave);
                pendingLeaves.Remove(leaveKVP.Key);
            }
        }

        public void EncodeGameStats(GameCharacter user, Packet pw)
        {
            //GW_Minigamerecord_Decode (20 bytes)
            user.GameStats[(MiniGameType)Type].Encode(pw);
        }

        public override void EncodeEnter(GameCharacter pCharacter, Packet pw)
        {
            base.EncodeEnter(pCharacter, pw);
            EncodeGameStats(pCharacter, pw);
        }
        public override void EncodeEnterResult(GameCharacter pCharacter, Packet pw)
        {
            base.EncodeEnterResult(pCharacter, pw);

            foreach (var user in Users)
            {
                pw.WriteByte(user.Value.RoomSlotId);
                EncodeGameStats(user.Value, pw);
            }

            pw.WriteByte(0xFF);
            //Rest of packet
            pw.WriteString(Title);
            pw.WriteByte(GameBoardType); //Pieces type

            //Continue, no idea what this part is.
            pw.WriteByte(0);
            pw.WriteByte(0);
            pw.WriteLong(0);
        }
    }
}
