using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Exporters
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
            BoaRepositoryFileExporter.AttachEvents(context);

            
        }
    }
}