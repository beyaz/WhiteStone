using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace BOA.Jaml
{
    /// <summary>
    ///     Defines the builder utility.
    /// </summary>
    public static class BuilderUtility
    {
        /// <summary>
        ///     Determines whether value is BindingExpression.
        /// </summary>
        public static bool IsBindingExpression(this string value)
        {
            if (value == null)
            {
                return false;
            }
            if (value.Replace(" ", "").StartsWith("{Binding", StringComparison.Ordinal))
            {
                return true;
            }
            value = value.Trim();

            if (value.StartsWith("{", StringComparison.OrdinalIgnoreCase) && value.EndsWith("}", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Gets the binding.
        /// </summary>
        public static Binding ConvertToBinding(this string bindingExpressionAsText, ITypeFinder typeFinder)
        {
            var binding = new Binding
            {
                Mode = BindingMode.TwoWay
            };
            var list = bindingExpressionAsText.Split('{', '}', '=', ',', ' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToList();

            var pathIndex = list.FindIndex(x => x.Contains("Binding")) + 1;
            if (list[pathIndex] == "Path")
            {
                pathIndex += 1;
            }

            binding.Path = new PropertyPath(list[pathIndex]);

            var modeIndex = list.FindIndex(x => x == "Mode");
            if (modeIndex > 0)
            {
                var mode = BindingMode.Default;

                var isParsed = Enum.TryParse(list[modeIndex + 1], true, out mode);
                if (!isParsed)
                {
                    throw Errors.ParsingError(list[modeIndex + 1]);
                }
                binding.Mode = mode;
            }

            var converterIndex = list.FindIndex(x => x == "Converter");
            if (converterIndex > 0)
            {
                var converterTypeName = list[converterIndex + 1];
                var converterType     = typeFinder.GetType(converterTypeName);
                binding.Converter = (IValueConverter)Activator.CreateInstance(converterType);
            }

            var updateSourceTriggerIndex = list.FindIndex(x => x == "UpdateSourceTrigger");
            if (updateSourceTriggerIndex > 0)
            {
                var updateSourceTriggerValue = list[updateSourceTriggerIndex + 1];

                binding.UpdateSourceTrigger = (UpdateSourceTrigger)Enum.Parse(typeof(UpdateSourceTrigger), updateSourceTriggerValue, true);
            }

            return binding;
        }

        /// <summary>
        ///     Searches the dependency property in view.
        /// </summary>
        public static DependencyProperty SearchDependencyProperty(string dpFullName, ITypeFinder typeFinder)
        {
            var lastDotIndex = dpFullName.LastIndexOf('.');
            var propertyName = dpFullName.Substring(lastDotIndex + 1);

            var fieldInfo = typeFinder.GetType(dpFullName.Substring(0, lastDotIndex))
                                      .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                                      .FirstOrDefault(p => p.FieldType == typeof(DependencyProperty) &&
                                                           p.Name == propertyName);

            if (fieldInfo == null)
            {
                return null;
            }

            return (DependencyProperty)fieldInfo.GetValue(null);
        }

        /// <summary>
        ///     Normalizes the given <paramref name="value" /> for given <paramref name="targetType" />.
        ///     <para>Example</para>
        ///     <para>5(long) , typeof(int) returns 5(int)</para>
        /// </summary>
        public static object NormalizeValueForType(object value, Type targetType)
        {
            var valueConverter = TypeDescriptor.GetConverter(targetType);

            // special case
            if (valueConverter is DoubleConverter)
            {
                if (value is long || value is int)
                {
                    value = value.ToString();
                }
            }

            if (valueConverter is NullableConverter)
            {
                if (value is long)
                {
                    value = value.ToString();
                }
            }

            if (valueConverter.CanConvertFrom(value.GetType()))
            {
                value = valueConverter.ConvertFrom(value);
            }

            return value;
        }
    }
}