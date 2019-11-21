using System;
using System.IO;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.UI.MainForm
{
    class CheckInCommentAccess
    {
        #region Properties
        static string TempFile => Path.GetTempPath() + AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".txt");
        #endregion

        #region Public Methods
        public static string GetCheckInComment()
        {
            var comment = "";
            if (File.Exists(TempFile))
            {
                comment = File.ReadAllText(TempFile).Trim();
            }

            if (comment.IsNullOrWhiteSpace())
            {
                comment = "2235# - AutoCheckInByEntityGenerator";
            }

            return comment;
        }

        public static void SaveCheckInComment(string comment)
        {
            File.WriteAllText(TempFile, comment);
        }
        #endregion
    }
}