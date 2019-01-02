﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using Mono.Cecil;

namespace BOA.OneDesigner.Helpers
{
    public class RequestIntellisenseData
    {
        #region Public Properties
        public Dictionary<string, IReadOnlyList<string>> Collections                           { get; set; }
        public IReadOnlyList<string>                     RequestCollectionPropertyIntellisense { get; set; }
        public IReadOnlyList<string>                     RequestPropertyIntellisense           { get; set; }
        public IReadOnlyList<string>                     RequestStringPropertyIntellisense     { get; set; }
        #endregion
    }

    static class CecilHelper
    {
        #region Static Fields
        static readonly List<string> PrimitiveTypes = new List<string>
        {
            typeof(string).FullName,

            typeof(sbyte).FullName,
            typeof(byte).FullName,
            typeof(short).FullName,
            typeof(int).FullName,
            typeof(long).FullName,
            typeof(decimal).FullName,
            typeof(DateTime).FullName,

            "System.Nullable`1<" + typeof(sbyte).FullName + ">",
            "System.Nullable`1<" + typeof(byte).FullName + ">",
            "System.Nullable`1<" + typeof(short).FullName + ">",
            "System.Nullable`1<" + typeof(int).FullName + ">",
            "System.Nullable`1<" + typeof(long).FullName + ">",
            "System.Nullable`1<" + typeof(decimal).FullName + ">",
            "System.Nullable`1<" + typeof(DateTime).FullName + ">"
        };
        #endregion

        #region Public Methods
        public static IReadOnlyList<string> GetAllBindProperties(string assemblyPath, string typeFullName)
        {
            var items = new List<string>();

            var typeDefinition = FindType(assemblyPath, typeFullName);
            if (typeDefinition != null)
            {
                GetAllBindProperties(string.Empty, items, typeDefinition);
            }

            return items;
        }

        public static IReadOnlyList<string> GetAllBindProperties(string assemblyPath, string typeFullName, string propertyPath)
        {
            var items = new List<string>();

            var typeDefinition = FindType(assemblyPath, typeFullName);

            var list = propertyPath.SplitAndClear(".");
            for (var i = 0; i < list.Count; i++)
            {
                var propertyName = list[i];
                if (typeDefinition == null)
                {
                    return items;
                }

                var typeReference = typeDefinition.Properties.FirstOrDefault(p => p.Name == propertyName)?.PropertyType;

                if (i == list.Count - 1)
                {
                    var genericInstanceType = typeReference as GenericInstanceType;
                    if (genericInstanceType != null)
                    {
                        typeDefinition = genericInstanceType.GenericArguments.First().Resolve();
                        break;
                    }
                }

                typeDefinition = typeReference?.Resolve();
            }

            if (typeDefinition != null)
            {
                GetAllBindProperties(string.Empty, items, typeDefinition);
            }

            return items;
        }

        public static IReadOnlyList<string> GetAllCollectionProperties(string assemblyPath, string typeFullName)
        {
            var items = new List<string>();

            var typeDefinition = FindType(assemblyPath, typeFullName);
            if (typeDefinition != null)
            {
                GetAllCollectionProperties(string.Empty, items, typeDefinition);
            }

            return items;
        }

        public static IReadOnlyList<string> GetAllRequestNames(string assemblyPath)
        {
            return GetAllTypeNames(assemblyPath).Where(t => t.EndsWith("Request")).ToList();
        }

        public static IReadOnlyList<string> GetAllStringBindProperties(string assemblyPath, string typeFullName)
        {
            var items = new List<string>();

            var typeDefinition = FindType(assemblyPath, typeFullName);
            if (typeDefinition != null)
            {
                GetAllStringBindProperties(string.Empty, items, typeDefinition);
            }

            return items;
        }

        public static IReadOnlyList<string> GetAllTypeNames(string assemblyPath)
        {
            var items = new List<string>();

            VisitAllTypes(assemblyPath, type => { items.Add(type.FullName); });

            return items;
        }

