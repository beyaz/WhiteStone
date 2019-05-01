using System.IO;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class ProjectInjector
    {
        #region Public Properties
        [Inject]
        public AllInOneForBusinessDll AllInOneForBusinessDll { get; set; }

        [Inject]
        public AllInOneForTypeDll AllInOneForTypeDll { get; set; }

        [Inject]
        public DataAccess DataAccess { get; set; }
        #endregion

        #region Public Methods
        

        public void Inject(string profileId)
        {
            Inject(DataAccess.GetByProfileId(profileId));
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

            File.WriteAllText(data.TypesProjectPath + "\\Generated\\CustomSql.cs", typeCode);
            File.WriteAllText(data.BusinessProjectPath + "\\Generated\\CustomSql.cs", businessCode);
        }
        #endregion
    }
}