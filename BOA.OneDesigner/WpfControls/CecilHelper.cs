using System.Collections.Generic;
using Mono.Cecil;

namespace BOA.OneDesigner.WpfControls
{
    static class CecilHelper
    {

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
    }
}