        public static RequestIntellisenseData GetRequestIntellisenseData(string assemblyPath, string requestTypeFullName)
        {
            var data = new RequestIntellisenseData
            {
                RequestPropertyIntellisense           = GetAllBindProperties(assemblyPath, requestTypeFullName),
                RequestStringPropertyIntellisense     = GetAllStringBindProperties(assemblyPath, requestTypeFullName),
                RequestCollectionPropertyIntellisense = GetAllCollectionProperties(assemblyPath, requestTypeFullName),
                Collections                           = new Dictionary<string, IReadOnlyList<string>>()
            };

            foreach (var path in data.RequestCollectionPropertyIntellisense)
            {
                data.Collections[path] = GetAllBindProperties(assemblyPath, requestTypeFullName, path);
            }

            return data;
        }
        #endregion

        #region Methods
        static TypeDefinition FindType(string assemblyPath, string typeFullName)
        {
            var typeDefinitions = new List<TypeDefinition>();

            VisitAllTypes(assemblyPath, type =>
            {
                if (type.FullName == typeFullName)
                {
                    typeDefinitions.Add(type);
                }
            });

            return typeDefinitions.FirstOrDefault();
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

                if (PrimitiveTypes.Contains(propertyDefinition.PropertyType.FullName))
                {
                    paths.Add(pathPrefix + propertyDefinition.Name);
                    continue;
                }

                if (IsCollection(propertyDefinition.PropertyType))
                {
                    continue;
                }

                if (propertyDefinition.PropertyType.IsValueType == false)
                {
                    GetAllBindProperties(pathPrefix + propertyDefinition.Name + ".", paths, propertyDefinition.PropertyType.Resolve());
                }
            }
        }

        static void GetAllCollectionProperties(string pathPrefix, List<string> paths, TypeDefinition typeDefinition)
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

                if (PrimitiveTypes.Contains(propertyDefinition.PropertyType.FullName))
                {
                    continue;
                }

                if (IsCollection(propertyDefinition.PropertyType))
                {
                    paths.Add(pathPrefix + propertyDefinition.Name);
                    continue;
                }

                if (propertyDefinition.PropertyType.IsValueType == false)
                {
                    GetAllCollectionProperties(pathPrefix + propertyDefinition.Name + ".", paths, propertyDefinition.PropertyType.Resolve());
                }
            }
        }

        static void GetAllStringBindProperties(string pathPrefix, List<string> paths, TypeDefinition typeDefinition)
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

                if (propertyDefinition.PropertyType.FullName == typeof(string).FullName)
                {
                    paths.Add(pathPrefix + propertyDefinition.Name);
                    continue;
                }

                if (IsCollection(propertyDefinition.PropertyType))
                {
                    continue;
                }

                if (propertyDefinition.PropertyType.IsValueType == false)
                {
                    GetAllStringBindProperties(pathPrefix + propertyDefinition.Name + ".", paths, propertyDefinition.PropertyType.Resolve());
                }
            }
        }

        static bool IsCollection(TypeReference typeReference)
        {
            if (typeReference.FullName.StartsWith("System.Collections.Generic.List`1<"))
            {
                return true;
            }

            if (typeReference.FullName.StartsWith("System.Collections.Generic.IReadOnlyList`1<"))
            {
                return true;
            }

            if (typeReference.FullName.StartsWith("System.Collections.Generic.ICollection`1<"))
            {
                return true;
            }

            if (typeReference.FullName.StartsWith("System.Array"))
            {
                return true;
            }

            return false;
        }

        static void VisitAllTypes(string assemblyPath, Action<TypeDefinition> action)
        {
            if (File.Exists(assemblyPath) == false)
            {
                return;
            }

            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(@"d:\boa\server\bin\");

            var assemblyDefinition = AssemblyDefinition.ReadAssembly(assemblyPath, new ReaderParameters {AssemblyResolver = resolver});

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