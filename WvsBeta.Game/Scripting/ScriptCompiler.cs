using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace WvsBeta.Game.Scripting
{
    public class ScriptCompiler
    {
        private static CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");

        public static IGameScript CompileScript(string scriptUri, Action<string> errorHandlerFnc, out string savename)
        {
            var results = Compile(scriptUri);
            var fi = new FileInfo(scriptUri);
            savename = fi.Name.Replace(".s", "").Replace(".cs", "");
            if (results.Errors.Count > 0)
            {
                errorHandlerFnc?.Invoke(Path.GetFileName(scriptUri));

                Program.MainForm.LogAppend($"Couldn't compile the file ({scriptUri}) correctly:");
                foreach (CompilerError error in results.Errors)
                {
                    Program.MainForm.LogAppend(
                        $"File {scriptUri}, Line {error.Line}, Column {error.Column}: {error.ErrorText}");
                }
                return null;
            }
            return FindInterface(results.CompiledAssembly, "IGameScript");
        }
        public static CompilerResults Compile(string source)
        {
            CompilerParameters parms = new CompilerParameters()
            {
                // Configure parameters
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };

            var mainPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (var r in Assembly.GetExecutingAssembly().GetReferencedAssemblies())
            {
                if (File.Exists(Path.Combine(mainPath, r.Name + ".dll")))
                    parms.ReferencedAssemblies.Add(Path.Combine(mainPath, r.Name + ".dll"));
                else
                    parms.ReferencedAssemblies.Add(r.Name + ".dll");
            }
            parms.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

            return compiler.CompileAssemblyFromFile(parms, source);
        }

        public static IGameScript FindInterface(Assembly dll, string interfaceName)
        {
            // Loop through types looking for one that implements the given interface
            foreach (Type t in dll.GetTypes())
            {
                if (t.GetInterface(interfaceName, true) != null)
                    return (IGameScript)dll.CreateInstance(t.FullName);
            }

            return null;
        }
    }
}