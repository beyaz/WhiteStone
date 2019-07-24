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
        public bool IsBoolean         { get; set; }
        public bool IsBooleanNullable { get; set; }
        public bool IsByte            { get; set; }
        public bool IsByteNullable    { get; set; }

        public bool IsDate            { get; set; }
        public bool IsDateNullable    { get; set; }
        public bool IsDecimal         { get; set; }
        public bool IsDecimalNullable { get; set; }

        public bool IsInt32         { get; set; }
        public bool IsInt32Nullable { get; set; }
        public bool IsLong          { get; set; }
        public bool IsLongNullable  { get; set; }
        public bool IsNumber        { get; set; }
        public bool IsShort         { get; set; }
        public bool IsShortNullable { get; set; }

        public string Name { get; set; }
        #endregion
    }

    public class RequestIntellisenseData
    {
        #region Public Properties
        public string                                    AssemblyPath                            { get; set; }
        public Dictionary<string, PropertyDefinition>    CollectionDetails                       { get; set; }
        public Dictionary<string, IReadOnlyList<string>> Collections                             { get; set; }
        public List<string>                              OrchestrationMethods                    { get; set; } = new List<string>();
        public List<string>                              RequestBooleanPropertyIntellisense      { get; set; }

        public List<string> RequestClassPropertyIntellisense { get; set; }
        public List<string>                              RequestCollectionPropertyIntellisense   { get; set; }
        public List<string>                              RequestNotNullInt32PropertyIntellisense { get; set; }
        public List<string>                              RequestPropertyIntellisense             { get; set; }

        
        public List<string> RequestJsSupportTypesPropertyIntellisense { get; set; }

        public List<string>                              RequestStringPropertyIntellisense       { get; set; }

        public List<string> RequestDatePropertyIntellisense { get; set; }
        


        public string                                    RequestTypeFullName                     { get; set; }
        public TypeDefinition                            TypeDefinition                          { get; set; }
        public List<string> RequestNullableInt32PropertyIntellisense { get; set; }
        public List<string> ProcessedClassNames { get; set; }
        public bool HasPropertyLikeDialogResponse { get; set; }
        public bool HasPropertyLikeErrorTexts { get; set; }
        #endregion


        public void AddRequestPropertyIntellisense(string value)
        {
            if (RequestPropertyIntellisense.Contains(value.Trim()))
            {
                return;
            }

            RequestPropertyIntellisense.Add(value.Trim());
        }
        public void AddRequestJsSupportTypesPropertyIntellisense(string value)
        {
            if (RequestJsSupportTypesPropertyIntellisense.Contains(value.Trim()))
            {
                return;
            }

            RequestJsSupportTypesPropertyIntellisense.Add(value.Trim());
        }




        #region Public Methods
        public CecilPropertyInfo FindPropertyInfoInCollectionFirstGenericArgumentType(string dataSourceBindingPath, string bindingPath)
        {
            var                key                = dataSourceBindingPath + bindingPath;
            PropertyDefinition propertyDefinition = null;
            if (CollectionDetails.TryGetValue(key, out propertyDefinition))
            {
                var info = new CecilPropertyInfo
                {
                    Name = propertyDefinition.Name
                };

                if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableDecimal)
                {
                    info.IsDecimalNullable = true;
                    info.IsNumber          = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(decimal).FullName)
                {
                    info.IsDecimal = true;
                    info.IsNumber  = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(DateTime).FullName)
                {
                    info.IsDate = true;
                }
                else if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableDateTime)
                {
                    info.IsDateNullable = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(int).FullName)
                {
                    info.IsInt32  = true;
                    info.IsNumber = true;
                }
                else if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableInt)
                {
                    info.IsInt32Nullable = true;
                    info.IsNumber        = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(byte).FullName)
                {
                    info.IsByte   = true;
                    info.IsNumber = true;
                }
                else if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableByte)
                {
                    info.IsByteNullable = true;
                    info.IsNumber       = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(short).FullName)
                {
                    info.IsShort  = true;
                    info.IsNumber = true;
                }
                else if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableShort)
                {
                    info.IsShortNullable = true;
                    info.IsNumber        = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(long).FullName)
                {
                    info.IsLong   = true;
                    info.IsNumber = true;
                }
                else if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableLong)
                {
                    info.IsLongNullable = true;
                    info.IsNumber       = true;
                }
                else if (propertyDefinition.PropertyType.FullName == typeof(bool).FullName)
                {
                    info.IsBoolean = true;
                }
                else if (propertyDefinition.PropertyType.FullName == CecilHelper.FullNameOfNullableBoolean)
                {
                    info.IsBooleanNullable = true;
                }

                return info;
            }

            return null;
        }
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
            return FindPropertyInfo(typeDefinition, propertyPath);
        }

        static bool IsObjectType(this TypeReference typeReference)
        {
            return typeReference.FullName == "System.Object";
        }

        public static PropertyDefinition FindPropertyInfo(TypeDefinition typeDefinition, string propertyPath)
        {
            var list = propertyPath.SplitAndClear(".");

            for (var i = 0; i < list.Count; i++)
            {
                var propertyName = list[i];

                if (typeDefinition == null)
                {
                    throw Error.InvalidBindingPath(propertyPath);
                }

                if (i == list.Count - 1)
                {
                    while (true)
                    {
                        var propertyDefinition = typeDefinition.Properties.FirstOrDefault(p => p.Name == propertyName);
                        if (propertyDefinition != null)
                        {
                            return propertyDefinition;
                        }

                        if (typeDefinition.BaseType.IsObjectType())
                        {
                            return null;
                        }

                        typeDefinition = typeDefinition.BaseType.Resolve();
                    }
                }

                var typeReference = typeDefinition.Properties.FirstOrDefault(p => p.Name == propertyName)?.PropertyType;

                typeDefinition = typeReference?.Resolve();
            }

            return null;
        }

        public static TypeReference FindTypeReferenceAtPath(TypeDefinition typeDefinition, string propertyPath)
        {
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

       

        public static IReadOnlyList<string> GetAllBindPropertiesOfCollectionPropertyFirstGenericArgumentType(TypeDefinition typeDefinition, string collectionPropertyPath)
        {
            var genericInstanceType = FindTypeReferenceAtPath(typeDefinition, collectionPropertyPath) as GenericInstanceType;

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
                ProcessedClassNames = new List<string>{requestTypeFullName},
                AssemblyPath        = assemblyPath,
                RequestTypeFullName = requestTypeFullName,

                RequestPropertyIntellisense             = new List<string>(),
                RequestJsSupportTypesPropertyIntellisense = new List<string>(),
                RequestStringPropertyIntellisense       = new List<string>(),
                RequestDatePropertyIntellisense = new List<string>(),
                RequestNotNullInt32PropertyIntellisense = new List<string>(),
                RequestNullableInt32PropertyIntellisense = new List<string>(),

                RequestBooleanPropertyIntellisense    = new List<string>(),
                RequestClassPropertyIntellisense = new List<string>(),
                RequestCollectionPropertyIntellisense = new List<string>(),
                Collections                           = new Dictionary<string, IReadOnlyList<string>>(),
                TypeDefinition                        = FindType(assemblyPath, requestTypeFullName),

                CollectionDetails = new Dictionary<string, PropertyDefinition>()
            };

            if (data.TypeDefinition == null)
            {
                return null;
            }

            CollectProperties(data, string.Empty, data.TypeDefinition);

            foreach (var path in data.RequestCollectionPropertyIntellisense)
            {
                data.Collections[path] = GetAllBindPropertiesOfCollectionPropertyFirstGenericArgumentType(data.TypeDefinition, path);

                var typeDefinition = (FindTypeReferenceAtPath(data.TypeDefinition, path) as GenericInstanceType)?.GenericArguments?.FirstOrDefault()?.Resolve();
                foreach (var propertyPath in data.Collections[path])
                {
                    var propertyInfo = FindPropertyInfo(typeDefinition, propertyPath);
                    if (propertyInfo == null)
                    {
                        throw Error.InvalidOperation();
                    }
                    data.CollectionDetails[path + propertyPath] = propertyInfo;
                }
            }

            var orchestrationAssemblyPath = assemblyPath.Replace(".Types.", ".Orchestration.");
            var orchestrationTypeFullName = requestTypeFullName.Replace(".Types.", ".Orchestration.").RemoveFromEnd("Request");

            var orchestrationTypeDefinition = FindType(orchestrationAssemblyPath, orchestrationTypeFullName);
            if (orchestrationTypeDefinition != null)
            {
                foreach (var methodDefinition in orchestrationTypeDefinition.Methods)
                {
                    if (!methodDefinition.IsPublic)
                    {
                        continue;
                    }

                    if (methodDefinition.Parameters.Count != 2)
                    {
                        continue;
                    }

                    if (methodDefinition.Parameters[0].ParameterType.FullName != requestTypeFullName)
                    {
                        continue;
                    }

                    if (methodDefinition.Parameters[1].ParameterType.FullName != "BOA.Base.ObjectHelper")
                    {
                        continue;
                    }

                    var returnType = methodDefinition.ReturnType as GenericInstanceType;
                    if (returnType == null)
                    {
                        continue;
                    }

                    if (returnType.GenericArguments.Count != 1)
                    {
                        continue;
                    }

                    if (returnType.GenericArguments[0].FullName != requestTypeFullName)
                    {
                        continue;
                    }

                    data.OrchestrationMethods.Add(methodDefinition.Name);
                }
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

            if (typeDefinition.FullName == "System.Object")
            {
                return;
            }

            foreach (var propertyDefinition in typeDefinition.Properties)
            {
                if (propertyDefinition.GetMethod == null || propertyDefinition.SetMethod == null)
                {
                    continue;
                }

                if (propertyDefinition.Name == "DialogResponse")
                {
                    data.HasPropertyLikeDialogResponse = true;
                }
                if (propertyDefinition.Name == "ErrorTexts")
                {
                    data.HasPropertyLikeErrorTexts = true;
                }


                if (PrimitiveTypes.Contains(propertyDefinition.PropertyType.FullName))
                {
                    data.AddRequestPropertyIntellisense(pathPrefix + propertyDefinition.Name);
                    data.AddRequestJsSupportTypesPropertyIntellisense(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(bool).FullName ||
                    propertyDefinition.PropertyType.FullName == FullNameOfNullableBoolean)
                {
                    data.RequestBooleanPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(DateTime).FullName ||
                    propertyDefinition.PropertyType.FullName == FullNameOfNullableDateTime)
                {
                    data.RequestDatePropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(string).FullName)
                {
                    data.RequestStringPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == typeof(int).FullName)
                {
                    data.RequestNotNullInt32PropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (propertyDefinition.PropertyType.FullName == FullNameOfNullableInt)
                {
                    data.RequestNullableInt32PropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                }

                if (IsCollection(propertyDefinition.PropertyType))
                {
                    data.AddRequestPropertyIntellisense(pathPrefix + propertyDefinition.Name);
                    data.RequestCollectionPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);
                    continue;
                }

                if (propertyDefinition.PropertyType.IsValueType == false)
                {

                    data.RequestClassPropertyIntellisense.Add(pathPrefix + propertyDefinition.Name);

                    data.AddRequestPropertyIntellisense(pathPrefix + propertyDefinition.Name);

                    if (data.ProcessedClassNames.Contains(propertyDefinition.PropertyType.FullName) )
                    {
                        continue;
                    }
                    CollectProperties(data, pathPrefix + propertyDefinition.Name + ".", propertyDefinition.PropertyType.Resolve());
                }
            }

            CollectProperties(data,pathPrefix, typeDefinition.BaseType.Resolve());
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

            if (  typeDefinition.FullName == "System.Object")
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


            GetAllBindProperties(pathPrefix, paths, typeDefinition.BaseType.Resolve());


        }

        public static bool IsCollection(TypeReference typeReference)
        {
            if (typeReference == null)
            {
                return false;
            }
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

            if (typeReference.FullName.StartsWith("System.Collections.Generic.IReadOnlyCollection`1<"))
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

        public static IReadOnlyCollection<string> GetAttributeAttachedPropertyNames(string assemblyPath, string className, string attributeName)
        {
            var typeDefinition = FindType(assemblyPath, className);
            if (typeDefinition == null)
            {
                throw new ArgumentException("AssemblyNotfound."+assemblyPath);
            }

            return (from p in typeDefinition.Properties where p.CustomAttributes.Any(a => a.AttributeType.Name == attributeName) select p.Name).ToList();
        }

        public static IReadOnlyCollection<string> GetAttributeAttachedPropertyNames_DoNotSendToServerFromClientAttribute(string assemblyPath, string className)
        {
            return GetAttributeAttachedPropertyNames( assemblyPath,  className,  "DoNotSendToServerFromClientAttribute");
        }
    }
}