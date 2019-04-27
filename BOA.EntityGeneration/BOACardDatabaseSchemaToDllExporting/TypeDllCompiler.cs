using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    /// <summary>
    ///     The compiler
    /// </summary>
    public class TypeDllCompiler
    {
        const string outputDirectory = @"d:\boa\server\bin\";

        #region Public Methods
        /// <summary>
        ///     Compiles given source code
        /// </summary>
        public void Compile(string schemaName,string sources)
        {
            const string SYSTEM_CORE = "System.Core.dll";
            const string SYSTEM_DATA = "System.Data.dll";
            const string MS_CORE_LIB = "mscorlib.dll";
            const string SYSTEM      = "System.dll";

            var referencedAssemblies = new List<string>
            {
                MS_CORE_LIB,
                SYSTEM,
                SYSTEM_CORE,
                SYSTEM_DATA,
                @"d:\boa\server\bin\BOA.Common.dll"
            };

           

            var fileNameWithoutExtension = $"BOA.Types.Kernel.Card.{schemaName}";

            var          OPTIONS  = $"/target:library /optimize /doc:{outputDirectory}{fileNameWithoutExtension}.xml";
            const string LANGUAGE = "CSharp";
            var          compiler = CodeDomProvider.CreateProvider(LANGUAGE);
            var compilerParams = new CompilerParameters(referencedAssemblies.Distinct().ToArray())
            {
                CompilerOptions         = OPTIONS,
                GenerateExecutable      = false,
                IncludeDebugInformation = true,
                OutputAssembly          = $"{outputDirectory}{fileNameWithoutExtension}.dll"
            };

            var results = compiler.CompileAssemblyFromSource(compilerParams, sources);

            var errors = ConvertToList(results.Errors);

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }

            

            // CopyToTfs(fileNameWithoutExtension);
        }

        static void CopyToTfs(string fileNameWithoutExtension)
        {
            
            var targetDirectory = $@"D:\work\BOA.ExternalLibraries\ExternalLibraries\AnyCPU\{fileNameWithoutExtension}\0.0.0.0\";

            Directory.CreateDirectory(targetDirectory);

            File.Copy($@"{outputDirectory}{fileNameWithoutExtension}.dll", $@"{targetDirectory}{fileNameWithoutExtension}.dll", true);
            File.Copy($@"{outputDirectory}{fileNameWithoutExtension}.pdb", $@"{targetDirectory}{fileNameWithoutExtension}.pdb", true);
            File.Copy($@"{outputDirectory}{fileNameWithoutExtension}.xml", $@"{targetDirectory}{fileNameWithoutExtension}.xml", true);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Converts to list.
        /// </summary>
        static IReadOnlyCollection<string> ConvertToList(CompilerErrorCollection compilerErrorCollection)
        {
            var errors = new List<string>();
            foreach (CompilerError compilerError in compilerErrorCollection)
            {
                errors.Add(compilerError.ToString());
            }

            return errors;
        }
        #endregion
    }
}