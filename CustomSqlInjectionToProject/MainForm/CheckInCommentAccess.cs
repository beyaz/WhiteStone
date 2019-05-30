using System.IO;
using BOA.Common.Helpers;

namespace CustomSqlInjectionToProject.MainForm
{
    class CheckInCommentAccess
    {
        static string TempFile => Path.GetTempPath() +  System.AppDomain.CurrentDomain.FriendlyName.Replace(".exe",".txt");

        public static string GetCheckInComment()
        {

            var comment = "";
            if (File.Exists(TempFile))
            {
                comment = File.ReadAllText(TempFile).Trim();
            }

            if (comment.IsNullOrWhiteSpace())
            {
                comment = "2235# - AutoCheckInByCustomSqlGenerator";
            }

            return comment;
        }

        
        public static void SaveCheckInComment(string comment)
        {

            File.WriteAllText(TempFile,comment);
            
        }
    }
}