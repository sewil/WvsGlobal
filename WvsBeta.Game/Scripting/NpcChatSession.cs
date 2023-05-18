using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WvsBeta.Common;
using WvsBeta.Common.Objects;
using WvsBeta.Game.Scripting;
using ThreadState = System.Threading.ThreadState;

namespace WvsBeta.Game
{
    public interface INpcScript : IGameScript
    {
        void Run(INpcHost self, GameCharacter target);
    }

    public interface INpcHost
    {
        IStandardScript Standard { get; }
        Map Field { get; }
        int mNpcID { get; }
        void Say(string message);
        int AskYesNo(string Message);
        int AskMenu(string Message);
        int AskMenu(string message, params string[] options);
        int AskNumber(string Message, int Default, int MinValue, int MaxValue);
        /// <summary>
        /// Have pet doll.
        /// </summary>
        /// <param name="skip">Pet doll to skip</param>
        bool HavePetDoll(long skip = -1);
        /// <summary>
        /// Pet doll selector.
        /// </summary>
        /// <param name="message">Selector text</param>
        /// <param name="skip">Pet doll to skip</param>
        long AskPetDoll(string message, long skip = -1);
        string AskText(string Message, string Default, short MinLength, short MaxLength);
        int AskAvatar(string message, int coupon, params int[] values);
        int MakeRandAvatar(int coupon, params int[] values);
        void AskShop(IEnumerable<ShopItemData> items);
        void AskShop(params ShopItemData[] items);
        object GetStrReg(string pName);
        void SetStrReg(string pName, object pValue);
        int GetIntReg(string name);
        void SetIntReg(string name, int value);
        int IncIntReg(string name, int value);
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
        public IStandardScript Standard { get; }
        public Map Field { get; }
        public int mNpcID { get; set; }
        public GameCharacter mCharacter { get; set; }
        private INpcScript compiledScript = null;
        public bool Started { get; private set; }

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
        public NpcChatSession(NpcLife npc, GameCharacter chr, INpcScript npcScript, string scriptName)
        {
            Standard = ScriptAccessor.GetScript(Server.Instance, "standard", null) as IStandardScript;
            Field = npc.Field;
            mNpcID = npc.ID;
            mCharacter = chr;
            mCharacter.NpcSession = this;
            scriptVars = Server.Instance.ScriptVars[scriptName];
            compiledScript = npcScript;
            if (chr.IsGM && chr.Job == 500) SetStrReg("name", chr.Name);
            ewh = new EventWaitHandle(false, EventResetMode.AutoReset);
            Started = true;
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

        private static INpcScript GetScript(NPCData npcData, Action<string> errorHandlerFnc, out string scriptName)
        {
            INpcScript script = null;
            scriptName = null;
            if (npcData.Quest != null)
            {
                scriptName = npcData.Quest;
                script = (INpcScript)ScriptAccessor.GetScript(Server.Instance, scriptName, errorHandlerFnc);
            }
            if (script == null)
            {
                scriptName = npcData.ID.ToString();
                script = (INpcScript)ScriptAccessor.GetScript(Server.Instance, scriptName, errorHandlerFnc);
            }
            if (script == null) scriptName = null;
            return script;
        }
        
        public static void Start(NpcLife npcLife, NPCData npcData, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            INpcScript script = GetScript(npcData, errorHandlerFnc, out string scriptName);
            if (script == null || chr.NpcSession != null) return;

            new NpcChatSession(npcLife, chr, script, scriptName);
        }

        public void HandleResponse(byte nRet = 0, string nRetStr = "", int nRetNum = 0)
        {
            this.nRet = nRet;
            this.nRetStr = nRetStr;
            this.nRetNum = nRetNum;
            ewh.Set();
#if DEBUG
            Trace.WriteLine("nRet:" + nRet + ",nRetStr:" + nRetStr + ",nRetNum:" + nRetNum);
#endif
        }
        public void SoftStop()
        {
            Started = false;
        }
        public void Stop()
        {
            SoftStop();
            mCharacter.NpcSession = null;
            compiledScript = null;
            ewh = null;
            thread?.Abort();
            thread = null;
            nRet = 0;
            nRetStr = "";
            nRetNum = 0;
        }
        private void Wait()
        {
            if (Started) ewh.WaitOne();
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
            bool showBack = sayIndex > 0;
            bool showNext = Started;
            NpcPacket.SendNPCChatTextSimple(mCharacter, mNpcID, message, showBack, showNext);
            Wait();
        }

        public int AskMenu(string Message)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextMenu(mCharacter, mNpcID, Message);
            Wait();
            return nRet;
        }
        public int AskMenu(string message, params string[] options)
        {
            string menu = "";
            for (int i = 0; i < options.Length; i++)
            {
                menu += $"\r\n#b#L{i}#{options[i]}#l#k";
            }
            return AskMenu(message + menu);
        }
        public int AskYesNo(string Message)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextYesNo(mCharacter, mNpcID, Message);
            Wait();
            return nRet;
        }

        public string AskText(string Message, string Default, short MinLength, short MaxLength)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextRequestText(mCharacter, mNpcID, Message, Default, MinLength, MaxLength);
            Wait();
            return nRetStr;
        }

        public int AskNumber(string Message, int Default, int MinValue, int MaxValue)
        {
            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextRequestInteger(mCharacter, mNpcID, Message, Default, MinValue, MaxValue);
            Wait();
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
            Wait();
            return (int)nAvatarSelectState;
        }
        public void RespondAvatar(byte nRet)
        {
            nAvatarSelectState = mCharacter.ChangeAvatar(coupon, avatars[nRet]);
            HandleResponse(nRet, "", 0);
        }

        public bool HavePetDoll(long skip = -1)
        {
            return mCharacter.Inventory.GetPets().Where(i => i.DeadDate == Item.NoItemExpiration && (skip == -1 || i.CashId != skip)).Count() > 0;
        }

        public long AskPetDoll(string message, long skip = -1)
        {
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentNullException();

            sayLines.Clear();
            sayIndex = 0;
            NpcPacket.SendNPCChatTextRequestPet(mCharacter, mNpcID, message, skip);
            Wait();
            if (long.TryParse(nRetStr, out long petCashID)) return petCashID;
            else return 0;
        }

        public void AskShop(params ShopItemData[] items)
        {
            AskShop(items.ToList());
        }
        public void AskShop(IEnumerable<ShopItemData> items)
        {
            GameDataProvider.NPCs[mNpcID].Shop = items.ToList(); // MapPacket takes over handling once shop is loaded. Reload shop with /reloadscript
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

        public int GetIntReg(string name)
        {
            object obj = GetStrReg(name);
            if (obj == null || !(obj is int value)) return -1;
            else return value;
        }

        public void SetIntReg(string name, int value)
        {
            SetStrReg(name, value);
        }
        public int IncIntReg(string name, int value)
        {
            int newValue = GetIntReg(name) + value;
            SetIntReg(name, newValue);
            return newValue;
        }
        public void Log(string text)
        {
            Program.MainForm.LogAppend(text);
        }
    }
}
