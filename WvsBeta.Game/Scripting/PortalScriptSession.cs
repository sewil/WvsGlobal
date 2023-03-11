using System;

namespace WvsBeta.Game.Scripting
{
    public class PortalScriptSession : IPortalHost
    {
        private IPortalScript _compiledScript = null;

        private GameCharacter mCharacter { get; set; }
        private string scriptName;

        public PortalScriptSession(GameCharacter chr, IPortalScript script, string scriptName)
        {
            mCharacter = chr;
            _compiledScript = script;
            this.scriptName = scriptName;
        }

        public static void Run(string script, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            Run((IPortalScript)ScriptAccessor.GetScript(Server.Instance, script, errorHandlerFnc), chr, script);
        }

        public static void Run(IPortalScript script, GameCharacter chr, string scriptName)
        {
            if (script == null) return;

            var session = new PortalScriptSession(chr, script, scriptName);
            session.RunScript();
        }

        public void RunScript()
        {
#if DEBUG
            mCharacter.Notice("Run portal script " + scriptName);
#endif
            _compiledScript.Run(this, mCharacter);
        }
    }
}
