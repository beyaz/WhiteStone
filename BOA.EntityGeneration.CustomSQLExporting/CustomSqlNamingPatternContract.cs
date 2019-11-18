using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using static BOA.EntityGeneration.CustomSQLExporting.CustomSqlNamingPatternContract;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting
{
    class CustomSqlNamingPatternContract
    {
        #region Static Fields
        internal static readonly IDataConstant<CustomSqlNamingPatternContract> CustomSqlNamingPattern = DataConstant.Create<CustomSqlNamingPatternContract>(nameof(CustomSqlNamingPattern));
        #endregion

        #region Public Properties
        public string ResultClassName { get; set; }
        public string RepositoryClassName { get; set; }
        public string InputClassName { get; set; }
        
        #endregion
    }

    static class CustomSqlNamingPatternInitializer
    {
        public static void Initialize(IDataContext context)
        {
            var config = context.Get(Config);
            var customSqlInfo = context.Get(CustomSqlInfo);

            var initialValues = new Dictionary<string, string>
            {
                {nameof(ProfileName), context.Get(ProfileName)},
                {"CamelCasedCustomSqlName", customSqlInfo.Name.ToContractName()}
            };

            var dictionary = ConfigurationDictionaryCompiler.Compile(config.CustomSqlNamingPattern, initialValues);

            context.Add(CustomSqlNamingPattern, new CustomSqlNamingPatternContract
            {
                ResultClassName = dictionary[nameof(CustomSqlNamingPatternContract.ResultClassName)],
                RepositoryClassName = dictionary[nameof(CustomSqlNamingPatternContract.RepositoryClassName)],
                InputClassName = dictionary[nameof(CustomSqlNamingPatternContract.InputClassName)]
            });
        }

        public static void Remove(IDataContext context)
        {
            context.Remove(CustomSqlNamingPattern);
        }
    }
}