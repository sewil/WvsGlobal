using System;

namespace WvsBeta.Game.Scripting
{
    public class PortalScriptSession : IPortalHost
    {
        private IPortalScript _compiledScript = null;

        private GameCharacter mCharacter { get; set; }

        public PortalScriptSession(GameCharacter chr, IPortalScript script)
        {
            mCharacter = chr;
            _compiledScript = script;
        }

        public static void Run(string script, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            Run((IPortalScript)ScriptAccessor.GetScript(Server.Instance, script, errorHandlerFnc), chr);
        }

        public static void Run(IPortalScript script, GameCharacter chr)
        {
            if (script == null) return;

            var session = new PortalScriptSession(chr, script);
            session.RunScript();
        }

        public void RunScript()
        {
            _compiledScript.Run(this, mCharacter);
        }
    }
}
