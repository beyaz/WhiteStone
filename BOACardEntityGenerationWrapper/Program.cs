using System;
using System.Threading;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;
using BOA.TfsAccess;

namespace BOACardEntityGenerationWrapper
{
    public class Program
    {
        #region Methods
        static void Main(string[] args)
        {
            // args = new[] {"BKM"};

            if (args == null || args.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            var schemaName = args[0].Trim();

            using (var kernel = new Kernel())
            {
                kernel.Bind<FileAccess>().To<FileAccessWithAutoCheckIn>();
                BOACardDatabaseExporter.Export(kernel, schemaName);
            }

            Thread.Sleep(3000);
        }
        #endregion
    }
}