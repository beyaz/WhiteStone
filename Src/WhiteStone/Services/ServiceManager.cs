using System;
using System.Collections.Generic;
using System.Reflection;

namespace WhiteStone.Services
{
    /// <summary>
    ///     Manages  service in using in program.
    /// </summary>
    public interface IServiceManager
    {
        /// <summary>
        ///     Gets specific service supports given interface.
        /// </summary>
        T GetService<T>();

        /// <summary>
        ///     Sets service instance
        /// </summary>
        void SetService<T>(T value);
    }

    /// <summary>
    ///     Maintains a list of services that can be shutdown in the reverse order of their initialization.
    ///     Maintains references to the core service implementations.
    /// </summary>
    public class ServiceManager : IServiceProvider, IServiceManager
    {
        static ServiceManager _instance;
        static readonly object lockObject = new object();

        /// <summary>
        ///     Gets unique instance of <see cref="IServiceManager" />
        /// </summary>
        public static IServiceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    // double - checked lock
                    lock (lockObject)
                    {
                        _instance = new ServiceManager();

                        if (_instance != null)
                        {
                            return _instance;
                        }

                        _instance = new ServiceManager();
                    }
                }
                return _instance;
            }
        }

        static string ServiceManagerNamespaceName
        {
            get { return "WhiteStone.Services"; }
        }

        readonly Dictionary<string, object> _serviceHash = new Dictionary<string, object>();

        #region IServiceProvider
        /// <summary>
        ///     Gets a service. Returns null if service is not found.
        /// </summary>
        public object GetService(Type serviceType)
        {
            return _serviceHash[serviceType.FullName];
        }
        #endregion

        /// <summary>
        ///     Gets a service. Returns null if service is not found.
        /// </summary>
        public object GetService(string serviceType)
        {
            if (string.IsNullOrWhiteSpace(serviceType))
            {
                throw new ArgumentNullException("serviceType");
            }

            if (_serviceHash.ContainsKey(serviceType))
            {
                return _serviceHash[serviceType];
            }

            if (TryToSetServiceFromName(serviceType))
            {
                return _serviceHash[serviceType];
            }

            return null;
        }

        bool TryToSetServiceFromName(string serviceType)
        {
            var isAlreadyImplementedInWhiteStoneLibrary = serviceType.StartsWith(ServiceManagerNamespaceName + ".", StringComparison.Ordinal);

            if (!isAlreadyImplementedInWhiteStoneLibrary)
            {
                return false;
            }
            var arr = serviceType.Split('.');

            var last = arr.Length - 1;
            arr[last] = arr[last].Substring(1);

            var targetClassName = string.Join(".", arr);

            var type = Type.GetType(targetClassName);
            if (type == null)
            {
                return false;
            }

            SetService(serviceType, Activator.CreateInstance(type));

            return true;
        }

        /// <summary>
        ///     Gets a service. Returns null if service is not found.
        /// </summary>
        public virtual T GetService<T>()
        {
            return (T) GetService(typeof(T).FullName);
        }

        /// <summary>
        ///     Sets service instance
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetService<T>(T value)
        {
            SetService(typeof(T).FullName, value);
        }

        /// <summary>
        ///     Sets service instance
        /// </summary>
        public virtual void SetService(string serviceName, object value)
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException("serviceName");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            _serviceHash[serviceName] = value;
        }

        /// <summary>
        ///     Loads from json string.
        /// </summary>
        /// <param name="serviceInfoJsonArray">The service information json array.</param>
        public void LoadFromJsonString(string serviceInfoJsonArray)
        {
            var arr = new JsonSerializer().Deserialize<ServiceInfo[]>(serviceInfoJsonArray);
            foreach (var serviceInfo in arr)
            {
                var assembly = Assembly.Load(serviceInfo.AssemblyName);
                var implementationType = assembly.GetType(serviceInfo.ImplementationTypeName, true);
                SetService(serviceInfo.InterfaceName, Activator.CreateInstance(implementationType));
            }
        }
    }

    /// <summary>
    ///     Defines the service information.
    /// </summary>
    [Serializable]
    public class ServiceInfo
    {
        /// <summary>
        ///     Gets or sets the name of the interface.
        /// </summary>
        public string InterfaceName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the implementation type.
        /// </summary>
        public string ImplementationTypeName { get; set; }

        /// <summary>
        ///     Gets or sets the name of the assembly.
        /// </summary>
        public string AssemblyName { get; set; }
    }
}