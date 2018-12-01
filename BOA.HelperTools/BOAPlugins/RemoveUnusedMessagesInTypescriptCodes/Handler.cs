using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using BOA.Common.Helpers;

namespace BOAPlugins.RemoveUnusedMessagesInTypescriptCodes
{
    class Handler
    {
        #region Public Methods
        public static void Handle(string directoryPath, IDictionary<string, string> propertyNames, string excludedFile)
        {
            var files = Directory.GetFiles(directoryPath, "*.tsx", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file == excludedFile)
                {
                    continue;
                }

                PickupMessage(File.ReadAllText(file), propertyNames);
            }
        }

        public static void HandleForCs(string directoryPath, IDictionary<string, string> propertyNames, string excludedFile)
        {
            var files = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file == excludedFile)
                {
                    continue;
                }

                PickupMessage(File.ReadAllText(file), propertyNames);
            }

            files = Directory.GetFiles(directoryPath, "*.xaml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file == excludedFile)
                {
                    continue;
                }

                PickupMessage(File.ReadAllText(file), propertyNames);
            }
        }
        #endregion

        #region Methods
        internal static void PickupMessage(string tsxCode, IDictionary<string, string> propertyNames)
        {
            const string Prefix = "Message.";

            const string pattern = "\\b" + Prefix + "(.+?(?=[+;!@#$%^&*(),.?:{ }|<> \n]))";

            var match = Regex.Match(tsxCode, pattern, RegexOptions.Multiline);
            while (match.Success)
            {
                var propertyName = match.Value.RemoveFromStart(Prefix).Trim();

                propertyNames.SetValue(propertyName, propertyName);

                match = match.NextMatch();
            }
        }
        #endregion
    }
}