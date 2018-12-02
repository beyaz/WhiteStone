using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using BOA.Common.Helpers;
using BOAPlugins.Utility;

namespace BOAPlugins.Messaging
{
    class MessagesCleaner
    {
        #region Public Methods
        public static void SearchPropertyNamesForCs(string directoryPath, IDictionary<string, string> usedPropertyNames, string excludedFile)
        {
            var files = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file == excludedFile)
                {
                    continue;
                }

                PickupMessage(File.ReadAllText(file), usedPropertyNames);
            }

            files = Directory.GetFiles(directoryPath, "*.xaml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file == excludedFile)
                {
                    continue;
                }

                PickupMessage(File.ReadAllText(file), usedPropertyNames);
            }
        }

        public static void SearchPropertyNamesForTsx(string directoryPath, IDictionary<string, string> usedPropertyNames, string excludedFile)
        {
            var files = Directory.GetFiles(directoryPath, "*.tsx", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                if (file == excludedFile)
                {
                    continue;
                }

                PickupMessage(File.ReadAllText(file), usedPropertyNames);
            }
        }
        #endregion

        #region Methods
        internal static void PickupMessage(string tsxCode, IDictionary<string, string> usedPropertyNames)
        {
            const string Prefix = "Message.";

            const string pattern = "\\b" + Prefix + "(.+?(?=[+;!@#$%^&*(),.?:{ }|<> \n]))";

            var match = Regex.Match(tsxCode, pattern, RegexOptions.Multiline);
            while (match.Success)
            {
                var propertyName = match.Value.RemoveFromStart(Prefix).Trim();

                Util.SetValue(usedPropertyNames, propertyName, propertyName);

                match = match.NextMatch();
            }
        }
        #endregion
    }
}