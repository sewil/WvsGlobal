using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WvsBeta.Common;
using WvsBeta.Game.Scripting;

namespace WvsBeta.Game
{
    public interface INpcScript : IGameScript
    {
        void Run(INpcHost self, GameCharacter target);
    }

    public interface INpcHost
    {
        int mNpcID { get; }
        void Say(string message);
        int AskYesNo(string Message);
        int AskMenu(string Message);
        int AskNumber(string Message, int Default, int MinValue, int MaxValue);
        string AskPet(string message);
        string AskText(string Message, string Default, short MinLength, short MaxLength);
        string AskPetAllExcept(string message, string petcashid);
        int AskAvatar(string message, int coupon, params int[] values);
        int MakeRandAvatar(int coupon, params int[] values);
        void AskShop(IEnumerable<ShopItemData> items);
        void AskShop(params ShopItemData[] items);
        object GetStrReg(string pName);
        void SetStrReg(string pName, object pValue);
        void Log(string text);
    }
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
    public enum AvatarSelectState
    {
        SysError = -3,
        Error = -2,
        MissingCoupon = -1,
        Success = 1,
    }
    public class NpcChatSession : INpcHost
    {
        public int mNpcID { get; set; }
        public GameCharacter mCharacter { get; set; }
        private INpcScript compiledScript = null;

        private List<string> sayLines { get; set; } = new List<string>();
        private readonly Dictionary<string, object> scriptVars;

        private int sayIndex { get; set; } = 0;
        private byte nRet = 0;

        private AvatarSelectState nAvatarSelectState;
        private string nRetStr;
        private int nRetNum = 0;
        public NpcState mLastSentType { get; set; }
        public bool WaitingForResponse => thread.ThreadState == ThreadState.WaitSleepJoin;
        private EventWaitHandle ewh;
        private Thread thread;
        public NpcChatSession(int npcId, GameCharacter chr, INpcScript npcScript, string scriptName)
        {
            mNpcID = npcId;
            mCharacter = chr;
            mCharacter.NpcSession = this;
            scriptVars = Server.Instance.ScriptVars[scriptName];
            compiledScript = (INpcScript)npcScript.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(npcScript, null);
            if (chr.IsGM && chr.Job == 500) SetStrReg("name", chr.Name);
            ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
            thread = new Thread(new ParameterizedThreadStart(RunScript));
            thread.Start(this);
        }
        private void RunScript(object o)
        {
            NpcChatSession s = (NpcChatSession)o;
            try
            {
                compiledScript.Run(s, s.mCharacter);
            }
            catch (ThreadAbortException) { }
            finally
            {
                s.Stop();
            }
        }

        private static INpcScript GetScript(NPCData npc, Action<string> errorHandlerFnc, out string scriptName)
        {
            INpcScript script = null;
            scriptName = null;
            if (npc.Quest != null)
            {
                scriptName = npc.Quest;
                script = (INpcScript)ScriptAccessor.GetScript(Server.Instance, scriptName, errorHandlerFnc);
            }
            if (script == null)
            {
                scriptName = npc.ID.ToString();
                script = (INpcScript)ScriptAccessor.GetScript(Server.Instance, scriptName, errorHandlerFnc);
            }
            if (script == null) scriptName = null;
            return script;
        }
        
        public static void Start(NPCData npc, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            INpcScript script = GetScript(npc, errorHandlerFnc, out string scriptName);
            if (script == null || chr.NpcSession != null) return;

            new NpcChatSession(npc.ID, chr, script, scriptName);
        }

        public void HandleResponse(byte nRet = 0, string nRetStr = "", int nRetNum = 0)
        {
            this.nRet = nRet;
            this.nRetStr = nRetStr;
            this.nRetNum = nRetNum;
            ewh.Set();
            #if DEBUG
                ChatPacket.SendText(ChatPacket.MessageTypes.Notice, "nRet:" + nRet + ",nRetStr:" + nRetStr + ",nRetNum:" + nRetNum, mCharacter, ChatPacket.MessageMode.ToPlayer);
            #endif
        }
        public void Stop()
        {
            mCharacter.NpcSession = null;
            compiledScript = null;
            ewh = null;
            thread?.Abort();
            thread = null;
            nRet = 0;
            nRetStr = "";
            nRetNum = 0;
        }

