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

        public static void Run(FieldReactor reactor, Action<string> errorHandlerFnc)
        {
            IReactorScript script = GetScript(reactor, errorHandlerFnc);
            if (script == null) return;

            var session = new ReactorScriptSession(reactor);
            session.Script = (IReactorScript)script.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(script, null);
            session.RunScript();
        }
        public static IReactorScript GetScript(FieldReactor reactor, Action<string> errorHandlerFnc)
        {
            IReactorScript script = null;
            if (reactor.Reactor.Action != null)
            {
                script = (IReactorScript)ScriptAccessor.GetScript(Server.Instance, reactor.Reactor.Action, errorHandlerFnc);
            }
            return script;
        }

        public void RunScript()
        {
            Script.Run(this, reactor);
        }
    }
}
