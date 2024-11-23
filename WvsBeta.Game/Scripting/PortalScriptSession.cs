using System;

namespace WvsBeta.Game.Scripting
{
    public class PortalScriptSession : IPortalHost
    {
        private IPortalScript _compiledScript = null;

        private GameCharacter mCharacter { get; set; }

        public Map Field { get; }

        public int GetPortalID { get; }

        private string scriptName;

        public PortalScriptSession(GameCharacter chr, Portal portal, IPortalScript script)
        {
            Field = chr.Field;
            mCharacter = chr;
            _compiledScript = script;
            scriptName = portal.Script;
            GetPortalID = portal.ID;
        }

        public static void Run(Portal portal, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            if (portal == null || portal.Script == null) return;
            IPortalScript script = (IPortalScript)ScriptAccessor.GetScript(Server.Instance, portal.Script, errorHandlerFnc);
            if (script == null) return;

            var session = new PortalScriptSession(chr, portal, script);
            session.RunScript();
        }

        public void RunScript()
        {
            try
            {
#if DEBUG
                mCharacter.Notice("Run portal script " + scriptName);
#endif
                _compiledScript.Run(this, mCharacter);
            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend("Script run exception @ PortalScriptSession::RunScript : {0}", ex.ToString());
            }
        }
    }
}
