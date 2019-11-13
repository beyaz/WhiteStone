using System.Linq;
using ___Company___.DataFlow;
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
        public string GetCode(IDataContext context)
        {
            context.Add(Data.TypesFileOutput, new PaddedStringBuilder());

            Write(context);

            var code = context.Get(Data.TypesFileOutput).ToString();

            context.Remove(Data.TypesFileOutput);

            return code;
        }
        #endregion

        #region Methods
        void Write(IDataContext context)
        {
            var sb         = context.Get(Data.TypesFileOutput);
            var schemaName = context.Get(Data.SchemaName);

            var progress = context.TryGet(Data.SchemaGenerationProcess);

            var isFirst = true;

            var items = DataPreparer.Prepare(schemaName);

            foreach (var tableInfo in items)
            {
                context.Add(Data.TableInfo, tableInfo);

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

                context.Remove(Data.TableInfo);
            }

            if (items.Any())
            {
                sb.EndNamespace();
            }
        }
        #endregion
    }
}