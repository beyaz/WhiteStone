using System;
using System.Collections.Concurrent;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Defines the Service Manager.
    /// </summary>
    public static class SM
    {
        #region Static Fields
        /// <summary>
        ///     The service hash
        /// </summary>
        static readonly ConcurrentDictionary<string, object> _serviceHash = new ConcurrentDictionary<string, object>();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets this instance.
        /// </summary>
        public static T Get<T>()
        {
            return (T) GetService(typeof(T).FullName);
        }

        /// <summary>
        ///     Gets the service.
        /// </summary>
        public static object GetService(string serviceType)
        {
            if (string.IsNullOrWhiteSpace(serviceType))
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            if (_serviceHash.ContainsKey(serviceType))
            {
                return _serviceHash[serviceType];
            }

            return null;
        }

        /// <summary>
        ///     Determines whether this instance has value.
        /// </summary>
        public static bool HasValue<T>()
        {
            return Get<T>() != null;
        }

        /// <summary>
        ///     Sets the specified value.
        /// </summary>
        public static void Set<T>(T value)
        {
            SetService(typeof(T).FullName, value);
        }

        /// <summary>
        ///     Sets service instance
        /// </summary>
        public static void SetService(string serviceName, object value)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _serviceHash[serviceName] = value;
        }
        #endregion
    }
}