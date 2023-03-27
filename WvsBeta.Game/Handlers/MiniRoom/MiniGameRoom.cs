using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WvsBeta.Common.Sessions;
using WvsBeta.Game.GameObjects.MiniRoom;

namespace WvsBeta.Game.Handlers.MiniRoom
{
    public class MiniGameRoom : BalloonRoom
    {
        public MiniGameType GameType { get; }
        public MiniGameRoom(GameCharacter owner, byte maxUsers, MiniRoomType type, string title, string password, bool @private, byte pieceType, MiniGameType gameType) : base(owner, maxUsers, type, title, password, @private)
        {
            PieceType = pieceType;
            GameType = gameType;
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

        public void StartGame(GameCharacter chr)
        {
            MiniGamePacket.Start(chr, chr.Room);
            GameStarted = true;
            SendBalloon(false);
        }

        public virtual void EndGame(GameCharacter winner, GameResult result)
        {
            foreach (var user in Users)
            {
                if (result == GameResult.Tie)
                {
                    user.Value.GameStats.AllStats[GameType].ties++;
                }
                else
                {
                    if (user.Value == winner)
                    {
                        winner.GameStats.AllStats[GameType].wins++;
                        if (winner.RoomSlotId == 0) mWinnerIndex = 1;
                        if (winner.RoomSlotId == 1) mWinnerIndex = 0;
                    }
                    else
                    {
                        user.Value.GameStats.AllStats[GameType].losses++;
                    }
                }
            }
            MiniGamePacket.EndGame(this, result, winner);
            
            GameStarted = false;
            SendBalloon(false);
            foreach (var leaveKVP in pendingLeaves.ToList())
            {
                RemovePlayer(leaveKVP.Value, MiniRoomLeaveReason.ForcedLeave);
                pendingLeaves.Remove(leaveKVP.Key);
            }
        }

        public void EncodeGameStats(GameCharacter user, Packet pw)
        {
            //GW_Minigamerecord_Decode (20 bytes)
            user.GameStats.AllStats[(MiniGameType)Type].Encode(pw);
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
            pw.WriteByte(PieceType); //Pieces type

            //Continue, no idea what this part is.
            pw.WriteByte(0);
            pw.WriteByte(0);
            pw.WriteLong(0);
        }
    }
}