        public void RespondPrevious()
        {
            if (sayIndex == 0 || sayLines.Count == 0) return;
            sayIndex--;
            if (sayLines.Count < sayIndex) return;
            
            string line = sayLines[sayIndex];
            bool hasBack = sayIndex > 0;
            NpcPacket.SendNPCChatTextSimple(mCharacter, mNpcID, line, hasBack, true);
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

                string line = sayLines[sayIndex];
                NpcPacket.SendNPCChatTextSimple(mCharacter, mNpcID, line, true, true);
            }
        }
        public void Say(string message)
        {
            sayLines.Add(message);
            sayIndex = sayLines.Count - 1;
            NpcPacket.SendNPCChatTextSimple(mCharacter, mNpcID, message, sayIndex > 0, true);
            ewh.WaitOne();
        }

        public int AskMenu(string Message)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextMenu(mCharacter, mNpcID, Message);
            ewh.WaitOne();
            return nRet;
        }

        public int AskYesNo(string Message)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextYesNo(mCharacter, mNpcID, Message);
            ewh.WaitOne();
            return nRet;
        }

        public string AskText(string Message, string Default, short MinLength, short MaxLength)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextRequestText(mCharacter, mNpcID, Message, Default, MinLength, MaxLength);
            ewh.WaitOne();
            return nRetStr;
        }

        public int AskNumber(string Message, int Default, int MinValue, int MaxValue)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextRequestInteger(mCharacter, mNpcID, Message, Default, MinValue, MaxValue);
            ewh.WaitOne();
            return nRetNum;
        }

        public int MakeRandAvatar(int coupon, params int[] values)
        {
            var r = Rand32.NextBetween(0, values.Length - 1);
            var state = mCharacter.ChangeAvatar(coupon, values[r]);
            return (int)state;
        }
        private int[] avatars;
        private int coupon;
        public int AskAvatar(string message, int coupon, params int[] values)
        {
            avatars = values;
            this.coupon = coupon;
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextRequestStyle(mCharacter, mNpcID, message, values.ToList());
            ewh.WaitOne();
            return (int)nAvatarSelectState;
        }
        public void RespondAvatar(byte nRet)
        {
            nAvatarSelectState = mCharacter.ChangeAvatar(coupon, avatars[nRet]);
            HandleResponse(nRet, "", 0);
        }
        public string AskPet(string message)
        {
            sayLines.Clear();
            sayIndex = 0;
            if (string.IsNullOrWhiteSpace(message)) // Check if have any pets
            {
                return mCharacter.Inventory.GetPets().FirstOrDefault()?.CashId.ToString() ?? "";
            }
            else
            {
                NpcPacket.SendNPCChatTextRequestPet(mCharacter, mNpcID, message);
                ewh.WaitOne();
                return nRetStr;
            }
        }

        public string AskPetAllExcept(string message, string petid)
        {
            sayLines.Clear();
            sayIndex = 0;

            int petskip = -1;
            try
            {
                petskip = int.Parse(petid);
            }
            catch { }
            NpcPacket.SendNPCChatTextRequestPet(mCharacter, mNpcID, message, petskip);
            ewh.WaitOne();
            return nRetStr;
        }
        public void AskShop(params ShopItemData[] items)
        {
            AskShop(items.ToList());
        }
        public void AskShop(IEnumerable<ShopItemData> items)
        {
            DataProvider.NPCs[mNpcID].Shop = items.ToList(); // MapPacket takes over handling once shop is loaded. Reload shop with /reloadscript
            mCharacter.ShopNPCID = mNpcID;
            NpcPacket.SendShowNPCShop(mCharacter, mNpcID);
        }

        public object GetStrReg(string pName)
        {
            if (scriptVars.ContainsKey(pName)) return scriptVars[pName];
            return null;
        }

        public void SetStrReg(string pName, object pValue)
        {
            if (!scriptVars.ContainsKey(pName))
                scriptVars.Add(pName, pValue);
            else
                scriptVars[pName] = pValue;
        }

        public void Log(string text)
        {
            Program.MainForm.LogAppend(text);
        }
    }
}
