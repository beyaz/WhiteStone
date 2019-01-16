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

            var words =
                Regex.Matches(fileName, @"([A-Z][a-z]+)")
                     .Cast<Match>()
                     .Select(m => m.Value);

            data.OutputTypeScriptFileName = string.Join("-", words.Select(x => x.ToLowerEN()));
        }
        #endregion
    }
}