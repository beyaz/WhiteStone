using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

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
            var config   = context.Get(Data.Config);
            var database = context.Get(Data.Database);

            context.Add(Data.CustomSqlInfoProject, ProjectCustomSqlInfoDataAccess.GetByProfileId(database, config, profileId));

            context.FireEvent(DataEvent.StartToExportCustomSqlInfoProject);

            context.Remove(Data.CustomSqlInfoProject);
        }

        /// <summary>
        ///     Injects the specified data.
        /// </summary>
        public static void Inject(IDataContext context)
        {
            var data       = context.Get(Data.CustomSqlInfoProject);
            var progress   = context.Get(Data.CustomSqlGenerationOfProfileIdProcess);
            var fileAccess = context.Get(Data.FileAccess);

            progress.Text = "Generating types...";
            var typeCode = AllInOneForTypeDll.GetCode(data);

            progress.Text = "Generating business...";
            var businessCode = AllInOneForBusinessDll.GetCode(data);

            progress.Text = "Writing to files...";

            fileAccess.WriteAllText(data.TypesProjectPath + "\\Generated\\CustomSql.cs", typeCode);
            fileAccess.WriteAllText(data.BusinessProjectPath + "\\Generated\\CustomSql.cs", businessCode);
        }
        #endregion
    }
}