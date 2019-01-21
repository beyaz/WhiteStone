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
        ///     Converts to.
        /// </summary>
        public static object ConvertTo(this IConvertible value, Type targetType, IFormatProvider provider)
        {
            
            if (value == null)
            {
                return targetType.GetDefaultValue();
            }

            if (targetType != typeof(string))
            {
                var valueAsString = value as string;
                if (valueAsString != null && string.IsNullOrWhiteSpace(valueAsString))
                {
                    return targetType.GetDefaultValue();
                }
            }

            if (value.GetTypeCode() == Type.GetTypeCode(targetType) ||
                value.GetType().IsAssignableFrom(targetType))
            {
                return value;
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType);

            if (underlyingType != null)
            {
                if (DBNull.Value.Equals(value))
                {
                    return targetType.GetDefaultValue();
                }

                targetType = underlyingType;
            }

            return Convert.ChangeType(value, targetType, provider);
        }

        /// <summary>
        ///     To the specified provider.
        /// </summary>
        public static TTargetType To<TTargetType>(this IConvertible value, IFormatProvider provider)
        {
            return DoCasting<TTargetType>(ConvertTo(value, typeof(TTargetType), provider));
        }

        /// <summary>
        ///     To the specified value.
        /// </summary>
        public static TTargetType To<TTargetType>(this IConvertible value)
        {
            return To<TTargetType>(value, CultureInfo.InvariantCulture);
        }

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

        #region Methods
        /// <summary>
        ///     Does the casting.
        /// </summary>
        static TTargetType DoCasting<TTargetType>(object value)
        {
            try
            {
                return (TTargetType) value;
            }
            catch (Exception ex)
            {
                var message = string.Format(CultureInfo.CurrentCulture, "'{0}' not casted to '{1}' .Exception:'{2}'", value, typeof(TTargetType), ex.Message);
                throw new InvalidCastException(message);
            }
        }
        #endregion
    }
}