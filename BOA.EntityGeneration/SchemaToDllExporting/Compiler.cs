﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BOA.EntityGeneration.SchemaToDllExporting
{
    public class CompilerData
    {
        #region Public Properties
        public string                OutputAssemblyName   { get; set; }
        public IReadOnlyList<string> ReferencedAssemblies { get; set; }
        public string[]              Sources              { get; set; }
        #endregion
    }
    /// <summary>
    ///     The compiler
    /// </summary>
    public class Compiler
    {
       

        #region Public Methods
        /// <summary>
        ///     Compiles given source code
        /// </summary>
        public void Compile(CompilerData data)
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
                SYSTEM_DATA
            };

            referencedAssemblies.AddRange(data.ReferencedAssemblies);

            var fileNameWithoutExtension = $"BOA.Types.Kernel.Card.{data.OutputAssemblyName}";

            const string OPTIONS  = "/target:library /optimize";
            const string LANGUAGE = "CSharp";
            var          compiler = CodeDomProvider.CreateProvider(LANGUAGE);
            var compilerParams = new CompilerParameters(referencedAssemblies.Distinct().ToArray())
            {
                CompilerOptions         = OPTIONS,
                GenerateExecutable      = false,
                IncludeDebugInformation = true,
                OutputAssembly          = $@"d:\boa\server\bin\{fileNameWithoutExtension}.dll"
            };

            var results = compiler.CompileAssemblyFromSource(compilerParams, data.Sources);

            var errors = ConvertToList(results.Errors);

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }

            var externalLibPath = $@"D:\work\BOA.ExternalLibraries\ExternalLibraries\AnyCPU\{fileNameWithoutExtension}\0.0.0.0\{fileNameWithoutExtension}.dll";

            Directory.CreateDirectory($@"D:\work\BOA.ExternalLibraries\ExternalLibraries\AnyCPU\{fileNameWithoutExtension}\0.0.0.0\");

            File.Copy($@"d:\boa\server\bin\{fileNameWithoutExtension}.dll",externalLibPath,true);


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