using log4net;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WvsBeta.Game.Scripting
{
    internal class ScriptAccessor
    {
        public static IGameScript GetScript(NPCData npc, Action<string> errorHandlerFnc)
        {
            INpcScript npcScript = null;
            if (npcScript == null && npc.Quest != null) npcScript = (INpcScript)GetScript(Server.Instance, npc.Quest, errorHandlerFnc);
            if (npcScript == null) npcScript = (INpcScript)GetScript(Server.Instance, npc.ID.ToString(), errorHandlerFnc);
            return npcScript;
        }
        public static IGameScript GetScript(Server server, string scriptName, Action<string> errorHandlerFnc, bool forceRecompile = false)
        {
            if (!forceRecompile && server.AvailableScripts.TryGetValue(scriptName, out IGameScript instance)) return instance;
            var scriptUri = GetScriptFileName(scriptName);
            if (scriptUri == null && !forceRecompile)
            {
                errorHandlerFnc?.Invoke(scriptName + " not found");
                return null;
            }
            var dll = ScriptCompiler.CompileScript(scriptUri, errorHandlerFnc, out string savename);
            if (dll == null) return null;
            instance = GetScriptInstance(dll, scriptName);
            server.AvailableScripts[scriptName] = instance;
            return instance;
        }
        public static IGameScript GetScriptInstance(Assembly dll, string scriptName)
        {
            // Loop through types looking for one that implements the display name or interface as fallback
            IGameScript fallback = null;
            foreach (Type t in dll.GetTypes())
            {
                var displayName = t.GetCustomAttribute(typeof(DisplayNameAttribute), true) as DisplayNameAttribute;
                if (displayName != null && displayName.DisplayName == scriptName)
                {
                    return (IGameScript)dll.CreateInstance(t.FullName);
                }
                if (t.GetInterface("IGameScript", true) != null)
                {
                    fallback = (IGameScript)dll.CreateInstance(t.FullName);
                }
            }
            return fallback;
        }
        public static string GetScriptFileName(string scriptName)
        {
            var scriptsDir = Path.Combine(Environment.CurrentDirectory, "..", "WvsBeta.Scripts", "Scripts");

            string filename = Path.Combine(scriptsDir, scriptName + ".cs");
            if (!File.Exists(filename))
            {
                var m = Regex.Match(scriptName, @"\d+$");
                if (m.Success) // Strip script digits
                {
                    filename = Path.Combine(scriptsDir, scriptName.Substring(0, m.Index) + ".cs");
                }
            }
            if (!File.Exists(filename)) return null;
            return filename;
        }
    }
}
