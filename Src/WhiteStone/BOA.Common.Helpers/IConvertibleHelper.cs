using System;
using System.Globalization;
using System.Threading;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for primitive values casting operations.
    /// </summary>
    public static class IConvertibleHelper
    {
        #region Public Methods
        /// <summary>
        ///     To the specified provider.
        /// </summary>
        public static TTargetType To<TTargetType>(this IConvertible value, IFormatProvider provider)
        {
            if (value == null)
            {
                return default(TTargetType);
            }

            if (typeof(TTargetType) != typeof(string))
            {
                var valueAsString = value as string;
                if (valueAsString != null && string.IsNullOrWhiteSpace(valueAsString))
                {
                    return default(TTargetType);
                }
            }

            if (value is TTargetType)
            {
                return (TTargetType) value;
            }

            var typeFromHandle = typeof(TTargetType);

            var underlyingType = Nullable.GetUnderlyingType(typeFromHandle);

            if (underlyingType != null)
            {
                if (DBNull.Value.Equals(value))
                {
                    return default(TTargetType);
                }

                typeFromHandle = underlyingType;
            }

            return DoCasting<TTargetType>(Convert.ChangeType(value, typeFromHandle, provider));
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
                var message = string.Format(Thread.CurrentThread.CurrentCulture, "'{0}' not casted to '{1}' .Exception:'{2}'", value, typeof(TargetType), ex.Message);
                throw new InvalidCastException(message);
            }
        }
        #endregion
    }
}