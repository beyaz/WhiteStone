using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public class CustomSqlDataContextCreator : DataContextCreatorBase
    {
        public IDataContext Create()
        {
            var context = new DataContext();

            InitializeServices(context);

            AttachEvents(context);
            

            return context;
        }

        protected virtual void AttachEvents(IDataContext context)
        {

            TypeFileExporter.AttachEvents(context);
            SharedFileExporter.AttachEvents(context);
            BoaRepositoryFileExporter.AttachEvents(context);
            TypesProjectExporter.AttachEvents(context);

            
        }
    }
}