using System;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The date time extensions
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Public Methods
        /// <summary>
        ///     Equalses the kind of the ignore.
        /// </summary>
        public static bool EqualsIgnoreKind(this DateTime left, DateTime right)
        {
            return left.Year == right.Year &&
                   left.Month == right.Month &&
                   left.Day == right.Day &&
                   left.Hour == right.Hour &&
                   left.Minute == right.Minute &&
                   left.Millisecond == right.Millisecond;
        }

        /// <summary>
        ///     Truncates the milliseconds.
        /// </summary>
        public static DateTime TruncateMilliseconds(this DateTime dateTime)
        {
            return dateTime.AddMilliseconds(-dateTime.Millisecond);
        }
        #endregion
    }
}