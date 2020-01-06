using System;
using System.Collections.Generic;
using BOA.TfsAccess;

namespace BOA.OneDesigner.TfsDomain
{
    static class Tfs
    {
        #region Public Methods
        public static IReadOnlyList<string> GetFolderNames()
        {
            const string SelfService = @"$/BOA/Dev/BOA.SelfService";

            var businessModules = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.BusinessModules/Dev/*");
            var cardModules     = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.CardModules/Dev/*");
            var loans           = TFSAccessForBOA.GetSubFolderNames(@"$/BOA.Loans/Dev/*");

            var items = new List<string>();

            items.AddRange(businessModules);
            items.AddRange(cardModules);
            items.AddRange(loans);
            items.Add(SelfService);

            return items;
        }

        public static FileAccessWriteResult WriteAllText(string path, string content)
        {
            var source = new FileAccess().WriteAllText(path, content);

            return new FileAccessWriteResult
            {
                TfsVersionAndNewContentIsSameSoNothingDoneAnything = source.TfsVersionAndNewContentIsSameSoNothingDoneAnything,
                ThereIsNoFileAndFileCreated                        = source.ThereIsNoFileAndFileCreated,
                Exception                                          = source.Exception
            };
        }
        #endregion
    }

    public class FileAccessWriteResult
    {
        #region Public Properties
        public Exception Exception                                          { get; set; }
        public bool      TfsVersionAndNewContentIsSameSoNothingDoneAnything { get; set; }
        public bool      ThereIsNoFileAndFileCreated                        { get; set; }
        #endregion
    }
}