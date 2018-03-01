using System;
using System.Globalization;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for primitive values casting operations.
    /// </summary>
    public static class IConvertibleHelper
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
        #endregion

        #region Methods
        /// <summary>
        ///     Does the casting.
        /// </summary>
        static TargetType DoCasting<TargetType>(object value)
        {
            try
            {
                return (TargetType) value;
            }
            catch (Exception ex)
            {
                var message = string.Format(CultureInfo.CurrentCulture, "'{0}' not casted to '{1}' .Exception:'{2}'", value, typeof(TargetType), ex.Message);
                throw new InvalidCastException(message);
            }
        }
        #endregion
    }
}