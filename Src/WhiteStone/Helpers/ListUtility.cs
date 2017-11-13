using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Extension methods for List class.
    /// </summary>
    public static class ListUtility
    {
        /// <summary>
        ///     Swaps two values at given indexes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static IList<T> Swap<T>
        (this IList<T> list,
         int source,
         int target)
        {
            if (source != target)
            {
                var temp = list[source];
                list[source] = list[target];
                list[target] = temp;
            }

            return list;
        }

        /// <summary>
        ///     Splits list elements to smaller element list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="sliceSize"></param>
        /// <returns></returns>
        public static List<T>[] Split<T>(this IList<T> items, int sliceSize)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            if (sliceSize < 0)
            {
                throw new ArgumentOutOfRangeException("sliceSize:" + sliceSize);
            }

            var list = new List<List<T>>();
            for (var index = 0; index < items.Count; index += sliceSize)
            {
                var count = Math.Min(sliceSize, items.Count - index);

                var itemsAsList = items as  List<T>;
                if (itemsAsList != null)
                {
                    list.Add(itemsAsList.GetRange(index, count));
                }
                else
                {
                    var array = new T[count];
                    for (var jIndex = 0; jIndex < count; jIndex++)
                    {
                        array[jIndex] = items[index + jIndex];
                    }
                    list.Add(array.ToList());
                }

                
                
            }
            return list.ToArray();
        }

        /// <summary>
        ///     Indicates whether the specified collection is null or [collection.Count] is zero.
        /// </summary>
        public static bool IsNullOrEmpty(this ICollection value)
        {
            return value == null || value.Count == 0;
        }
    }
}