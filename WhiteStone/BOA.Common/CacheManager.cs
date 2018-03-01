using System;
using System.Collections.Generic;

namespace BOA.Common
{
    class CacheManager
    {
        #region Fields
        readonly Func<object> CreateNewItem;
        readonly Func<string, TimeSpan?> GetDuration;

        Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        #endregion

        #region Constructors
        public CacheManager(Func<object> createNewItem, Func<string, TimeSpan?> getDuration)
        {
            CreateNewItem = createNewItem;
            GetDuration = getDuration;
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Clears all cache.
        /// </summary>
        public void ClearCache()
        {
            _cache = new Dictionary<string, CacheItem>();
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        public object GetValue(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            CacheItem value = null;

            _cache.TryGetValue(key, out value);

            if (value != null)
            {
                if (value.Duration == null)
                {
                    return value.Value;
                }

                var isActive = DateTime.Now - value.CreationTime <= value.Duration.Value;

                if (isActive)
                {
                    return value.Value;
                }
            }

            value = new CacheItem(CreateNewItem());

            _cache.Add(key, value);

            value.Duration = GetDuration(key + ".CacheRefreshPeriodInTimeSpanFormat");

            return value.Value;
        }
        #endregion

        class CacheItem
        {
            #region Constructors
            internal CacheItem(object value)
            {
                Value = value;
                CreationTime = DateTime.Now;
            }
            #endregion

            #region Public Properties
            public DateTime CreationTime { get; private set; }

            public TimeSpan? Duration { get; set; }
            public object Value { get; private set; }
            #endregion
        }
    }
}