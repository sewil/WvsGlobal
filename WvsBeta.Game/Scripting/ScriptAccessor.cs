using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WvsBeta.Game.Scripting
{
    internal class ScriptAccessor
    {
        public static IGameScript GetScript(Server server, string scriptName, Action<string> errorHandlerFnc, bool forceRecompile = false)
        {
            if (!forceRecompile && server.AvailableScripts.TryGetValue(scriptName, out IGameScript instance)) return instance;
            string scriptPath = GetScriptPath(scriptName);
            if (scriptPath == null && !forceRecompile)
            {
                errorHandlerFnc?.Invoke(scriptName + " not found.");
                return null;
            }
            var dll = ScriptCompiler.CompileScript(scriptPath, errorHandlerFnc);
            if (dll == null) return null;

            instance = CreateScriptInstance(dll, scriptName, errorHandlerFnc);
            if (instance == null) return null;

            server.AvailableScripts[scriptName] = instance;
            server.ScriptVars[scriptName] = new Dictionary<string, object>();
            return instance;
        }
        public static IGameScript CreateScriptInstance(Assembly dll, string scriptName, Action<string> errorHandlerFnc)
        {
            foreach (Type t in dll.GetTypes())
            {
                ScriptAttribute attr = t.GetCustomAttribute(typeof(ScriptAttribute), true) as ScriptAttribute;
                if (attr != null && attr.DisplayName == scriptName)
                {
                    return (IGameScript)dll.CreateInstance(t.FullName);
                }
            }
            errorHandlerFnc?.Invoke(string.Format("Script implementation for '{0}' not found.", scriptName));
            return null;
        }
        public static string GetScriptPath(string scriptName)
        {
            var scriptsDir = Path.Combine(Environment.CurrentDirectory, "..", "WvsBeta.Scripts", "Scripts");

            string scriptPath = Path.Combine(scriptsDir, scriptName + ".cs");
            if (!File.Exists(scriptPath))
            {
                if (!Server.Instance.AvailableScripts.TryGetValue("standard", out IGameScript script))
                {
                    script = GetScript(Server.Instance, "standard", null);
                }
                if (((IStandardScript)script).ScriptNameMap.TryGetValue(scriptName, out string mapValue))
                {
                    scriptPath = Path.Combine(scriptsDir, mapValue + ".cs");
                }
            }
            if (!File.Exists(scriptPath)) return null;
            return scriptPath;
        }
    }
}
