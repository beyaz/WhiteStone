﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BOA.CodeGeneration.Common;
using BOA.CodeGeneration.Model;
using BOA.Common.Helpers;
using Mono.Cecil;

namespace BOA.CodeGeneration.Util
{
    /// <summary>
    ///     The extensions
    /// </summary>
    public static class Extensions
    {
        #region Static Fields
        /// <summary>
        ///     The culture information
        /// </summary>
        static readonly CultureInfo CultureInfo = new CultureInfo("en-US");
        #endregion

        #region Public Methods
        /// <summary>
        ///     Ases the method parameter.
        /// </summary>
        public static string AsMethodParameter(this string columnName)
        {
            if (columnName == null)
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            //return columnName[0].ToString().ToLowerTR() + columnName.Substring(1);
            var firstChar = columnName[0].ToString().ToLowerTR();
            if (firstChar == "ı")
            {
                firstChar = "i";
            }

            return firstChar + columnName.Substring(1);
        }

        /// <summary>
        ///     Finds the colum.
        /// </summary>
        /// <exception cref="Exception">
        ///     Where column not processed.@SearchValue" + ReflectionHelper.ExportObjectToCSharpCode(where) + Environment.NewLine +
        ///     "DataSource:" + ReflectionHelper.ExportObjectToCSharpCode(columns)
        /// </exception>
        public static ColumnInfo FindColumn(this Where where, IReadOnlyList<ColumnInfo> columns)
        {
            var c = columns.FirstOrDefault(x => x.ColumnName == where.Equal);

            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.BiggerThan);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.LessThan);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.LessThanOrEquals);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.BiggerThanOrEquals);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.StartsWith);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.EndsWith);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.Contains);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.NotEqual);
            if (c != null)
            {
                return c;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.IsNull);
            if (c != null)
            {
                return c;
            }

            throw new Exception("Where column not processed.@SearchValue" + ReflectionHelper.ExportObjectToCSharpCode(where) + Environment.NewLine +
                                "DataSource:" + ReflectionHelper.ExportObjectToCSharpCode(columns));
        }

        /// <summary>
        ///     Formats the code.
        /// </summary>
        public static string FormatCode(this string format, params object[] args)
        {
            return string.Format(CultureInfo, format, args);
        }

        /// <summary>
        ///     Gets the name of the property.
        /// </summary>
        public static string GetPropertyName(this Where where, IReadOnlyList<ColumnInfo> columns)
        {
            var c = columns.FirstOrDefault(x => x.ColumnName == where.Equal);

            if (c != null)
            {
                return null;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.BiggerThan);
            if (c != null)
            {
                return Names.BiggerThan;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.LessThan);
            if (c != null)
            {
                return Names.LessThan;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.LessThanOrEquals);
            if (c != null)
            {
                return Names.LessThanOrEquals;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.BiggerThanOrEquals);
            if (c != null)
            {
                return Names.BiggerThanOrEquals;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.StartsWith);
            if (c != null)
            {
                return Names.StartsWith;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.EndsWith);
            if (c != null)
            {
                return Names.EndsWith;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.Contains);
            if (c != null)
            {
                return Names.Contains;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.NotEqual);
            if (c != null)
            {
                return Names.NotEqual;
            }

            c = columns.FirstOrDefault(x => x.ColumnName == where.IsNull);
            if (c != null)
            {
                return Names.IsNull;
            }

            throw new Exception("Column");
        }

        /// <summary>
        ///     Gets the service.
        /// </summary>
        public static T GetService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }

        /// <summary>
        ///     Gets the service.
        /// </summary>
        public static TInterface GetService<TType, TInterface>(this IServiceProvider serviceProvider)
            where TInterface : class
        {
            return serviceProvider.GetService(typeof(TType)) as TInterface;
        }

        /// <summary>
        ///     Loads the type of the boa.
        /// </summary>
        public static ITypeDefinition LoadBOAType(this string classFullName)
        {
            if (classFullName == null)
            {
                return null;
            }

            if (classFullName == "bool" ||
                classFullName == "decimal?")
            {
                return new InternalTypeDefinition(classFullName);
            }

            var serverPath = @"d:\BOA\Server\bin\";

            var arr = classFullName.Split('.');

            var dllName = string.Join(".", arr.Where(x => x != arr.Last())) + ".dll";

            var path       = serverPath + dllName;
            var definition = AssemblyDefinition.ReadAssembly(path);

            var typeDefinition = definition.MainModule.Types.FirstOrDefault(t => t.FullName == classFullName);
            if (typeDefinition == null)
            {
                throw new MissingMemberException(classFullName);
            }

            return new InternalTypeDefinition(typeDefinition);
        }

        /// <summary>
        ///     To the i type definition.
        /// </summary>
        public static ITypeDefinition ToITypeDefinition(this TypeDefinition definition)
        {
            return new InternalTypeDefinition(definition);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Gets all properties.
        /// </summary>
        static List<PropertyDefinition> GetAllProperties(this TypeDefinition typeDefinition)
        {
            var properties = new List<PropertyDefinition>(
                                                          from PropertyDefinition property
                                                              in typeDefinition.Properties
                                                          select property);

            if (typeDefinition.BaseType != null &&
                typeDefinition.BaseType != typeDefinition.Module.Import(typeof(object)))
            {
                var moduleDefinition = typeDefinition.BaseType.Scope as ModuleDefinition;
                if (moduleDefinition != null)
                {
                    var baseType = moduleDefinition.GetType(typeDefinition.BaseType.FullName);
                    if (baseType != null)
                    {
                        properties.AddRange(baseType.GetAllProperties());
                    }
                }
            }

            return properties;
        }
        #endregion

        /// <summary>
        ///     .
        /// </summary>
        class InternalPropertyInfo : IPropertyDefinition
        {
            #region Fields
            /// <summary>
            ///     The definition
            /// </summary>
            readonly PropertyDefinition _definition;
            #endregion

            #region Constructors
            /// <summary>
            ///     Initializes a new instance of the <see cref="InternalPropertyInfo" /> class.
            /// </summary>
            public InternalPropertyInfo(PropertyDefinition definition)
            {
                if (definition == null)
                {
                    throw new ArgumentNullException(nameof(definition));
                }

                _definition = definition;
            }
            #endregion

            #region Public Properties
            /// <summary>
            ///     Gets the name.
            /// </summary>
            public string Name => _definition.Name;

            /// <summary>
            ///     Gets the type of the property.
            /// </summary>
            public Type PropertyType
            {
                get
                {
                    if (_definition.PropertyType.FullName.StartsWith("System.Nullable`1<"))
                    {
                        var parameterTypeName = _definition.PropertyType.FullName.RemoveFromStart("System.Nullable`1<").RemoveFromEnd(">");
                        var parameterType     = Type.GetType(parameterTypeName, true);

                        return typeof(Nullable<>).MakeGenericType(parameterType);
                    }

                    return Type.GetType(_definition.PropertyType.FullName, true);
                }
            }
            #endregion
        }

        /// <summary>
        ///     .
        /// </summary>
        class InternalTypeDefinition : ITypeDefinition
        {
            #region Fields
            /// <summary>
            ///     The definition
            /// </summary>
            readonly TypeDefinition _definition;

            /// <summary>
            ///     The properties
            /// </summary>
            IReadOnlyList<IPropertyDefinition> _properties;
            #endregion

            #region Constructors
            /// <summary>
            ///     Initializes a new instance of the <see cref="InternalTypeDefinition" /> class.
            /// </summary>
            public InternalTypeDefinition(TypeDefinition definition)
            {
                if (definition == null)
                {
                    throw new ArgumentNullException(nameof(definition));
                }

                _definition = definition;
            }

            /// <summary>
            ///     Initializes a new instance of the <see cref="InternalTypeDefinition" /> class.
            /// </summary>
            public InternalTypeDefinition(string classFullName)
            {
                PrimitiveTypeFullName = classFullName;
            }
            #endregion

            #region Public Properties
            /// <summary>
            ///     Gets the definition.
            /// </summary>
            public TypeDefinition Definition => _definition;

            /// <summary>
            ///     Gets the full name.
            /// </summary>
            public string FullName
            {
                get
                {
                    if (_definition == null)
                    {
                        return PrimitiveTypeFullName;
                    }

                    return _definition.FullName;
                }
            }

            /// <summary>
            ///     Gets the name.
            /// </summary>
            public string Name
            {
                get
                {
                    if (_definition == null)
                    {
                        return PrimitiveTypeFullName;
                    }

                    return _definition.Name;
                }
            }

            /// <summary>
            ///     Gets the properties.
            /// </summary>
            public IReadOnlyList<IPropertyDefinition> Properties
            {
                get
                {
                    if (_properties == null)
                    {
                        _properties = (from propertyDefinition in _definition.GetAllProperties()
                                       select new InternalPropertyInfo(propertyDefinition)).ToList();
                    }

                    return _properties;
                }
            }
            #endregion

            #region Properties
            /// <summary>
            ///     Gets the full name of the primitive type.
            /// </summary>
            string PrimitiveTypeFullName { get; }
            #endregion

            #region Public Methods
            /// <summary>
            ///     Returns a <see cref="System.String" /> that represents this instance.
            /// </summary>
            public override string ToString()
            {
                return FullName;
            }
            #endregion
        }
    }
}