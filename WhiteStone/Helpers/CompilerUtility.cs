using System.CodeDom.Compiler;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Helper methods for compile operations
    /// </summary>
    public static class CompilerUtility
    {
        /// <summary>
        ///     Compiles c# codes to .Net byte code.
        /// </summary>
        /// <param name="outputAssembly"></param>
        /// <param name="referencedAssemblies"></param>
        /// <param name="sources"></param>
        /// <returns></returns>
        public static CompilerResults CompileCSharpCodesToAssembly(string outputAssembly,
            string[] referencedAssemblies,
            string[] sources)
        {
            var compiler = CodeDomProvider.CreateProvider("CSharp");

            var compilerParams = new CompilerParameters
            {
                CompilerOptions = "/target:library /optimize",
                GenerateExecutable = false,
                OutputAssembly = outputAssembly,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };

            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParams.ReferencedAssemblies.Add("System.dll");

            if (referencedAssemblies != null)
            {
                foreach (var item in referencedAssemblies)
                {
                    compilerParams.ReferencedAssemblies.Add(item);
                }
            }

            return compiler.CompileAssemblyFromSource(compilerParams, sources);
        }
    }
}