﻿using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using static BOA.EntityGeneration.CustomSQLExporting.CustomSqlNamingPatternContract;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    class CustomSqlNamingPatternContract
    {
        #region Static Fields
        
        #endregion

        #region Public Properties
        public string InputClassName             { get; set; }
        public string ReferencedEntityAccessPath { get; set; }
        public string RepositoryClassName        { get; set; }
        public string ResultClassName            { get; set; }
        public string ReferencedEntityAssemblyPath { get; set; }
        public string ReferencedEntityReaderMethodPathPath { get; set; }
        public string ReferencedRepositoryAssemblyPath { get; set; }
        #endregion
    }

    static class CustomSqlNamingPatternInitializer
    {
        #region Public Methods
        public static void Initialize(IDataContext context)
        {
            var config        = Data.Config[context];
            var customSqlInfo = Data.CustomSqlInfo[context];
            var profileName   = Data.ProfileName[context];

            var initialValues = new Dictionary<string, string>
            {
                {nameof(Data.ProfileName), profileName},
                {"CamelCasedCustomSqlName", customSqlInfo.Name.ToContractName()}
            };

            var entityReferencedResultColumn = customSqlInfo.ResultColumns.FirstOrDefault(x => x.IsReferenceToEntity);

            if (entityReferencedResultColumn != null)
            {
                initialValues[nameof(customSqlInfo.SchemaName)] = customSqlInfo.SchemaName;
                initialValues["CamelCasedResultName"]           = entityReferencedResultColumn.Name.ToContractName();
            }

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.CustomSqlNamingPattern, initialValues);

            context.Add(Data.CustomSqlNamingPattern, new CustomSqlNamingPatternContract
            {
                ResultClassName            = dictionary[nameof(CustomSqlNamingPatternContract.ResultClassName)],
                RepositoryClassName        = dictionary[nameof(CustomSqlNamingPatternContract.RepositoryClassName)],
                InputClassName             = dictionary[nameof(CustomSqlNamingPatternContract.InputClassName)],
                ReferencedEntityAccessPath = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedEntityAccessPath)],
                ReferencedEntityAssemblyPath = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedEntityAssemblyPath)],
                ReferencedEntityReaderMethodPathPath = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedEntityReaderMethodPathPath)],
                ReferencedRepositoryAssemblyPath = dictionary[nameof(CustomSqlNamingPatternContract.ReferencedRepositoryAssemblyPath)],
            });
        }
        #endregion
    }
}