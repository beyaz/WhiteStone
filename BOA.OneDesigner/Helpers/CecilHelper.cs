using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using Mono.Cecil;

namespace BOA.OneDesigner.Helpers
{
    public class CecilPropertyInfo
    {
        #region Public Properties
        public TypeReference DotNetType { get; set; }
        public string        Name       { get; set; }
        #endregion
    }

    public class RequestIntellisenseData
    {
        #region Public Properties
        public string                                    AssemblyPath                            { get; set; }
        public Dictionary<string, IReadOnlyList<string>> Collections                             { get; set; }
        public List<string>                              RequestBooleanPropertyIntellisense      { get; set; }
        public List<string>                              RequestCollectionPropertyIntellisense   { get; set; }
        public List<string>                              RequestNotNullInt32PropertyIntellisense { get; set; }
        public List<string>                              RequestPropertyIntellisense             { get; set; }
        public List<string>                              RequestStringPropertyIntellisense       { get; set; }
        public string                                    RequestTypeFullName                     { get; set; }
        public TypeDefinition                            TypeDefinition                          { get; set; }
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
            typeof(bool).FullName,

            FullNameOfNullableSbyte,
            FullNameOfNullableByte,
            FullNameOfNullableShort,
            FullNameOfNullableInt,
            FullNameOfNullableLong,
            FullNameOfNullableDecimal,
            FullNameOfNullableDateTime,
            FullNameOfNullableBoolean
        };
        #endregion

        #region Public Properties
        public static string FullNameOfNullableBoolean  => "System.Nullable`1<" + typeof(bool).FullName + ">";
        public static string FullNameOfNullableByte     => "System.Nullable`1<" + typeof(byte).FullName + ">";
        public static string FullNameOfNullableDateTime => "System.Nullable`1<" + typeof(DateTime).FullName + ">";
        public static string FullNameOfNullableDecimal  => "System.Nullable`1<" + typeof(decimal).FullName + ">";
        public static string FullNameOfNullableInt      => "System.Nullable`1<" + typeof(int).FullName + ">";
        public static string FullNameOfNullableLong     => "System.Nullable`1<" + typeof(long).FullName + ">";
        public static string FullNameOfNullableSbyte    => "System.Nullable`1<" + typeof(sbyte).FullName + ">";
        public static string FullNameOfNullableShort    => "System.Nullable`1<" + typeof(short).FullName + ">";
        #endregion

        #region Public Methods
        public static PropertyDefinition FindPropertyInfo(string assemblyPath, string typeFullName, string propertyPath)
        {
            var typeDefinition = FindType(assemblyPath, typeFullName);

            var list = propertyPath.SplitAndClear(".");

            for (var i = 0; i < list.Count; i++)
            {
                var propertyName = list[i];

                if (typeDefinition == null)
                {
                    throw Error.InvalidOperation();
                }

                if (i == list.Count - 1)
                {
                    return typeDefinition.Properties.FirstOrDefault(p => p.Name == propertyName);
                }

                var typeReference = typeDefinition.Properties.FirstOrDefault(p => p.Name == propertyName)?.PropertyType;

                typeDefinition = typeReference?.Resolve();
            }

            return null;
        }

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


        public static TypeReference FindTypeReferenceAtPath(TypeDefinition typeDefinition, string propertyPath)
        {
            var items = new List<string>();

            var list = propertyPath.SplitAndClear(".");
            for (var i = 0; i < list.Count; i++)
            {
                

                var propertyName = list[i];
                if (typeDefinition == null)
                {
                    return null;
                }

                var typeReference = typeDefinition.Properties.FirstOrDefault(p => p.Name == propertyName)?.PropertyType;

                if (i == list.Count - 1)
                {
                    return typeReference;
                }

                typeDefinition = typeReference?.Resolve();
            }

            return null;
        }

        public static IReadOnlyList<string> GetAllBindProperties(TypeDefinition typeDefinition, string propertyPath)
        {
            //TODO change name

            var genericInstanceType = FindTypeReferenceAtPath(typeDefinition,propertyPath) as GenericInstanceType;

            typeDefinition = genericInstanceType?.GenericArguments.First().Resolve();

            var items = new List<string>();
            if (typeDefinition != null)
            {
                GetAllBindProperties(string.Empty, items, typeDefinition);
            }

            return items;
        }

        public static IReadOnlyList<string> GetAllRequestNames(string assemblyPath)
        {
            return GetAllTypeNames(assemblyPath).Where(t => t.EndsWith("Request")).ToList();
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
                AssemblyPath        = assemblyPath,
                RequestTypeFullName = requestTypeFullName,

                RequestPropertyIntellisense             = new List<string>(),
                RequestStringPropertyIntellisense       = new List<string>(),
                RequestNotNullInt32PropertyIntellisense = new List<string>(),

                RequestBooleanPropertyIntellisense    = new List<string>(),
                RequestCollectionPropertyIntellisense = new List<string>(),
                Collections                           = new Dictionary<string, IReadOnlyList<string>>(),
                TypeDefinition                        = FindType(assemblyPath, requestTypeFullName)
            };

            CollectProperties(data, string.Empty, data.TypeDefinition);

            foreach (var path in data.RequestCollectionPropertyIntellisense)
            {
                data.Collections[path] = GetAllBindProperties(data.TypeDefinition, path);
            }

            return data;
        }
        #endregion

        #region Methods
        static void CollectProperties(RequestIntellisenseData data, string pathPrefix, TypeDefinition typeDefinition)
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
                    data.RequestPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(bool).FullName ||
                    propertyDefinition.PropertyType.FullName == FullNameOfNullableBoolean)
                {
                    data.RequestBooleanPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(string).FullName)
                {
                    data.RequestStringPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(int).FullName)
                {
                    data.RequestNotNullInt32PropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (IsCollection(propertyDefinition.PropertyType))
                {
                    data.RequestCollectionPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                    continue;
                }

                if (propertyDefinition.PropertyType.IsValueType == false)
                {
                    CollectProperties(data, pathPrefix + propertyDefinition.Name + ".", propertyDefinition.PropertyType.Resolve());
                }
            }
        }

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