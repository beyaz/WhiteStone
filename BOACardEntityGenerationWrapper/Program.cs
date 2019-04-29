using System;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;

namespace BOACardEntityGenerationWrapper
{
    class Program
    {
        #region Methods
        static void Main(string[] args)
        {
            if (args?.Length == 0)
            {
                throw new ArgumentException(nameof(args));
            }

            BOACardDatabaseExporter.Export(args[0].Trim());
        }
        #endregion
    }
}