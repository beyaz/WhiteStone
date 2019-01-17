using System.Collections.Generic;
using BOA.DatabaseAccess;

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


        public List<Aut_ResourceAction> GetResourceActions(string resourceCode)
        {
            return this.GetRecords<Aut_ResourceAction>("SELECT Name,CommandName from AUT.ResourceAction WHERE ResourceId = (SELECT  ResourceId from AUT.Resource WITH(NOLOCK) WHERE ResourceCode = @resourceCode OR Name = @resourceCode)", nameof(resourceCode), resourceCode);
        }
    }
}