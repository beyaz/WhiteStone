using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.DataAccess;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    public static class CustomSqlExporter
    {

        public static void Export(IDataContext context, string profileId)
        {
            context.Add(ProfileId,profileId);
            context.Add(ProcessedCustomSqlInfoListInProfile,new List<ICustomSqlInfo>());

            InitializeProfileInfo(context);
            ProcessCustomSQLsInProfile(context);

            context.Remove(ProcessedCustomSqlInfoListInProfile);
            context.Remove(ProfileId);

        }


        #region Static Fields
        public static readonly IEvent ProfileInfoIsAvailable = new Event {Name = nameof(ProfileInfoIsAvailable)};
        public static readonly IEvent CustomSqlInfoIsAvailable = new Event {Name = nameof(CustomSqlInfoIsAvailable)};

        public static readonly IEvent StartedToExportObjectId  = new Event {Name = nameof(StartedToExportObjectId)};
        public static readonly IEvent StartedToExportProfileId = new Event {Name = nameof(StartedToExportProfileId)};
        #endregion

       

        #region Output Strings
        public static readonly IDataConstant<PaddedStringBuilder> SharedDalFile = DataConstant.Create<PaddedStringBuilder>(nameof(SharedDalFile));
        public static readonly IDataConstant<PaddedStringBuilder> BoaDalFile    = DataConstant.Create<PaddedStringBuilder>(nameof(BoaDalFile));
        #endregion

        #region Data

        public static readonly IDataConstant<List<ICustomSqlInfo>> ProcessedCustomSqlInfoListInProfile = DataConstant.Create<List<ICustomSqlInfo>>();

        public static readonly IDataConstant<string>                ProfileId            = DataConstant.Create<string>(nameof(ProfileId));
        public static readonly IDataConstant<ICustomSqlInfo>        CustomSqlInfo        = DataConstant.Create<ICustomSqlInfo>();

        public static readonly IDataConstant<ICustomSqlProfileInfo> CustomSqlProfileInfo = DataConstant.Create<ICustomSqlProfileInfo>();
        
        #endregion



        static void InitializeProfileInfo(IDataContext context)
        {
            var database  = context.Get(Data.Database);
            var profileId = context.Get(ProfileId);

            var profileInfo = ProjectCustomSqlInfoDataAccess.GetByProfileIdFromDatabase(database, profileId);

            context.Add(CustomSqlProfileInfo,profileInfo);

            context.FireEvent(ProfileInfoIsAvailable);
            
        }

        static void ProcessCustomSQLsInProfile(IDataContext context)
        {
            var profileInfo = context.Get(CustomSqlProfileInfo);

            var config    = context.Get(Data.Config);
            var database  = context.Get(Data.Database);
            var profileId = context.Get(ProfileId);


            var switchCaseIndex = 0;
            foreach (var objectId in profileInfo.ObjectIdList)
            {
                var customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileId, objectId, config, switchCaseIndex++);
                
                context.Get(ProcessedCustomSqlInfoListInProfile).Add(customSqlInfo);

                context.Add(CustomSqlInfo,customSqlInfo);
                context.FireEvent(CustomSqlInfoIsAvailable);
                context.Remove(CustomSqlInfo);
            }

        }
    }
}