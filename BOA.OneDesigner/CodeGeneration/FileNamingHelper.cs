using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BOA.Common.Helpers;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class FileNamingHelper
    {
        #region Public Methods
        public static void InitDefaultOutputTypeScriptFileName(ScreenInfo data)
        {
            if (data.OutputTypeScriptFileName.HasValue())
            {
                return;
            }

            if (data.RequestName.IsNullOrWhiteSpace())
            {
                return;
            }

            var fileName = SnapNamingHelper.GetLastPropertyName(data.RequestName.RemoveFromEnd("Request"));

            var words = GetWords(fileName);

            data.OutputTypeScriptFileName = string.Join("-", words.Select(x => x.ToLowerEN()));
        }

        public static List<string> GetWords(string camelCaseString)
        {
            return  Regex.Matches(camelCaseString, @"([A-Z][a-z]+)")
                     .Cast<Match>()
                     .Select(m => m.Value).ToList();
        }
        #endregion
    }
}