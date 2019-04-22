﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace BOA.CodeGeneration.Contracts.Transforms
{
    /// <summary>
    ///     The compiler
    /// </summary>
    public class Compiler
    {
        #region Public Properties
        public string   OutputAssemblyName { get; set; }
        public string[] Sources            { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Compiles given source code
        /// </summary>
        public void Compile()
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
                @"d:\boa\server\bin\BOA.Process.Kernel.Card.dll",
                @"d:\boa\server\bin\BOA.Common.dll"
            };

            const string OPTIONS  = "/target:library /optimize";
            const string LANGUAGE = "CSharp";
            var          compiler = CodeDomProvider.CreateProvider(LANGUAGE);
            var compilerParams = new CompilerParameters(referencedAssemblies.Distinct().ToArray())
            {
                CompilerOptions         = OPTIONS,
                GenerateExecutable      = false,
                IncludeDebugInformation = false,
                OutputAssembly          = $@"d:\boa\server\bin\BOA.Types.Kernel.Card.{OutputAssemblyName}.dll"
            };

            var results = compiler.CompileAssemblyFromSource(compilerParams, Sources);

            var errors = ConvertToList(results.Errors);

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }
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