using System.Collections.Generic;
using BOA.CodeGeneration.Util;

namespace BOA.OneDesigner.Helpers
{
    static class TfsHelper
    {
        #region Public Methods
        public static IReadOnlyList<string> GetFolderNames()
        {
            var businessModules = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.BusinessModules/Dev/*");
            var cardModules     = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.CardModules/Dev/*");

            var items = new List<string>();
            items.AddRange(businessModules);
            items.AddRange(cardModules);

            return items;
        }
        #endregion
    }
}