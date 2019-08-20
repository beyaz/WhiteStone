using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The zip assembly resolver
    /// </summary>
    public class ZipAssemblyResolver
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="ZipAssemblyResolver" /> class.
        /// </summary>
        public ZipAssemblyResolver(string zipFilePath)
        {
            ZipFilePath = zipFilePath;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the zip file path.
        /// </summary>
        public string ZipFilePath { get; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Attaches to current domain.
        /// </summary>
        public void AttachToCurrentDomain()
        {
            AppDomain.CurrentDomain.AssemblyResolve += DomainAssemblyResolve;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Domains the assembly resolve.
        /// </summary>
        Assembly DomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name  = args.Name;
            var index = name.IndexOf(',');
            if (index > 0)
            {
                name = name.Substring(0, index);
            }

            Log.Push(args.Name);

            var zipEntryName = string.Format(@"{0}.dll", name);

            if (ZipHelper.HasEntry(ZipFilePath, zipEntryName))
            {
                var outputPath = Path.GetTempPath() + Path.DirectorySeparatorChar + zipEntryName;

                ZipHelper.ExtractFromZipFile(ZipFilePath, null, new Dictionary<string, string> {{zipEntryName, outputPath}});

                return Assembly.LoadFile(outputPath);
            }

            return null;
        }
        #endregion
    }
}