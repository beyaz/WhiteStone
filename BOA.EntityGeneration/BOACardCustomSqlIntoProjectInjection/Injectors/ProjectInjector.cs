using BOA.DataFlow;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.DataFlow;
using static BOA.EntityGeneration.DataFlow.CustomSqlExportingData;
using static BOA.EntityGeneration.DataFlow.Data;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    /// <summary>
    ///     The project injector
    /// </summary>
    public class ProjectInjector
    {
        #region Public Methods
        /// <summary>
        ///     Injects the specified profile identifier.
        /// </summary>
        public static void Inject(IDataContext context, string profileId)
        {
            var config   = context.Get(Config);
            var database = context.Get(Database);

            context.Add(CustomSqlExporter.ProfileId,profileId);
            context.FireEvent(CustomSqlExportingEvent.StartedToExportProfileId);

            context.Add(CustomSqlExporter.CustomSqlInfoProject, ProjectCustomSqlInfoDataAccess.GetByProfileId(context));

            context.FireEvent(DataEvent.StartToExportCustomSqlInfoProject);

            context.Remove(CustomSqlExporter.CustomSqlInfoProject);

            context.Remove(CustomSqlExporter.ProfileId);

        }

       
        #endregion
    }
}