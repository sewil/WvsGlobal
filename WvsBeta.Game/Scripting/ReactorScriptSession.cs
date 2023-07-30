using System;
using System.Reflection;

namespace WvsBeta.Game.Scripting
{
    public class ReactorScriptSession : IReactorHost
    {
        public IReactorScript Script { private get; set; }

        private FieldReactor reactor;

        private ReactorScriptSession(FieldReactor reactor)
        {
            this.reactor = reactor;
        }

        public static void Run(FieldReactor reactor, Action<string> errorHandlerFnc, string scriptName = null)
        {
            if (scriptName == null) scriptName = reactor.Reactor.Action ?? reactor.Reactor.ID.ToString();
            var script = (IReactorScript)ScriptAccessor.GetScript(Server.Instance, scriptName, errorHandlerFnc);
            if (script == null) return;

            var session = new ReactorScriptSession(reactor);
            session.Script = (IReactorScript)script.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(script, null);
            session.RunScript();
        }

        public void RunScript()
        {
            Script.Run(this, reactor);
        }
    }
}
