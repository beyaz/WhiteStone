using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Resources;

namespace BOA.EntityGeneration.UI.Container.Bootstrapper.Infrastructure
{
    /// <summary>
    ///     The embedded zipped assembly resolver data
    /// </summary>
    public sealed class EmbeddedZippedAssemblyResolverData
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the application domain.
        /// </summary>
        public AppDomain AppDomain { get; set; }

        /// <summary>
        ///     Gets or sets the assembly.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        ///     Gets or sets the embedded resource path in assembly.
        /// </summary>
        public string EmbeddedResourcePathInAssembly { get; set; }

        /// <summary>
        ///     Gets the out dir.
        /// </summary>
        public string OutDir => Path.GetTempPath() + Path.GetFileNameWithoutExtension(EmbeddedResourcePathInAssembly) + Path.DirectorySeparatorChar;

        /// <summary>
        ///     Gets the target file path.
        /// </summary>
        public string TargetFilePath => OutDir + EmbeddedResourcePathInAssembly;
        #endregion
    }

    /// <summary>
    ///     The embedded zipped assembly resolver
    /// </summary>
    public static class EmbeddedZippedAssemblyResolver
    {
        #region Public Methods
        /// <summary>
        ///     Attaches the specified data.
        /// </summary>
        public static void Attach(EmbeddedZippedAssemblyResolverData data)
        {
            Extract(data);

            if (data.EmbeddedResourcePathInAssembly.EndsWith(".zip"))
            {
                ZipFile.ExtractToDirectory(data.TargetFilePath, data.OutDir);    
            }
            

            data.AppDomain.AssemblyResolve += new AssemblyResolver(data.OutDir).Resolve;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Extracts the specified name space.
        /// </summary>
        static void Extract(EmbeddedZippedAssemblyResolverData data)
        {
            var assembly = data.Assembly;

            var filePath = data.EmbeddedResourcePathInAssembly;

            using (var manifestResourceStream = assembly.GetManifestResourceStream(filePath))
            {
                if (manifestResourceStream == null)
                {
                    throw new MissingManifestResourceException(filePath);
                }

                var targetFilePath = data.TargetFilePath;

                if (File.Exists(targetFilePath))
                {
                    File.Delete(targetFilePath);
                }

                var targetDirectory = Path.GetDirectoryName(targetFilePath);
                if (targetDirectory == null)
                {
                    throw new ArgumentNullException(nameof(targetDirectory));
                }

                if (File.Exists(targetDirectory))
                {
                    File.Delete(targetDirectory);
                }

                if (Directory.Exists(targetDirectory))
                {
                    Directory.Delete(targetDirectory, true);
                }

                Directory.CreateDirectory(targetDirectory);

                using (var fs = new FileStream(targetFilePath, FileMode.OpenOrCreate))
                {
                    manifestResourceStream.ReadAllWriteToOutput(fs);
                }
            }
        }

        /// <summary>
        ///     Reads all write to output.
        /// </summary>
        static void ReadAllWriteToOutput(this Stream inputStream, Stream outputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            if (outputStream == null)
            {
                throw new ArgumentNullException(nameof(outputStream));
            }

            var array = new byte[2048];
            int count;
            do
            {
                count = inputStream.Read(array, 0, array.Length);

                outputStream.Write(array, 0, count);
            } while (count > 0);
        }
        #endregion

        /// <summary>
        ///     The assembly resolver
        /// </summary>
        class AssemblyResolver
        {
            #region Constructors
            /// <summary>
            ///     Initializes a new instance of the <see cref="AssemblyResolver" /> class.
            /// </summary>
            internal AssemblyResolver(string binDirectory)
            {
                BinDirectory = binDirectory;
            }
            #endregion

            #region Public Properties
            /// <summary>
            ///     Gets the bin directory.
            /// </summary>
            public string BinDirectory { get; }
            #endregion

            #region Public Methods
            /// <summary>
            ///     Resolves the specified sender.
            /// </summary>
            public Assembly Resolve(object sender, ResolveEventArgs args)
            {
                var assemblyPath = Path.Combine(BinDirectory, new AssemblyName(args.Name).Name + ".dll");
                if (!File.Exists(assemblyPath))
                {
                    return null;
                }

                return Assembly.LoadFrom(assemblyPath);
            }
            #endregion
        }
    }
}