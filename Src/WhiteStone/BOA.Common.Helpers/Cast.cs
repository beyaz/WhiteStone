using System;
using System.Globalization;
using System.Threading;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Utility methods for casting operations
    /// </summary>
    public static class Cast
    {
        #region Public Methods
        /// <summary>
        ///     Casts value to 'TTargetType'
        /// </summary>
        public static TTargetType To<TTargetType>(object value, IFormatProvider provider)
        {
            if (value is TTargetType)
            {
                return (TTargetType) value;
            }

            var convertible = value as IConvertible;
            if (convertible != null)
            {
                return convertible.To<TTargetType>(provider);
            }

            if (value == null)
            {
                return default(TTargetType);
            }
            return DoCasting<TTargetType>(value);
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