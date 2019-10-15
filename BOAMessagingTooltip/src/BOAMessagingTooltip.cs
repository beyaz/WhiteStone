using System;
using System.Collections.Generic;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;

namespace BOAMessagingTooltip
{
    public class ExecutionData
    {
        #region Public Properties
        public string              Line                   { get; set; }
        public bool                LineParsedSuccessFully => MessagingAccessInfo != null;
        public MessagingAccessInfo MessagingAccessInfo    { get; set; }
        public List<string>        Trace                  { get; set; } = new List<string>();
        #endregion
    }

    public class Execution
    {
        #region Public Methods
        public static void Process(ExecutionData data)
        {
            data.Trace.Add($"Started to process line:{data.Line}");

            var messagingAccessInfo = Parser.Parse(data.Line);

            if (messagingAccessInfo == null)
            {
                data.Trace.Add("MessagingNotParsed.");
                return;
            }

            data.Trace.Add("MessagingParsedSuccessfully.");

            MessagingDataAccess.FillTR(messagingAccessInfo);

            if (string.IsNullOrWhiteSpace(messagingAccessInfo.TurkishText))
            {
                data.Trace.Add("MessagingTurkishTextIsEmpty.");
                return;
            }

            data.MessagingAccessInfo = messagingAccessInfo;
        }
        #endregion
    }

    [Serializable]
    public class MessagingAccessInfo
    {
        #region Public Properties
        public string GroupCode    { get; set; }
        public string PropertyName { get; set; }
        public string TurkishText  { get; set; }
        #endregion
    }

    public static class Parser
    {
        #region Public Methods
        public static MessagingAccessInfo Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            const string methodPattern = "MessagingHelper.GetMessage";
            if (line.Contains(methodPattern))
            {
                var startIndex = line.IndexOf(methodPattern, StringComparison.OrdinalIgnoreCase) + methodPattern.Length;
                var part       = line.Substring(startIndex);
                part = part.Replace("(", " ").Replace(")", " ").Replace("\"", " ").Replace(";", " ").Replace(":", " ").Trim();

                var arr = part.SplitAndClear(",");
                if (arr.Count == 2)
                {
                    return new MessagingAccessInfo
                    {
                        GroupCode    = arr[0],
                        PropertyName = arr[1]
                    };
                }
            }

            return null;
        }
        #endregion
    }

    public static class MessagingDataAccess
    {
        #region Public Methods
        public static void FillTR(MessagingAccessInfo info)
        {
            info.TurkishText = GetTurkishText(info);
        }

        public static string GetTurkishText(MessagingAccessInfo info)
        {
            using (var database = new SqlDatabase("server=srvdev\\ATLAS;database =BOA;integrated security=true"))
            {
                const string sql = @"
  SELECT TOP 1 md.Description	AS TR_Description
	  FROM COR.Messaging         m WITH(NOLOCK) 
INNER JOIN COR.MessagingDetail  md WITH(NOLOCK) ON m.Code = md.Code  AND md.LanguageId  = 1
 LEFT JOIN COR.MessagingDetail md2 WITH(NOLOCK) ON m.Code = md2.Code AND md2.LanguageId = 2
 WHERE MessagingGroupId = ( SELECT MessagingGroupId 
							                FROM Cor.MessagingGroup WITH(NOLOCK) 
							                WHERE Name           = @" + nameof(info.GroupCode) + @"
                                AND m.PropertyName = @" + nameof(info.PropertyName) + @"
                          )

";

                database.CommandText                = sql;
                database[nameof(info.GroupCode)]    = info.GroupCode;
                database[nameof(info.PropertyName)] = info.PropertyName;

                var turkishText = database.ExecuteScalar() + string.Empty;

                if (string.IsNullOrWhiteSpace(turkishText))
                {
                    return null;
                }

                return turkishText;
            }
        }
        #endregion
    }
}