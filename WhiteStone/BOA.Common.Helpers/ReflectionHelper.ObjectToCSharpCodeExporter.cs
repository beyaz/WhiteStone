using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The reflection helper
    /// </summary>
    public static partial class ReflectionHelper
    {
        /// <summary>
        ///     The object to c sharp code exporter
        /// </summary>
        internal class ObjectToCSharpCodeExporter
        {
            #region Constants
            /// <summary>
            ///     The comma
            /// </summary>
            const string Comma = ",";

            /// <summary>
            ///     The dot
            /// </summary>
            const string Dot = ".";

            /// <summary>
            ///     The english culture name
            /// </summary>
            const string EnglishCultureName = "en-US";

            /// <summary>
            ///     The left brace
            /// </summary>
            const string LeftBrace = "{";

            /// <summary>
            ///     The left parenthesis
            /// </summary>
            const string LeftParenthesis = "(";

            /// <summary>
            ///     The padding
            /// </summary>
            const int Padding = 4;

            /// <summary>
            ///     The right brace
            /// </summary>
            const string RightBrace = "}";

            /// <summary>
            ///     The right parenthesis
            /// </summary>
            const string RightParenthesis = ")";

            /// <summary>
            ///     The unresolved symbol
            /// </summary>
            const string UnresolvedSymbol = "?";
            #endregion

            #region Static Fields
            /// <summary>
            ///     The english culture
            /// </summary>
            static readonly CultureInfo EnglishCulture = new CultureInfo(EnglishCultureName);
            #endregion

            #region Fields
            /// <summary>
            ///     The object creation stack
            /// </summary>
            internal readonly Stack<object> _objectCreationStack = new Stack<object>();

            /// <summary>
            ///     The using list
            /// </summary>
            internal List<string> UsingList = new List<string>();

            /// <summary>
            ///     The sb
            /// </summary>
            readonly StringBuilder _sb = new StringBuilder();

            /// <summary>
            ///     The current padding
            /// </summary>
            int _currentPadding;
            #endregion

            #region Public Methods
            /// <summary>
            ///     Exports the specified object.
            /// </summary>
            public string Export(object obj)
            {
                Write(obj);

                return _sb.ToString();
            }
            #endregion

            #region Methods
            /// <summary>
            ///     Cleans the name of the generic type.
            /// </summary>
            static string CleanGenericTypeName(string typeName, int genericParameterCount)
            {
                const string NewValue = "<";

                const string Format = "`{0}[";

                var OldValue = string.Format(EnglishCulture, Format, genericParameterCount);

                return typeName.Replace(OldValue, NewValue).Replace(']', '>');
            }

            /// <summary>
            ///     Gets the default type of the value from.
            /// </summary>
            static object GetDefaultValueFromType(Type type)
            {
                if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
                {
                    return Activator.CreateInstance(type);
                }

                return null;
            }

            /// <summary>
            ///     Determines whether [is instance of generic] [the specified target type].
            /// </summary>
            static bool IsInstanceOfGeneric(Type targetType, object value)
            {
                if (value == null)
                {
                    return false;
                }

                var oType = value.GetType();

                if (oType.IsGenericType && oType.GetGenericTypeDefinition() == targetType)
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Determines whether [is numeric type] [the specified type].
            /// </summary>
            static bool IsNumericType(Type type)
            {
                if (type == typeof(sbyte) ||
                    type == typeof(byte) ||
                    type == typeof(short) ||
                    type == typeof(ushort) ||
                    type == typeof(int) ||
                    type == typeof(double) ||
                    type == typeof(uint) ||
                    type == typeof(long) ||
                    type == typeof(float) ||
                    type == typeof(decimal) ||
                    type == typeof(ulong))
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            ///     Appends the specified value.
            /// </summary>
            void Append(string value)
            {
                WritePadding();
                _sb.Append(value);
            }

            /// <summary>
            ///     Appends the line.
            /// </summary>
            void AppendLine(string value)
            {
                WritePadding();
                _sb.AppendLine(value);
            }

            /// <summary>
            ///     Appends the line.
            /// </summary>
            void AppendLine()
            {
                AppendLine(string.Empty);
            }

            /// <summary>
            ///     Appends the no padding.
            /// </summary>
            void AppendNoPadding(string value)
            {
                _sb.Append(value);
            }

            /// <summary>
            ///     Gets the name of the type.
            /// </summary>
            string GetTypeName(Type type)
            {
                return GetTypeName(type.FullName);
            }

            /// <summary>
            ///     Gets the name of the type.
            /// </summary>
            string GetTypeName(string typeFullName)
            {
                if (UsingList == null)
                {
                    return typeFullName;
                }

                foreach (var usingName in UsingList)
                {
                    var prefix = usingName + Dot;

                    if (typeFullName.Trim().StartsWith(prefix))
                    {
                        return typeFullName.Replace(prefix, string.Empty);
                    }
                }

                return typeFullName;
            }

            /// <summary>
            ///     Paddings the back.
            /// </summary>
            void PaddingBack()
            {
                _currentPadding -= Padding;
            }

            /// <summary>
            ///     Paddings the next.
            /// </summary>
            void PaddingNext()
            {
                _currentPadding += Padding;
            }

            /// <summary>
            ///     Writes the specified object.
            /// </summary>
            void Write(object obj)
            {
                if (obj == null)
                {
                    const string NullValue = "null";
                    AppendNoPadding(NullValue);
                    return;
                }

                var str = obj as string;
                if (str != null)
                {
                    const string StringStart                 = "\"";
                    const string StringStartWithMultipleLine = "@\"";
                    if (str.Contains(Environment.NewLine) || str.Contains('\\'))
                    {
                        AppendNoPadding(StringStartWithMultipleLine + str + StringStart);
                        return;
                    }

                    AppendNoPadding(StringStart + str + StringStart);
                    return;
                }

                if (obj is decimal)
                {
                    const string DecimalEnd = "M";
                    AppendNoPadding(obj + DecimalEnd);
                    return;
                }

                if (obj is float)
                {
                    const string FloatEnd = "f";
                    AppendNoPadding(obj + FloatEnd);
                    return;
                }

                if (IsNumericType(obj.GetType()))
                {
                    AppendNoPadding(obj.ToString());
                    return;
                }

                if (obj is bool)
                {
                    AppendNoPadding(obj.ToString().ToLower(EnglishCulture));
                    return;
                }

                if (obj is Guid)
                {
                    var guid = (Guid) obj;
                    if (guid == Guid.Empty)
                    {
                        const string EmptyGuid = "System.Guid.Empty";
                        AppendNoPadding(GetTypeName(EmptyGuid));
                        return;
                    }

                    const string GuidCreationFormat = "new {0}(\"{1}\")";
                    AppendNoPadding(string.Format(EnglishCulture, GuidCreationFormat, GetTypeName(typeof(Guid)), guid));
                    return;
                }

                if (obj is DateTime)
                {
                    var          date               = (DateTime) obj;
                    const string DateCreationFormat = "new {0}({1},{2},{3},{4},{5},{6},{7})";
                    AppendNoPadding(string.Format(EnglishCulture, DateCreationFormat, GetTypeName(typeof(DateTime)), date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond));
                    return;
                }

                if (obj is Enum)
                {
                    AppendNoPadding(GetTypeName(obj.GetType().FullName) + Dot + obj);
                    return;
                }

                var type = obj.GetType();

                if (IsInstanceOfGeneric(typeof(KeyValuePair<,>), obj))
                {
                    var fullName = CleanGenericTypeName(type.ToString(), 2);

                    AppendNoPadding(GetTypeName(fullName));

                    AppendNoPadding(LeftParenthesis);

                    object key   = UnresolvedSymbol;
                    object value = UnresolvedSymbol;

                    const string NameofKey       = "Key";
                    var          propertyInfoKey = type.GetProperty(NameofKey);
                    if (propertyInfoKey != null)
                    {
                        key = propertyInfoKey.GetValue(obj, null);
                    }

                    const string NameofValue       = "Value";
                    var          propertyInfoValue = type.GetProperty(NameofValue);
                    if (propertyInfoValue != null)
                    {
                        value = propertyInfoValue.GetValue(obj, null);
                    }

                    Write(key);
                    AppendNoPadding(Comma);
                    Write(value);

                    AppendNoPadding(RightParenthesis);
                    return;
                }

                var isFirstPropertyWrite = true;

                const string NewCreation = "new ";
                AppendNoPadding(NewCreation);

                if (IsInstanceOfGeneric(typeof(List<>), obj))
                {
                    var fullName = CleanGenericTypeName(type.ToString(), 1);

                    AppendNoPadding(GetTypeName(fullName));
                    AppendLine();

                    AppendLine(LeftBrace);

                    PaddingNext();

                    foreach (var item in (IList) obj)
                    {
                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(Comma);
                            AppendLine();
                        }

                        WritePadding();
                        Write(item);
                    }

                    PaddingBack();
                    AppendLine();

                    Append(RightBrace);

                    return;
                }

                if (IsInstanceOfGeneric(typeof(IDictionary<,>), obj))
                {
                    var fullName = CleanGenericTypeName(type.ToString(), 2);

                    AppendNoPadding(GetTypeName(fullName));
                    AppendLine();
                    AppendLine(LeftBrace);

                    PaddingNext();

                    foreach (DictionaryEntry item in (IDictionary) obj)
                    {
                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(Comma);
                            AppendLine();
                        }

                        WritePadding();
                        AppendNoPadding(LeftBrace);
                        Write(item.Key);
                        AppendNoPadding(Comma);
                        Write(item.Value);
                        AppendNoPadding(RightBrace);
                    }

                    PaddingBack();
                    AppendLine();
                    Append(RightBrace);

                    return;
                }

                AppendNoPadding(GetTypeName(type.FullName));

                var array = obj as Array;

                if (array != null)
                {
                    if (IsNumericType(array.GetType().GetElementType()))
                    {
                        AppendNoPadding(LeftBrace);
                        foreach (var value in array)
                        {
                            if (isFirstPropertyWrite)
                            {
                                isFirstPropertyWrite = false;
                            }
                            else
                            {
                                AppendNoPadding(Comma);
                            }

                            Write(value);
                        }

                        AppendNoPadding(RightBrace);
                        return;
                    }

                    AppendLine();
                    Append(LeftBrace);

                    PaddingNext();

                    foreach (var value in array)
                    {
                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(Comma);
                            AppendLine();
                        }

                        Write(value);
                    }

                    PaddingBack();

                    AppendLine();
                    Append(RightBrace);

                    return;
                }

                _objectCreationStack.Push(obj);

                var constructorParameterValues = new List<object>();

                var constructorInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (constructorInfos.Length == 1)
                {
                    var parameterInfos = constructorInfos[0].GetParameters();
                    if (parameterInfos.Length > 0)
                    {
                        foreach (var parameterInfo in parameterInfos)
                        {
                            var searchPropertyInfo = type.GetProperties().FirstOrDefault(p => p.CanRead && (p.SetMethod == null || p.SetMethod != null && p.SetMethod.IsPrivate) && p.PropertyType == parameterInfo.ParameterType);
                            if (searchPropertyInfo != null)
                            {
                                constructorParameterValues.Add(searchPropertyInfo.GetValue(obj));
                            }
                            else
                            {
                                constructorParameterValues.Add(GetDefaultValueFromType(parameterInfo.ParameterType));
                            }
                        }

                        AppendNoPadding(LeftParenthesis);
                        var isFirst = true;
                        foreach (var parameterValue in constructorParameterValues)
                        {
                            if (!isFirst)
                            {
                                AppendNoPadding(Comma);
                            }

                            Write(parameterValue);
                            isFirst = false;
                        }

                        AppendNoPadding(RightParenthesis);
                    }
                }

                var propertyValues = new List<KeyValuePair<string, object>>();

                foreach (var property in type.GetProperties().Where(p => p.CanRead && p.SetMethod != null && p.SetMethod.IsPublic))
                {
                    var value = property.GetValue(obj, null);
                    if (value == null)
                    {
                        continue;
                    }

                    var hasCircularReference = _objectCreationStack.Contains(value);
                    if (hasCircularReference)
                    {
                        continue;
                    }

                    if (value.Equals(GetDefaultValueFromType(property.PropertyType)))
                    {
                        continue;
                    }

                    if (property.PropertyType == typeof(DateTime))
                    {
                        if (value.Equals(new DateTime()))
                        {
                            continue;
                        }
                    }

                    propertyValues.Add(new KeyValuePair<string, object>(property.Name, value));
                }

                if (propertyValues.Count == 0)
                {
                    if (constructorParameterValues.Count == 0)
                    {
                        AppendNoPadding(LeftParenthesis);
                        AppendNoPadding(RightParenthesis);
                    }

                    _objectCreationStack.Pop();
                    return;
                }

                AppendLine();
                Append(LeftBrace);

                PaddingNext();

                foreach (var pair in propertyValues)
                {
                    var propertyName  = pair.Key;
                    var propertyValue = pair.Value;

                    if (isFirstPropertyWrite)
                    {
                        isFirstPropertyWrite = false;
                    }
                    else
                    {
                        AppendNoPadding(Comma);
                    }

                    AppendLine();

                    Append(propertyName);
                    const string Assignment = " = ";
                    AppendNoPadding(Assignment);

                    Write(propertyValue);
                }

                PaddingBack();

                AppendLine();
                Append(RightBrace);

                _objectCreationStack.Pop();
            }

            /// <summary>
            ///     Writes the padding.
            /// </summary>
            void WritePadding()
            {
                if (_currentPadding > 0)
                {
                    _sb.Append(string.Empty.PadRight(_currentPadding, ' '));
                }
            }
            #endregion
        }
    }
}