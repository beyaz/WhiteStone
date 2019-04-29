﻿using System;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting;

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

            BOACardDatabaseExporter.Export(args[0].Trim());
        }
        #endregion
    }
}