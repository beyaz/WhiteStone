using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOA.UI.Types;
using BOAPlugins.Messaging;

namespace BOA.OneDesigner.AppModel
{
    public class DevelopmentDatabase : SqlDatabase
    {
        #region Constants
        const string ConnectionString = "server=srvdev\\ATLAS;database =BOA;integrated security=true";

        const string TableName = "BOA.DBT.OneDesigner";
        #endregion

        #region Constructors
        public DevelopmentDatabase() : base(ConnectionString)
        {
        }
        #endregion

        #region Public Methods
        public int DeleteByRequestName(string requestName)
        {
            CommandText = $"DELETE FROM {TableName} WHERE {nameof(ScreenInfo.RequestName)} = @{nameof(requestName)}";

            this[nameof(requestName)] = requestName;

            return ExecuteNonQuery();
        }

        public List<ScreenInfo> GetAllScreens()
        {
            var records = this.GetRecords<ScreenInfo>($"SELECT RequestName from {TableName} ");
            foreach (var screenInfo in records)
            {
                Load(screenInfo);
            }

            return records;
        }

        public IReadOnlyList<string> GetDefaultRequestNames()
        {
            return this.GetRecords<Pair>("SELECT RequestName AS [Key] FROM BOA.DBT.OneDesigner WITH(NOLOCK)").Select(x => x.Key).ToList();
        }

        public IReadOnlyList<string> GetMessagingGroupNames()
        {
            return this.GetRecords<Pair>("select DISTINCT(Name) as [Key] from BOA.COR.MessagingGroup WITH(NOLOCK)").Select(x => x.Key).ToList();
        }

        public IList<PropertyInfo> GetPropertyNames(string groupName)
        {
            return DataSource.GetPropertyNames(groupName);
        }

        public List<Aut_ResourceAction> GetResourceActions(string resourceCode)
        {
            return this.GetRecords<Aut_ResourceAction>(@"
 SELECT Name,CommandName 
   FROM AUT.ResourceAction     as b WITH(NOLOCK) INNER JOIN 
        BOA.ONE.ResourceAction as o WITH(NOLOCK) ON b.ResourceId = o.ResourceId AND b.ActionId = o.ResourceActionId
  WHERE b.ResourceId = (SELECT  ResourceId from AUT.Resource WITH(NOLOCK) WHERE ResourceCode = @resourceCode OR Name = @resourceCode)", 
                                                       nameof(resourceCode), resourceCode);
        }

        public ScreenInfo GetScreenInfo(string requestName)
        {
            var screenInfo = new ScreenInfo
            {
                RequestName = requestName
            };

            var exists = Load(screenInfo);
            if (exists)
            {
                return screenInfo;
            }

            return null;
        }

        public IReadOnlyList<string> GetTfsFolderNames()
        {
            return TfsHelper.GetFolderNames();
        }

        public bool Load(ScreenInfo data)
        {
            CommandText = $@"
SELECT TOP 1 *
 FROM  BOA.DBT.OneDesigner 
WHERE ({nameof(data.RequestName)} = @{nameof(data.RequestName)} OR {nameof(data.ResourceCode)} = @{nameof(data.ResourceCode)})
  
";

            this[nameof(data.RequestName)]  = data.RequestName;
            this[nameof(data.ResourceCode)] = data.ResourceCode;

            var reader  = ExecuteReader();
            var success = reader.Read();
            if (success == false)
            {
                reader.Close();
                return false;
            }

            data.ResourceCode             = Convert.ToString(reader[nameof(data.ResourceCode)]);
            data.RequestName              = Convert.ToString(reader[nameof(data.RequestName)]);
            data.FormType                 = Convert.ToString(reader[nameof(data.FormType)]);
            data.MessagingGroupName       = Convert.ToString(reader[nameof(data.MessagingGroupName)]);
            data.OutputTypeScriptFileName = Convert.ToString(reader[nameof(data.OutputTypeScriptFileName)]);
            data.TfsFolderName            = Convert.ToString(reader[nameof(data.TfsFolderName)]);
            data.UserName                 = Convert.ToString(reader[nameof(data.UserName)]);
            data.SystemDate               = Convert.ToDateTime(reader[nameof(data.SystemDate)]);

            data.JsxModel        = BinarySerialization.Deserialize(CompressionHelper.Decompress((byte[]) reader[nameof(data.JsxModel)]));
            data.ResourceActions = (List<Aut_ResourceAction>) BinarySerialization.Deserialize(CompressionHelper.Decompress((byte[]) reader[nameof(data.ResourceActions)]));

            reader.Close();

            if (data.ResourceCode.IsNullOrWhiteSpace())
            {
                data.ResourceCode = null;
            }

            // TODO: eski sisteme destek verebilmek için. Bütün ekranlarda bir kez denenip sonrasında bu satır kaldırılmalı.
            VisitHelper.VisitAllChildren(data, VisitHelper.ConvertToAccountComponent);

            return true;
        }

