using System;
using System.Collections.Generic;
using System.IO;
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
            typeof(decimal?).FullName,
            typeof(DateTime).FullName,
            typeof(DateTime?).FullName
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


                GetAllBindProperties("request.",items,typeDefinition);
            }

            return items;


        }

        static void GetAllBindProperties(string pathPrefix, List<string> paths, TypeDefinition typeDefinition)
        {
            if (typeDefinition == null)
            {
                return;
            }
            foreach (var propertyDefinition in typeDefinition.Properties)
            {
                if (propertyDefinition.GetMethod == null || propertyDefinition.SetMethod == null)
                {
                    continue;
                }

                if ( PrimitiveTypes.Contains(propertyDefinition.PropertyType.FullName) )
                {
                    paths.Add(pathPrefix+propertyDefinition.Name);
                    continue;
                }

                if (propertyDefinition.PropertyType.IsValueType == false)
                {
                    GetAllBindProperties(pathPrefix + propertyDefinition.Name + ".", paths, propertyDefinition.PropertyType.Resolve());
                }
            }
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

            if (File.Exists(assemblyPath) == false)
            {
                return;
            }

            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(@"d:\boa\server\bin\");

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath,new ReaderParameters {AssemblyResolver = resolver});
            

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