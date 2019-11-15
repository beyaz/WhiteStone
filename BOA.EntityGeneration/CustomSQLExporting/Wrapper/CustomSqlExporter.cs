using System;
using System.Collections.Generic;
using System.Threading;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces;
using BOA.EntityGeneration.DataFlow;

namespace BOA.EntityGeneration.CustomSQLExporting.Wrapper
{
    public static class CustomSqlExporter
    {
        #region Static Fields
        public static readonly IEvent OnProfileInfoInitialized   = new Event {Name = nameof(OnProfileInfoInitialized)};
        public static readonly IEvent OnProfileInfoRemove = new Event {Name = nameof(OnProfileInfoRemove)};
        public static readonly IEvent OnCustomSqlInfoInitialized = new Event {Name = nameof(OnCustomSqlInfoInitialized)};
        #endregion


        public static void Export(IDataContext context, string profileId)
        {
            context.Add(ProfileId,profileId);
            context.Add(ProcessedCustomSqlInfoListInProfile,new List<ICustomSqlInfo>());

            InitializeProfileInfo(context);
            ProcessCustomSQLsInProfile(context);
            RemoveProfileInfo(context);

            context.Remove(ProcessedCustomSqlInfoListInProfile);
            context.Remove(ProfileId);

            var processInfo = context.Get(Data.CustomSqlGenerationOfProfileIdProcess);
            processInfo.Text = "Finished Successfully.";
            Thread.Sleep(TimeSpan.FromSeconds(2));

        }


       

       

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

            context.Get(Data.CustomSqlGenerationOfProfileIdProcess).Text = "Fetching profile informations...";
            var profileInfo = ProjectCustomSqlInfoDataAccess.GetByProfileIdFromDatabase(database, profileId);

            context.Add(CustomSqlProfileInfo,profileInfo);

            context.FireEvent(OnProfileInfoInitialized);
        }

        static void RemoveProfileInfo(IDataContext context)
        {

            context.FireEvent(OnProfileInfoRemove);
            context.Remove(CustomSqlProfileInfo);
        }

        static void ProcessCustomSQLsInProfile(IDataContext context)
        {
            var profileInfo = context.Get(CustomSqlProfileInfo);
            var processInfo = context.Get(Data.CustomSqlGenerationOfProfileIdProcess);

            var config    = context.Get(Data.Config);
            var database  = context.Get(Data.Database);
            var profileId = context.Get(ProfileId);

            processInfo.Total = profileInfo.ObjectIdList.Count;

            var switchCaseIndex = 0;
            foreach (var objectId in profileInfo.ObjectIdList)
            {
                processInfo.Text = $"Processing '{objectId}'";
                processInfo.Current = switchCaseIndex;    

                var customSqlInfo = ProjectCustomSqlInfoDataAccess.GetCustomSqlInfo(database, profileId, objectId, config, switchCaseIndex++);
                
                context.Get(ProcessedCustomSqlInfoListInProfile).Add(customSqlInfo);

                context.Add(CustomSqlInfo,customSqlInfo);
                context.FireEvent(OnCustomSqlInfoInitialized);
                context.Remove(CustomSqlInfo);
            }

        }
    }
}