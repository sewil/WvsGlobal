using System;

namespace WvsBeta.Game.Scripting
{
    public interface IFieldSetScript : IGameScript
    {
        void Run(IFieldSetScriptHost host, GameCharacter target);
    }
    public interface IFieldSetScriptHost
    {
        FieldSet FieldSet { get; }
    }
    public class FieldSetScriptHost : IFieldSetScriptHost
    {
        public FieldSet FieldSet { get; }
        private readonly IFieldSetScript script;
        private readonly GameCharacter target;
        private readonly string scriptName;
        public FieldSetScriptHost(FieldSet fieldSet, IFieldSetScript script, GameCharacter target, string scriptName)
        {
            FieldSet = fieldSet;
            this.script = script;
            this.target = target;
            this.scriptName = scriptName;
        }
        public static void RunScript(FieldSet fieldSet, GameCharacter target, string scriptName, Action<string> errorHandler)
        {
            var script = (IFieldSetScript)ScriptAccessor.GetScript(Server.Instance, scriptName, errorHandler);
            if (script == null) return;
            var host = new FieldSetScriptHost(fieldSet, script, target, scriptName);
            host.RunScript();
        }

        public void RunScript()
        {
            try
            {
#if DEBUG
                target.Notice("Run fieldset script " + scriptName);
#endif
                script.Run(this, target);

            }
            catch (Exception ex)
            {
                Program.MainForm.LogAppend("Script run exception @ FieldSetScriptHost::RunScript : {0}", ex.ToString());
            }
        }
    }
}
