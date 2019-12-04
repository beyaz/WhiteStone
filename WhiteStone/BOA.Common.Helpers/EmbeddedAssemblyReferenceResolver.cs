using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The embedded assembly reference resolver
    /// </summary>
    public class EmbeddedAssemblyReferenceResolver
    {
        #region Fields
        /// <summary>
        ///     The application domain
        /// </summary>
        readonly AppDomain appDomain;

        /// <summary>
        ///     The assembly full name
        /// </summary>
        readonly string assemblyFullName;

        /// <summary>
        ///     The located assembly
        /// </summary>
        readonly Assembly locatedAssembly;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbeddedAssemblyReferenceResolver" /> class.
        /// </summary>
        public EmbeddedAssemblyReferenceResolver(AppDomain appDomain, Assembly locatedAssembly, string assemblyFullName)
        {
            this.appDomain        = appDomain;
            this.locatedAssembly  = locatedAssembly;
            this.assemblyFullName = assemblyFullName;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="EmbeddedAssemblyReferenceResolver" /> class.
        /// </summary>
        public EmbeddedAssemblyReferenceResolver(Assembly locatedAssembly, string assemblyFullName) : this(AppDomain.CurrentDomain, locatedAssembly, assemblyFullName)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Resolves the specified assembly full name.
        /// </summary>
        public static void Resolve(string assemblyFullName)
        {
            new EmbeddedAssemblyReferenceResolver(typeof(EmbeddedAssemblyReferenceResolver).Assembly, assemblyFullName).Resolve();
        }

        /// <summary>
        ///     Resolves the specified assembly full names.
        /// </summary>
        public static void Resolve(IEnumerable<string> assemblyFullNames)
        {
            foreach (var assemblyFullName in assemblyFullNames)
            {
                new EmbeddedAssemblyReferenceResolver(typeof(EmbeddedAssemblyReferenceResolver).Assembly, assemblyFullName).Resolve();
            }
        }

        /// <summary>
        ///     Resolves this instance.
        /// </summary>
        public void Resolve()
        {
            appDomain.AssemblyResolve += Resolve;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Reads the resource.
        /// </summary>
        static byte[] ReadResource(Assembly locatedAssembly, string resourceSuffix)
        {
            var matchedResourceNames = locatedAssembly.GetManifestResourceNames().Where(x => x.EndsWith(resourceSuffix)).ToList();

            if (!matchedResourceNames.Any())
            {
                throw new MissingManifestResourceException(resourceSuffix);
            }

            var resourceName = matchedResourceNames[0];
            if (resourceName == null)
            {
                throw new MissingManifestResourceException(resourceSuffix);
            }

            using (var manifestResourceStream = locatedAssembly.GetManifestResourceStream(resourceName))
            {
                if (manifestResourceStream == null)
                {
                    throw new MissingManifestResourceException(resourceName);
                }

                var data = new byte[manifestResourceStream.Length];

                manifestResourceStream.Read(data, 0, data.Length);

                return data;
            }
        }

        /// <summary>
        ///     Resolves the specified sender.
        /// </summary>
        Assembly Resolve(object sender, ResolveEventArgs args)
        {
            // Sample:
            // args.Name => YamlDotNet, Version=8.0.0.0, Culture=neutral, PublicKeyToken=ec19458f3c15af5e
            // new AssemblyName(args.Name).Name => YamlDotNet
            // searchFileName => YamlDotNet.dll

            var searchFileName = new AssemblyName(args.Name).Name + ".dll";
            if (!searchFileName.Equals(assemblyFullName, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return Assembly.Load(ReadResource(locatedAssembly, "." + assemblyFullName));
        }
        #endregion
    }
}