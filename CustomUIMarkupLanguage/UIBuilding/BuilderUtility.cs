using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     Defines the builder utility.
    /// </summary>
    static class BuilderUtility
    {
        #region Public Methods
        /// <summary>
        ///     Converts to binding.
        /// </summary>
        public static Binding ConvertToBinding(this BindingInfoContract bindingInfoContract, TypeFinder typeFinder,string bindingPathPrefix)
        {
            var binding = new Binding
            {
                Mode = bindingInfoContract.BindingMode,
                Path = new PropertyPath(bindingPathPrefix+bindingInfoContract.SourcePath)
            };

            if (bindingInfoContract.ConverterTypeFullName != null)
            {
                var converterType = typeFinder.GetType(bindingInfoContract.ConverterTypeFullName);
                binding.Converter = (IValueConverter) Activator.CreateInstance(converterType);
            }

            return binding;
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

        /// <summary>
        ///     Searches the dependency property in view.
        /// </summary>
        public static DependencyProperty SearchDependencyProperty(string dpFullName, TypeFinder typeFinder)
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

            return (DependencyProperty) fieldInfo.GetValue(null);
        }
        #endregion
    }
}