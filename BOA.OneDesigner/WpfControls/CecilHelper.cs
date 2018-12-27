using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace BOA.OneDesigner.WpfControls
{
    static class CecilHelper
    {

        static readonly List<string> PrimitiveTypes = new List<string>
        {
            typeof(string).FullName,
            typeof(int).FullName,
            typeof(int?).FullName,
            typeof(short).FullName,
            typeof(short?).FullName,
            typeof(byte).FullName,
            typeof(byte?).FullName,
            typeof(long).FullName,
            typeof(long).FullName,
            typeof(decimal).FullName,
            typeof(decimal?).FullName
        };

        public static IReadOnlyList<string> GetAllBindProperties(string assemblyPath,string typeFullName)
        {
            
            var typeDefinitions = new List<TypeDefinition>();

            VisitAllTypes(assemblyPath, (type) =>
            {
                if (type.FullName == typeFullName)
                {
                    typeDefinitions.Add(type);
                }
            });

            var items  = new List<string>();

            if (typeDefinitions.Count>0)
            {
                var typeDefinition = typeDefinitions[0];

                foreach (var propertyDefinition in typeDefinition.Properties)
                {
                    if ( PrimitiveTypes.Contains(propertyDefinition.PropertyType.FullName) )
                    {
                        items.Add(propertyDefinition.Name);
                    }
                }
            }

            return items;


        }

        #region Public Methods
        public static IReadOnlyList<string> GetAllRequestNames(string assemblyPath)
        {
            return GetAllTypeNames(assemblyPath).Where(t => t.EndsWith("Request")).ToList();
        }

        public static IReadOnlyList<string> GetAllTypeNames(string assemblyPath)
        {
            var items = new List<string>();

            VisitAllTypes(assemblyPath, (type) =>
            {
                items.Add(type.FullName);
            });
            

            return items;
        }


         static void VisitAllTypes(string assemblyPath, Action<TypeDefinition> action)
        {
            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath);


            foreach (var moduleDefinition in assemblyDefinition.Modules)
            {
                foreach (var type in moduleDefinition.Types)
                {
                    if (type.Name.Contains("<"))
                    {
                        continue;
                    }

                    action(type);
                }
            }

        }

        #endregion
    }
}