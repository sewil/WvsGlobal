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
                string err = $"Script file for script '{scriptName}' not found.";
                errorHandlerFnc?.Invoke(err);
                Program.MainForm.LogAppend(err);
                return null;
            }
            var dll = ScriptCompiler.CompileScript(scriptPath, errorHandlerFnc);
            if (dll == null) return null;

            instance = GetScriptInstance(server, dll, scriptName, errorHandlerFnc);
            return instance;
        }
        public static IGameScript GetScriptInstance(Server server, Assembly dll, string scriptName, Action<string> errorHandlerFnc)
        {
            var instance = CreateScriptInstance(dll, scriptName, errorHandlerFnc);
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
                    var scriptInstance = (IGameScript)dll.CreateInstance(t.FullName);
                    return (IGameScript)scriptInstance.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(scriptInstance, null);
                }
            }
            string err = string.Format("Script implementation for '{0}' not found.", scriptName);
            Program.MainForm.LogAppend(err);
            errorHandlerFnc?.Invoke(err);
            return null;
        }
        public static IList<string> GetScriptNames(Assembly dll)
        {
            var scriptNames = new List<string>();
            foreach (Type t in dll.GetTypes())
            {
                ScriptAttribute attr = t.GetCustomAttribute(typeof(ScriptAttribute), true) as ScriptAttribute;
                if (attr != null)
                {
                    scriptNames.Add(attr.DisplayName);
                }
            }
            return scriptNames;
        }
        public static string GetScriptPath(string scriptName)
        {
            var scriptsDir = Path.Combine(Environment.CurrentDirectory, "Scripts");

            string scriptPath = Path.Combine(scriptsDir, scriptName + ".cs");
            if (!File.Exists(scriptPath) && scriptName != "standard")
            {
                if (ScriptNameMap.TryGetValue(scriptName, out string scriptFileName))
                {
                    scriptPath = Path.Combine(scriptsDir, scriptFileName);
                }
            }
            if (!File.Exists(scriptPath)) return null;
            return scriptPath;
        }
        public static void PrecompileAllScripts()
        {
            var scriptFiles = Directory.GetFiles("Scripts");
            ScriptNameMap = new Dictionary<string, string>();
            foreach (var scriptFile in scriptFiles)
            {
                var scriptPath = Path.GetFullPath(scriptFile);
                var scriptFileName = Path.GetFileName(scriptFile);
                Program.MainForm.LogAppend($"Precompiling script file '{scriptFileName}'...");
                var dll = ScriptCompiler.CompileScript(scriptPath, null);
                if (dll == null) continue;
                var scriptNames = GetScriptNames(dll);
                foreach (var scriptName in scriptNames)
                {
                    ScriptNameMap[scriptName] = scriptFileName;
                    Program.MainForm.LogAppend($"Loading script '{scriptName}'...");
                    GetScriptInstance(Server.Instance, dll, scriptName, null);
                }
            }
        }
        /// <summary>
        /// Keys are script names, values are script file names.
        /// </summary>
        public static IDictionary<string, string> ScriptNameMap { get; private set; }
    }
}
