using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.AppModel
{
    class DevelopmentDatabase : SqlDatabase
    {
        #region Constants
        const string ConnectionString = "server=srvdev\\ATLAS;database =BOA;integrated security=true";
        #endregion

        #region Constructors
        public DevelopmentDatabase() : base(ConnectionString)
        {
        }
        #endregion

        #region Public Methods
        public List<Aut_ResourceAction> GetResourceActions(string resourceCode)
        {
            return this.GetRecords<Aut_ResourceAction>("SELECT Name,CommandName from AUT.ResourceAction WHERE ResourceId = (SELECT  ResourceId from AUT.Resource WITH(NOLOCK) WHERE ResourceCode = @resourceCode OR Name = @resourceCode)", nameof(resourceCode), resourceCode);
        }

        public void Load(ScreenInfo data)
        {
            CommandText = $@"
SELECT TOP 1 *
 FROM  BOA.DBT.OneDesigner 
WHERE {nameof(data.RequestName)} = @{nameof(data.RequestName)}
";

            this[nameof(data.RequestName)] = data.RequestName;

            var reader = ExecuteReader();
            reader.Read();

            data.ResourceCode = Convert.ToString(reader[nameof(data.ResourceCode)]);
            data.RequestName              =Convert.ToString( reader[nameof(data.RequestName)]);
            data.FormType                 =Convert.ToString( reader[nameof(data.FormType)]);
            data.MessagingGroupName       =Convert.ToString( reader[nameof(data.MessagingGroupName)]);
            data.OutputTypeScriptFileName =Convert.ToString( reader[nameof(data.OutputTypeScriptFileName)]);
            data.TfsFolderName            =Convert.ToString( reader[nameof(data.TfsFolderName)]);
            data.UserName                 =Convert.ToString( reader[nameof(data.UserName)]);
            data.SystemDate               = Convert.ToDateTime( reader[nameof(data.SystemDate)]);

            data.JsxModel        = BinarySerialization.Deserialize(CompressionHelper.Decompress((byte[]) reader[nameof(data.JsxModel)]));
            data.ResourceActions = (List<Aut_ResourceAction>) BinarySerialization.Deserialize(CompressionHelper.Decompress((byte[]) reader[nameof(data.ResourceActions)]));
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
WHERE {nameof(data.RequestName)} = @{nameof(data.RequestName)}
";

            this[nameof(data.RequestName)] = data.RequestName;

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