using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.SharedClasses;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne
{
    /// <summary>
    ///     All business classes in one
    /// </summary>
    public class AllBusinessClassesInOne
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the data preparer.
        /// </summary>
        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }

        /// <summary>
        ///     Gets or sets the generator of business class.
        /// </summary>
        [Inject]
        public GeneratorOfBusinessClass GeneratorOfBusinessClass { get; set; }

        /// <summary>
        ///     Gets or sets the naming helper.
        /// </summary>
        [Inject]
        public NamingHelper NamingHelper { get; set; }

      
        /// <summary>
        ///     Gets or sets the configuration.
        /// </summary>
        [Inject]
        public Config Config { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Gets the code.
        /// </summary>
        public string GetCode(string schemaName)
        {
            var sb = new PaddedStringBuilder();

            Write(sb, schemaName);

            return sb.ToString();
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Utilities the class.
        /// </summary>
        static void UtilClass(PaddedStringBuilder sb, Config config)
        {

            var path = Path.GetDirectoryName(typeof(AllBusinessClassesInOne).Assembly.Location) + Path.DirectorySeparatorChar + "EmbeddedUtilizationClassForDao.txt";

            var code = FileHelper.ReadFile(path);

            code = code.Replace("{DatabaseEnumName}", config.DatabaseEnumName);
            sb.AppendAll(code.Trim());
            sb.AppendLine();
        }

        /// <summary>
        ///     Writes the specified sb.
        /// </summary>
        void Write(PaddedStringBuilder sb, string schemaName)
        {

            var progress = Context.Get(Data.SchemaGenerationProcess);

            var items = DataPreparer.Prepare(schemaName);

            if (items.Count == 0)
            {
                throw new NotImplementedException(schemaName);
            }

            GeneratorOfBusinessClass.WriteUsingList(sb, items.First());
            Context.Get(Data.SharedRepositoryClassOutput).BeginNamespace(NamingHelper.GetSharedRepositoryClassNamespace(schemaName));
            SqlInfoClassWriter.Write(Context.Get(Data.SharedRepositoryClassOutput));

            sb.AppendLine();
            sb.BeginNamespace(NamingHelper.GetBusinessClassNamespace(schemaName));
            ObjectHelperSqlUtilClassWriter.Write(sb);

            UtilClass(sb,Config);

            progress.Total   = items.Count;
            progress.Current = 0;

            foreach (var tableInfo in items)
            {
                progress.Text = $"Generating business class: {tableInfo.TableName}";

                progress.Current++;

                sb.AppendLine();
                GeneratorOfBusinessClass.WriteClass(sb, tableInfo);
            }

            sb.EndNamespace();  

            Context.Get<PaddedStringBuilder>(Data.SharedRepositoryClassOutput).EndNamespace();
        }
        #endregion
    }
}