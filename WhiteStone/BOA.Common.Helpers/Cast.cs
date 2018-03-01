using System;
using System.Globalization;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for casting operations
    /// </summary>
    public static class Cast
    {
        #region Public Methods
        /// <summary>
        ///     To the specified value.
        /// </summary>
        public static object To(object value, Type targetType, IFormatProvider provider)
        {
            if (value == null)
            {
                return targetType.GetDefaultValue();
            }

            if (value.GetType().IsAssignableFrom(targetType))
            {
                return value;
            }

            var convertible = value as IConvertible;
            if (convertible != null)
            {
                return convertible.ConvertTo(targetType, provider);
            }

            throw new InvalidCastException("@value:" + value + "not convertible to " + targetType.FullName);
        }

        /// <summary>
        ///     Casts value to 'TTargetType'
        /// </summary>
        public static TTargetType To<TTargetType>(object value, IFormatProvider provider)
        {
            return (TTargetType) To(value, typeof(TTargetType), provider);
        }

        /// <summary>
        ///     Casts value to 'TTargetType'
        /// </summary>
        public static TTargetType To<TTargetType>(object value)
        {
            return To<TTargetType>(value, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}