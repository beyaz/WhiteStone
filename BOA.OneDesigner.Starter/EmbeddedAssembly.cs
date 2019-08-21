using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BOA.OneDesigner.Starter
{
    /// <summary>
    ///     The embedded assembly
    /// </summary>
    public static class EmbeddedAssembly
    {
        #region Static Fields
        /// <summary>
        ///     The location map
        /// </summary>
        static readonly Dictionary<string, string> LocationMap = new Dictionary<string, string>();
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the assembly.
        /// </summary>
        public static void AddAssembly(string assemblyName, string locatedAssemblyName, string resourceName)
        {
            LocationMap[assemblyName] = $"{locatedAssemblyName},{resourceName}";
        }

        /// <summary>
        ///     Attaches to current domain.
        /// </summary>
        public static void AttachToCurrentDomain()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= DomainAssemblyResolve;
            AppDomain.CurrentDomain.AssemblyResolve += DomainAssemblyResolve;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Domains the assembly resolve.
        /// </summary>
        static Assembly DomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name  = args.Name;
            var index = name.IndexOf(',');
            if (index > 0)
            {
                name = name.Substring(0, index);
            }

            var assemblyName = string.Format(@"{0}.dll", name);

            if (!LocationMap.ContainsKey(assemblyName))
            {
                assemblyName = string.Format(@"{0}.exe", name);
                if (!LocationMap.ContainsKey(assemblyName))
                {
                    return null;
                }
            }

            var location            = LocationMap[assemblyName];
            var locatedAssemblyName = location.Split(',')[0];
            var resourceName        = location.Split(',')[1];

            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.ManifestModule.Name == locatedAssemblyName);

            if (assembly == null)
            {
                assembly = Assembly.Load(locatedAssemblyName);
            }

            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new Exception(resourceName + " is not found in Embedded Resources.");
                }

                var rawAssembly = new byte[(int) resourceStream.Length];

                resourceStream.Read(rawAssembly, 0, (int) resourceStream.Length);

                return Assembly.Load(rawAssembly);
            }
        }
        #endregion
    }
}