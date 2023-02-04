using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public interface INpcScript : IGameScript
    {
        void Run(INpcHost self, GameCharacter target);
    }

    public interface INpcHost
    {
        int mID { get; }
        void Say(string message);
        int AskYesNo(string Message);
        int AskMenu(string Message);
        int AskNumber(string Message, int Default, int MinValue, int MaxValue);
        string AskPet(string message);
        string AskText(string Message, string Default, short MinLength, short MaxLength);
        string AskPetAllExcept(string message, string petcashid);
        int AskStyle(string Message, List<int> Values);

        object GetStrReg(string pName);
        void SetStrReg(string pName, object pValue);
    }
    public class NpcEscapeException : Exception { }
    public enum NpcState
    {
        Next = 0,
        YesNo = 1,
        Text = 2,
        RequestInteger = 3,
        Menu = 4,
        Style = 5,
        Pet = 6
    }
    public class NpcChatSession : INpcHost
    {
        public int mID { get; set; }
        public GameCharacter mCharacter { get; set; }
        public bool Stopped => mCharacter.NpcSession == null;
        private INpcScript compiledScript = null;

        private List<string> sayLines { get; set; } = new List<string>();
        private Dictionary<string, object> _savedObjects = new Dictionary<string, object>();

        private int sayIndex { get; set; } = 0;
        private byte nRet = 0;
        private string nRetStr;
        private int nRetNum = 0;
        public NpcState mLastSentType { get; set; }
        public bool WaitingForResponse { get; set; }

        private Task task;
        public NpcChatSession(int id, GameCharacter chr, INpcScript npcScript)
        {
            mID = id;
            mCharacter = chr;
            mCharacter.NpcSession = this;
            compiledScript = (INpcScript)npcScript.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(npcScript, null);
            task = Task.Factory.StartNew(() =>
            {
                try
                {
                    compiledScript.Run(this, mCharacter);
                }
                catch { }
                finally
                {
                    Stop();
                }
            });
        }
        
        public static void Start(int npcId, string script, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            Start(npcId, (INpcScript)ScriptAccessor.GetScript(Server.Instance, script, errorHandlerFnc), chr);
        }
        
        public static void Start(int npcId, INpcScript npcScript, GameCharacter chr)
        {
            if (npcScript == null || chr.NpcSession != null) return;

            new NpcChatSession(npcId, chr, npcScript);
        }

        public void HandleResponse(byte nRet = 0, string nRetStr = "", int nRetNum = 0)
        {
            this.nRet = nRet;
            this.nRetStr = nRetStr;
            this.nRetNum = nRetNum;
            WaitingForResponse = false;
            #if DEBUG
                ChatPacket.SendText(ChatPacket.MessageTypes.Notice, "nRet:" + nRet + ",stringAnswer:" + nRetStr + ",nRetNum:" + nRetNum, mCharacter, ChatPacket.MessageMode.ToPlayer);
            #endif
        }

        public void Stop()
        {
            mCharacter.NpcSession = null;
            WaitingForResponse = false;
            compiledScript = null;
            task = null;
            nRet = 0;
            nRetStr = "";
            nRetNum = 0;
        }

        private void WaitForResponse()
        {
            while (WaitingForResponse) continue;
            if (Stopped) throw new NpcEscapeException();
        }

        public void RespondPrevious()
        {
            if (sayIndex == 0 || sayLines.Count == 0) return;
            sayIndex--;
            if (sayLines.Count < sayIndex) return;

            WaitingForResponse = true;
            string line = sayLines[sayIndex];
            bool hasBack = sayIndex > 0;
            NpcPacket.SendNPCChatTextSimple(mCharacter, mID, line, hasBack, true);
        }

        public void RespondNext()
        {
            if (sayLines.Count == sayIndex + 1) // Last say, continue script execution
            {
                HandleResponse(0, "", 0);
            }
            else
            {
                sayIndex++;
                if (sayLines.Count < sayIndex) return;

                WaitingForResponse = true;
                string line = sayLines[sayIndex];
                NpcPacket.SendNPCChatTextSimple(mCharacter, mID, line, true, true);
            }
        }
        public void Say(string message)
        {
            if (Stopped) throw new NpcEscapeException();
            sayLines.Add(message);
            sayIndex = sayLines.Count - 1;
            WaitingForResponse = true;
            NpcPacket.SendNPCChatTextSimple(mCharacter, mID, message, sayIndex > 0, true);
            WaitForResponse();
        }

        public int AskMenu(string Message)
        {
            if (Stopped) throw new NpcEscapeException();
            sayLines.Clear();
            sayIndex = 0;
            WaitingForResponse = true;
            NpcPacket.SendNPCChatTextMenu(mCharacter, mID, Message);
            WaitForResponse();
            return nRet;
        }

        public int AskYesNo(string Message)
        {
            if (Stopped) throw new NpcEscapeException();
            sayLines.Clear();
            sayIndex = 0;
            WaitingForResponse = true;
            NpcPacket.SendNPCChatTextYesNo(mCharacter, mID, Message);
            WaitForResponse();
            return nRet;
        }

        public string AskText(string Message, string Default, short MinLength, short MaxLength)
        {
            if (Stopped) throw new NpcEscapeException();
            sayLines.Clear();
            sayIndex = 0;
            WaitingForResponse = true;
            NpcPacket.SendNPCChatTextRequestText(mCharacter, mID, Message, Default, MinLength, MaxLength);
            WaitForResponse();
            return nRetStr;
        }

        public int AskNumber(string Message, int Default, int MinValue, int MaxValue)
        {
            if (mCharacter.NpcSession == null) throw new NpcEscapeException();

            sayLines.Clear();
            sayIndex = 0;
            WaitingForResponse = true;
            NpcPacket.SendNPCChatTextRequestInteger(mCharacter, mID, Message, Default, MinValue, MaxValue);
            WaitForResponse();
            return nRetNum;
        }

        public int AskStyle(string Message, List<int> Values)
        {
            if (mCharacter.NpcSession == null) throw new NpcEscapeException();

            sayLines.Clear();
            sayIndex = 0;
            WaitingForResponse = true;
            NpcPacket.SendNPCChatTextRequestStyle(mCharacter, mID, Message, Values);
            WaitForResponse();
            return nRet;
        }

        public string AskPet(string message)
        {
            if (mCharacter.NpcSession == null) throw new NpcEscapeException();
            sayLines.Clear();
            sayIndex = 0;
            if (string.IsNullOrWhiteSpace(message)) // Check if have any pets
            {
                return mCharacter.Inventory.GetPets().FirstOrDefault()?.CashId.ToString() ?? "";
            }
            else
            {
                WaitingForResponse = true;
                NpcPacket.SendNPCChatTextRequestPet(mCharacter, mID, message);
                WaitForResponse();
                return nRetStr;
            }
        }

        public string AskPetAllExcept(string message, string petid)
        {
            if (mCharacter.NpcSession == null) throw new NpcEscapeException();
            sayLines.Clear();
            sayIndex = 0;
            WaitingForResponse = true;

            int petskip = -1;
            try
            {
                petskip = int.Parse(petid);
            }
            catch { }
            NpcPacket.SendNPCChatTextRequestPet(mCharacter, mID, message, petskip);
            WaitForResponse();
            return nRetStr;
        }

        public object GetStrReg(string pName)
        {
            if (_savedObjects.ContainsKey(pName)) return _savedObjects[pName];
            return null;
        }

        public void SetStrReg(string pName, object pValue)
        {
            if (!_savedObjects.ContainsKey(pName))
                _savedObjects.Add(pName, pValue);
            else
                _savedObjects[pName] = pValue;
        }
    }
}