        public void Save(ScreenInfo data)
        {
            var exists = Exists(data);
            if (exists)
            {
                Update(data);
                return;
            }

            Insert(data);
        }
        #endregion

        #region Methods
        void ApplyParameters(ScreenInfo data)
        {
            data.SystemDate = DateTime.Now;
            data.UserName   = Environment.UserName;

            this[nameof(data.ResourceCode)]             = data.ResourceCode;
            this[nameof(data.RequestName)]              = data.RequestName;
            this[nameof(data.FormType)]                 = data.FormType;
            this[nameof(data.MessagingGroupName)]       = data.MessagingGroupName;
            this[nameof(data.OutputTypeScriptFileName)] = data.OutputTypeScriptFileName;
            this[nameof(data.TfsFolderName)]            = data.TfsFolderName;
            this[nameof(data.JsxModel)]                 = CompressionHelper.Compress(BinarySerialization.Serialize(data.JsxModel) ?? new byte[0]);
            this[nameof(data.ResourceActions)]          = CompressionHelper.Compress(BinarySerialization.Serialize(data.ResourceActions) ?? new byte[0]);
            this[nameof(data.UserName)]                 = data.UserName;
            this[nameof(data.SystemDate)]               = data.SystemDate;
        }

        bool Exists(ScreenInfo data)
        {
            CommandText = $@"
SELECT TOP 1 1
 FROM  BOA.DBT.OneDesigner 
WHERE ({nameof(data.RequestName)} = @{nameof(data.RequestName)} OR {nameof(data.ResourceCode)} = @{nameof(data.ResourceCode)})
";

            this[nameof(data.RequestName)]  = data.RequestName;
            this[nameof(data.ResourceCode)] = data.ResourceCode;

            return (int?) ExecuteScalar() == 1;
        }

        void Insert(ScreenInfo data)
        {
            CommandText = $@"
INSERT INTO BOA.DBT.OneDesigner 
(
    {nameof(data.ResourceCode)},
    {nameof(data.RequestName)},
    {nameof(data.FormType)},
    {nameof(data.MessagingGroupName)},
    {nameof(data.OutputTypeScriptFileName)},
    {nameof(data.TfsFolderName)},
    {nameof(data.JsxModel)},
    {nameof(data.ResourceActions)},
    {nameof(data.UserName)},
    {nameof(data.SystemDate)}
)
VALUES
(
    @{nameof(data.ResourceCode)},
    @{nameof(data.RequestName)},
    @{nameof(data.FormType)},
    @{nameof(data.MessagingGroupName)},
    @{nameof(data.OutputTypeScriptFileName)},
    @{nameof(data.TfsFolderName)},
    @{nameof(data.JsxModel)},
    @{nameof(data.ResourceActions)},
    @{nameof(data.UserName)},
    @{nameof(data.SystemDate)}

)";

            ApplyParameters(data);

            ExecuteNonQuery();
        }

        void Update(ScreenInfo data)
        {
            CommandText = $@"
UPDATE BOA.DBT.OneDesigner SET

{nameof(data.ResourceCode)}                    = @{nameof(data.ResourceCode)},           

{nameof(data.FormType)}                        = @{nameof(data.FormType)},
{nameof(data.MessagingGroupName)}              = @{nameof(data.MessagingGroupName)},
{nameof(data.OutputTypeScriptFileName)}        = @{nameof(data.OutputTypeScriptFileName)},
{nameof(data.TfsFolderName)}                   = @{nameof(data.TfsFolderName)},
{nameof(data.JsxModel)}                        = @{nameof(data.JsxModel)},
{nameof(data.ResourceActions)}                 = @{nameof(data.ResourceActions)},
{nameof(data.UserName)}                        = @{nameof(data.UserName)},
{nameof(data.SystemDate)}                      = @{nameof(data.SystemDate)}

WHERE {nameof(data.RequestName)} = @{nameof(data.RequestName)}
";

            ApplyParameters(data);

            ExecuteNonQuery();
        }
        #endregion
    }
}