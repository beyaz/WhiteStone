using System;
using System.IO;
using System.Reflection;

namespace WhiteStone.Helpers
{
    static partial class ReflectionUtility
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
            readonly string _binDirectory;

            internal AssemblyResolver(string binDirectory)
            {
                _binDirectory = binDirectory;
            }


            internal Assembly Resolve(object sender, ResolveEventArgs args)
            {
                var assemblyPath = Path.Combine(_binDirectory, new AssemblyName(args.Name).Name + ".dll");
                if (!File.Exists(assemblyPath))
                {
                    return null;
                }
                return Assembly.LoadFrom(assemblyPath);
            }
        }
    }
}