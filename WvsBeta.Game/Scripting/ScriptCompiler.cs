using System;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;

namespace WvsBeta.Game.Scripting
{
    public class ScriptCompiler
    {
        private static CodeDomProvider compiler = new CSharpCodeProvider();

        public static Assembly CompileScript(string scriptPath, Action<string> errorHandlerFnc)
        {
            var results = CompileFile(scriptPath);
            var fi = new FileInfo(scriptPath);
            if (results.Errors.Count > 0)
            {
                errorHandlerFnc?.Invoke(Path.GetFileName(scriptPath));

                Program.MainForm.LogAppend($"Couldn't compile the file ({scriptPath}) correctly:");
                foreach (CompilerError error in results.Errors)
                {
                    Program.MainForm.LogAppend(
                        $"File {scriptPath}, Line {error.Line}, Column {error.Column}: {error.ErrorText}");
                }
                return null;
            }
            return results.CompiledAssembly;
        }
        public static CompilerResults CompileFile(string filePath)
        {
            CompilerParameters parms = new CompilerParameters()
            {
                // Configure parameters
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };

            var exc = Assembly.GetExecutingAssembly();
            var mainPath = Path.GetDirectoryName(exc.Location);

            foreach (var r in exc.GetReferencedAssemblies())
            {
                if (File.Exists(Path.Combine(mainPath, r.Name + ".dll")))
                    parms.ReferencedAssemblies.Add(Path.Combine(mainPath, r.Name + ".dll"));
                else
                    parms.ReferencedAssemblies.Add(r.Name + ".dll");
            }
            parms.ReferencedAssemblies.Add(Path.Combine(mainPath, "Microsoft.CSharp.dll"));
            parms.ReferencedAssemblies.Add(exc.Location);

            return compiler.CompileAssemblyFromFile(parms, filePath);
        }
    }
}