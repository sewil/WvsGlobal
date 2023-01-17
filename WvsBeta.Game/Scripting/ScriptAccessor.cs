using System;
using System.IO;

namespace WvsBeta.Game.Scripting
{
    internal class ScriptAccessor
    {
        public static IGameScript GetScript(Server server, string scriptName, Action<string> errorHandlerFnc, bool forceRecompile = false)
        {
            if (!forceRecompile && server.AvailableScripts.TryGetValue(scriptName, out IGameScript ret)) return ret;
            var scriptUri = GetScriptFilename(scriptName);
            if (scriptUri == null && !forceRecompile)
            {
                errorHandlerFnc?.Invoke(scriptName + " not found");
                return null;
            }
            ret = ScriptCompiler.CompileScript(scriptUri, errorHandlerFnc, out string savename);
            server.AvailableScripts[savename] = ret;
            return ret;
        }
        public static string GetScriptFilename(string scriptName)
        {
            var scriptsDir = Path.Combine(Environment.CurrentDirectory, "..", "WvsBeta.Scripts", "Scripts");

            string filename = Path.Combine(scriptsDir, scriptName + ".s");
            if (!File.Exists(filename)) filename = Path.Combine(scriptsDir, scriptName + ".cs");
            if (!File.Exists(filename)) return null;
            return filename;
        }
    }
}
