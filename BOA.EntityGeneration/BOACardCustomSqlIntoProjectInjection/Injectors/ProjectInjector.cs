using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.TfsAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    public class ProjectInjector
    {  [Inject]
        public Tracer Tracer { get; set; }
        #region Public Properties
        [Inject]
        public AllInOneForBusinessDll AllInOneForBusinessDll { get; set; }

        [Inject]
        public AllInOneForTypeDll AllInOneForTypeDll { get; set; }

        [Inject]
        public ProjectCustomSqlInfoDataAccess ProjectCustomSqlInfoDataAccess { get; set; }

        [Inject]
        public FileAccess FileAccess { get; set; }
        #endregion

        #region Public Methods
        public void Inject(string profileId)
        {
            Inject(ProjectCustomSqlInfoDataAccess.GetByProfileId(profileId));
        }
        #endregion

        #region Methods
        void Inject(ProjectCustomSqlInfo data)
        {
            
            

            for (var i = 0; i < data.CustomSqlInfoList.Count; i++)
            {
                data.CustomSqlInfoList[i].SwitchCaseIndex = i;
            }

            Tracer.CustomSqlGenerationOfProfileIdProcess.Text = "Generating types...";
            var typeCode     = AllInOneForTypeDll.GetCode(data);

            Tracer.CustomSqlGenerationOfProfileIdProcess.Text = "Generating business...";
            var businessCode = AllInOneForBusinessDll.GetCode(data);

            Tracer.CustomSqlGenerationOfProfileIdProcess.Text = "Writing to files...";

            FileAccess.WriteAllText(data.TypesProjectPath + "\\Generated\\CustomSql.cs", typeCode);
            FileAccess.WriteAllText(data.BusinessProjectPath + "\\Generated\\CustomSql.cs", businessCode);
        }
        #endregion
    }
}