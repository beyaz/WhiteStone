using System;
using System.IO;
using System.Reflection;

namespace BOA.Common.Helpers
{
    static partial class ReflectionHelper
    {
        /// <summary>
        ///     Creates the assembly resolve handler.
        /// </summary>
        /// <param name="binDirectoryPath">The bin directory path.</param>
        /// <returns></returns>
        public static ResolveEventHandler CreateAssemblyResolveHandler(string binDirectoryPath)
        {
            return new AssemblyResolver(binDirectoryPath).Resolve;
        }

        /// <summary>
        ///     Adds the assembly search directory.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="binDirectoryPath">The bin directory path.</param>
        public static void AddAssemblySearchDirectory(this AppDomain domain, string binDirectoryPath)
        {
            domain.AssemblyResolve += CreateAssemblyResolveHandler(binDirectoryPath);
        }

        class AssemblyResolver
        {
            public string BinDirectory { get; }

            internal AssemblyResolver(string binDirectory)
            {
                BinDirectory = binDirectory;
            }


            public Assembly Resolve(object sender, ResolveEventArgs args)
            {
                var assemblyPath = Path.Combine(BinDirectory, new AssemblyName(args.Name).Name + ".dll");
                if (!File.Exists(assemblyPath))
                {
                    return null;
                }
                return Assembly.LoadFrom(assemblyPath);
            }
        }
    }
}