using System;

namespace WhiteStone.Common
{
    /// <summary>
    ///     Utility pair for holding pair values.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public sealed class Pair<TKey, TValue>
    {
        /// <summary>
        ///     Utility pair for holding pair values.
        /// </summary>
        public Pair()
        {
        }

        /// <summary>
        ///     Utility pair for holding pair values.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        ///     Key
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        ///     Value
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        ///     Represents string value of pair.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + Key + "," + Value + "]";
        }
    }
}