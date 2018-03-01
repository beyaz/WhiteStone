using System;
using System.Collections.Generic;
using System.Linq;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Represents helper methods for type <see cref="IEnumerable{T}" />
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        ///     Runs an action for each item in enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            var forEach = source as IList<T> ?? source.ToList();

            foreach (var element in forEach)
            {
                action(element);
            }

            return forEach;
        }
    }
}