using System.Collections.Generic;
using BOA.CodeGeneration.Util;

namespace BOA.OneDesigner.Helpers
{
    static class TfsHelper
    {
        public const string SelfService = @"$/BOA/Dev/BOA.SelfService";

        #region Public Methods
        public static IReadOnlyList<string> GetFolderNames()
        {
            var businessModules = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.BusinessModules/Dev/*");
            var cardModules     = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.CardModules/Dev/*");
            var loans = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.Loans/Dev/*");

            var items = new List<string>();

            items.AddRange(businessModules);
            items.AddRange(cardModules);
            items.AddRange(loans);
            items.Add(SelfService);

            return items;
        }
        #endregion
    }
}