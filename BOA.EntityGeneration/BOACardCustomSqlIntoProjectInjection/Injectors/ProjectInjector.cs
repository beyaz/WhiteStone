using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Model;
using BOA.TfsAccess;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Injectors
{
    public class ProjectInjector
    {
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

            var typeCode     = AllInOneForTypeDll.GetCode(data);
            var businessCode = AllInOneForBusinessDll.GetCode(data);

            FileAccess.WriteAllText(data.TypesProjectPath + "\\Generated\\CustomSql.cs", typeCode);
            FileAccess.WriteAllText(data.BusinessProjectPath + "\\Generated\\CustomSql.cs", businessCode);
        }
        #endregion
    }
}