using System.Linq;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using Ninject;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.AllInOne
{
    public class AllTypeClassesInOne
    {
        #region Public Properties
        [Inject]
        public SchemaExporterDataPreparer DataPreparer { get; set; }



      
        #endregion

        #region Public Methods
        public string GetCode(string schemaName)
        {
            Context.Add(Data.TypesFileOutput, new PaddedStringBuilder());

            Write();

            var code = Context.Get(Data.TypesFileOutput).ToString();

            Context.Remove(Data.TypesFileOutput);

            return code;
        }
        #endregion

        #region Methods
        void Write()
        {
            var sb         = Context.Get(Data.TypesFileOutput);
            var schemaName = Context.Get(Data.SchemaName);

            var progress = Context.TryGet(Data.SchemaGenerationProcess);

            var isFirst = true;

            var items = DataPreparer.Prepare(schemaName);

            foreach (var tableInfo in items)
            {
                Context.Add(Data.TableInfo, tableInfo);

                if (isFirst)
                {
                    GeneratorOfTypeClass.WriteUsingList();
                    sb.AppendLine();

                    sb.BeginNamespace(NamingHelper.GetTypeClassNamespace(schemaName));

                    isFirst = false;
                }

                progress.Text = $"Generating Type class for {tableInfo.TableName}";

                sb.AppendLine();
                GeneratorOfTypeClass.WriteClass();

                Context.Remove(Data.TableInfo);
            }

            if (items.Any())
            {
                sb.EndNamespace();
            }
        }
        #endregion
    }
}