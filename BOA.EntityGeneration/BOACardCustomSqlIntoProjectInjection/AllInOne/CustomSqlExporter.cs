using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models.Interfaces;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
    public static class CustomSqlExporter
    {
        #region Static Fields
        public static readonly IEvent StartedToExportObjectId  = new Event {Name = nameof(StartedToExportObjectId)};
        public static readonly IEvent StartedToExportProfileId = new Event {Name = nameof(StartedToExportProfileId)};
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            TypeFileExporter.AttachEvents(context);
        }
        #endregion

        #region Output Strings
        public static readonly IDataConstant<PaddedStringBuilder> SharedDalFile = DataConstant.Create<PaddedStringBuilder>(nameof(SharedDalFile));
        public static readonly IDataConstant<PaddedStringBuilder> BoaDalFile    = DataConstant.Create<PaddedStringBuilder>(nameof(BoaDalFile));
        #endregion

        #region Data
        public static readonly IDataConstant<string>                ProfileId            = DataConstant.Create<string>(nameof(ProfileId));
        public static readonly IDataConstant<ICustomSqlInfo>        CustomSqlInfo        = DataConstant.Create<ICustomSqlInfo>();
        public static readonly IDataConstant<IProjectCustomSqlInfo> CustomSqlInfoProject = DataConstant.Create<IProjectCustomSqlInfo>();
        #endregion
    }
}