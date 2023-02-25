using System;

namespace WvsBeta.Game.Scripting
{
    public class PortalScriptSession : IPortalHost
    {
        private IPortalScript _compiledScript = null;

        private GameCharacter mCharacter { get; set; }

        public PortalScriptSession(GameCharacter chr)
        {
            mCharacter = chr;
        }

        public static void Run(string script, GameCharacter chr, Action<string> errorHandlerFnc)
        {
            Run((IPortalScript)ScriptAccessor.GetScript(Server.Instance, script, errorHandlerFnc), chr);
        }

        public static void Run(IPortalScript portal, GameCharacter chr)
        {
            if (portal == null) return;

            var session = new PortalScriptSession(chr);
            session.SetScript((IPortalScript)portal.GetType().GetMethod("MemberwiseClone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(portal, null));
            session.HandleThing();
        }

        public IPortalScript CompiledScript
        {
            get { return _compiledScript; }
        }

        public void SetScript(IPortalScript script)
        {
            _compiledScript = script;
        }

        public void HandleThing(byte state = 0, byte action = 0, string text = "", int integer = 0)
        {
            _compiledScript.Run(this, mCharacter);
        }
    }
}
