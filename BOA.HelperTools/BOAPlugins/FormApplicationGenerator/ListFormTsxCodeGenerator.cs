using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOAPlugins.FormApplicationGenerator.Types;
using BOAPlugins.Utility;
using Mono.Cecil;

namespace BOAPlugins.FormApplicationGenerator
{
    public class ListFormTsxCodeGeneratorData
    {
        #region Fields
        internal TypeDefinition TypeDefinition;
        #endregion

        #region Public Properties
        public string                    RequestClassLocation { get; set; }
        public IReadOnlyList<BField> SearchFields         { get; set; }
        #endregion
    }

    public class ListFormTsxCodeGenerator
    {
        #region Public Methods
        public static void Generate(ListFormTsxCodeGeneratorData data)
        {
            if (data.TypeDefinition == null)
            {
                var assemblyFilePath   = $@"d:\boa\server\bin\{data.RequestClassLocation.SplitAndClear("->")[0]}";
                var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyFilePath);
                if (assemblyDefinition == null)
                {
                    throw new ArgumentException("Assembly not found." + assemblyFilePath);
                }

                data.TypeDefinition = assemblyDefinition.FindType(data.RequestClassLocation.SplitAndClear("->")[1]);
                if (data.TypeDefinition == null)
                {
                    throw new ArgumentException("TypeDefinition not found.");
                }
            }
        }
        #endregion
    }
}