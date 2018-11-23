using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for reflection operations.
    /// </summary>
    public static partial class ReflectionHelper
    {
        class ObjectToCSharpCodeExporter
        {
            #region Constants
            const string UnresolvedSymbol = "?";
            #endregion

            const string EnglishCultureName = "en-US";
            #region Static Fields
            static readonly CultureInfo EnglishCulture = new CultureInfo(EnglishCultureName);
            #endregion

            #region Fields
            readonly int           _padding = 4;
            readonly StringBuilder _sb      = new StringBuilder();
            int                    _currentPadding;
            #endregion

            #region Methods
            internal string Export(object obj)
            {
                Write(obj);

                return _sb.ToString();
            }

            /// <summary>
            ///     Gets default value of <paramref name="type" />
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
            ///     Return true if given <paramref name="value" /> is instanceOf <paramref name="targetType" />
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

            void Append(string value)
            {
                WritePadding();
                _sb.Append(value);
            }

            void AppendLine(string value)
            {
                WritePadding();
                _sb.AppendLine(value);
            }
            void AppendLine()
            {
                AppendLine(string.Empty);
            }

            void AppendNoPadding(string value)
            {
                _sb.Append(value);
            }

            void PaddingBack()
            {
                _currentPadding -= _padding;
            }

            void PaddingNext()
            {
                _currentPadding += _padding;
            }

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
                    const string StringStart = "\"";
                    const string StringStartWithMultipleLine = "@\"";
                    if (str.Contains(Environment.NewLine) || str.Contains('\\'))
                    {
                        AppendNoPadding(StringStartWithMultipleLine + str + StringStart);
                        return;
                    }

                    AppendNoPadding(StringStart + str + StringStart);
                    return;
                }

                if (obj is sbyte ||
                    obj is byte ||
                    obj is short ||
                    obj is ushort ||
                    obj is int ||
                    obj is double ||
                    obj is uint ||
                    obj is long ||
                    obj is ulong)
                {
                    AppendNoPadding(obj.ToString());
                    return;
                }

                if (obj is decimal)
                {
                    const string DecimalEnd = "M";
                    AppendNoPadding(obj + DecimalEnd);
                    return;
                }

                if (obj is bool)
                {
                    AppendNoPadding(obj.ToString().ToLower(EnglishCulture));
                    return;
                }

                if (obj is float)
                {
                    const string FloatEnd = "f";
                    AppendNoPadding(obj + FloatEnd);
                    return;
                }

                if (obj is Guid)
                {
                    var guid = (Guid) obj;
                    if (guid == Guid.Empty)
                    {
                        const string EmptyGuid = "System.Guid.Empty";
                        AppendNoPadding(EmptyGuid);
                        return;
                    }

                    const string GuidCreationFormat = "new System.Guid(\"{0}\")";
                    AppendNoPadding(string.Format(EnglishCulture, GuidCreationFormat, guid));
                    return;
                }

                if (obj is DateTime)
                {
                    var date = (DateTime) obj;
                    const string DateCreationFormat = "new System.DateTime({0},{1},{2},{3},{4},{5},{6})";
                    AppendNoPadding(string.Format(EnglishCulture, DateCreationFormat, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond));
                    return;
                }

                if (obj is Enum)
                {
                    const string Dot = ".";
                    AppendNoPadding(obj.GetType().FullName + Dot + obj);
                    return;
                }

                var type = obj.GetType();

                if (IsInstanceOfGeneric(typeof(KeyValuePair<,>), obj))
                {
                    var fullName = type.ToString().Replace("`2[", "<").Replace("]", ">");
                    AppendNoPadding(fullName);
                    const string LeftParenthesis = "(";
                    AppendNoPadding(LeftParenthesis);

                    object key   = UnresolvedSymbol;
                    object value = UnresolvedSymbol;

                    const string NameKey = "Key";
                    var propertyInfoKey = type.GetProperty(NameKey);
                    if (propertyInfoKey != null)
                    {
                        key = propertyInfoKey.GetValue(obj, null);
                    }

                    const string NameValue = "Value";
                    var propertyInfoValue = type.GetProperty(NameValue);
                    if (propertyInfoValue != null)
                    {
                        value = propertyInfoValue.GetValue(obj, null);
                    }

                    Write(key);
                    AppendNoPadding(",");
                    Write(value);

                    AppendNoPadding(")");
                    return;
                }

                var isFirstPropertyWrite = true;

                AppendNoPadding("new ");

                if (IsInstanceOfGeneric(typeof(List<>), obj))
                {
                    var fullName = type.ToString().Replace("`1[", "<").Replace("]", ">");

                    AppendNoPadding(fullName);
                    AppendLine();
                    AppendLine("{");

                    PaddingNext();

                    foreach (var item in (IList) obj)
                    {
                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                            AppendLine();
                        }

                        WritePadding();
                        Write(item);
                    }

                    PaddingBack();
                    AppendLine();
                    Append("}");

                    return;
                }

                if (IsInstanceOfGeneric(typeof(Dictionary<,>), obj))
                {
                    var fullName = type.ToString().Replace("`2[", "<").Replace("]", ">");

                    AppendNoPadding(fullName);
                    AppendLine();
                    AppendLine("{");

                    PaddingNext();

                    foreach (DictionaryEntry item in (IDictionary) obj)
                    {
                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                            AppendLine();
                        }

                        WritePadding();
                        AppendNoPadding("{");
                        Write(item.Key);
                        AppendNoPadding(",");
                        Write(item.Value);
                        AppendNoPadding("}");
                    }

                    PaddingBack();
                    AppendLine();
                    Append("}");

                    return;
                }

                AppendNoPadding(type.FullName);
                AppendLine();
                Append("{");

                PaddingNext();

                var array = obj as Array;
                if (array != null)
                {
                    foreach (var value in array)
                    {
                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                            AppendLine();
                        }

                        Write(value);
                    }
                }
                else
                {
                    foreach (var property in type.GetProperties().Where(p => p.CanRead && p.CanWrite))
                    {
                        var value = property.GetValue(obj, null);
                        if (value == null)
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

                        if (isFirstPropertyWrite)
                        {
                            isFirstPropertyWrite = false;
                        }
                        else
                        {
                            AppendNoPadding(",");
                        }

                        AppendLine();

                        Append(property.Name);
                        AppendNoPadding(" = ");

                        Write(value);
                    }
                }

                PaddingBack();

                AppendLine();
                Append("}");
            }

            void WritePadding()
            {
                if (_currentPadding > 0)
                {
                    _sb.Append("".PadRight(_currentPadding, ' '));
                }
            }
            #endregion
        }
    }
}