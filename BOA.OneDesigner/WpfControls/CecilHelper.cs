using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace BOA.OneDesigner.WpfControls
{
    static class CecilHelper
    {
        #region Public Methods
        public static IReadOnlyList<string> GetAllRequestNames(string assemblyPath)
        {
            return GetAllTypeNames(assemblyPath).Where(t => t.EndsWith("Request")).ToList();
        }

        public static IReadOnlyList<string> GetAllTypeNames(string assemblyPath)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);

            var items = new List<string>();

            foreach (var moduleDefinition in assemblyDefinition.Modules)
            {
                foreach (var type in moduleDefinition.Types)
                {
                    if (type.Name.Contains("<"))
                    {
                        continue;
                    }

                    items.Add(type.FullName);
                }
            }

            return items;
        }
        #endregion
    }
}