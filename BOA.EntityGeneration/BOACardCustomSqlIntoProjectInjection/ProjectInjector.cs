using System.IO;
using Ninject;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class ProjectInjector
    {
        [Inject]
        public AllInOneForTypeDll AllInOneForTypeDll { get; set; }

        [Inject]
        public AllInOneForBusinessDll AllInOneForBusinessDll { get; set; }
        [Inject]
        public DataAccess DataAccess { get; set; }

        public void Inject(string profileId)
        {
            Inject(DataAccess.GetByProfileId(profileId));
        }
        void Inject(ProjectCustomSqlInfo data)
        {
            var typeCode     = AllInOneForTypeDll.GetCode(data);
            var businessCode = AllInOneForBusinessDll.GetCode(data);

            File.WriteAllText(data.TypesProjectPath+"\\CustomSql.cs",typeCode);
            File.WriteAllText(data.BusinessProjectPath+"\\CustomSql.cs",businessCode);

        }
    }
}