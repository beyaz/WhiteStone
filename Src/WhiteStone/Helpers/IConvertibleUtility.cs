using System;
using System.Globalization;
using System.Threading;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Utility methods for primitive values casting operations.
    /// </summary>
    public static class IConvertibleUtility
    {
        /// <summary>
        ///     <para>Casts given value to 'TTargetType'</para>
        ///     <para>if value is null or empty then returns <code>default(TTargetType)</code></para>
        ///     <para><code>TTargetType</code> can be <code>Nullable</code> type.</para>
        /// </summary>
        /// <typeparam name="TTargetType">Target type</typeparam>
        /// <param name="value">
        ///     An object that implements the <see cref="System.IConvertible" /> interface. Can be nullable value
        ///     or enum value
        /// </param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        public static TTargetType To<TTargetType>(this IConvertible value, IFormatProvider provider)
        {
            if (value == null)
            {
                return default(TTargetType);
            }

            if (typeof (TTargetType) != typeof (string))
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

            var typeFromHandle = typeof (TTargetType);

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
        ///     <para>Casts given value to 'TTargetType'</para>
        ///     <para>if value is null or empty then returns <code>default(TTargetType)</code></para>
        ///     <para><code>TTargetType</code> can be <code>Nullable</code> type.</para>
        ///     <para>Uses <code>CultureInfo.InvariantCulture</code></para>
        /// </summary>
        /// <typeparam name="TTargetType"></typeparam>
        /// <param name="value">An object that implements the <see cref="System.IConvertible" /> interface. </param>
        public static TTargetType To<TTargetType>(this IConvertible value)
        {
            return To<TTargetType>(value, CultureInfo.InvariantCulture);
        }

        static TargetType DoCasting<TargetType>(object value)
        {
            try
            {
                return (TargetType) value;
            }
            catch (Exception ex)
            {
                var message = string.Format(Thread.CurrentThread.CurrentCulture, "'{0}' not casted to '{1}' .Exception:'{2}'", value, typeof (TargetType), ex.Message);
                throw new InvalidCastException(message);
            }
        }
    }
}