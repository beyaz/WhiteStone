using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BOA.Common.Helpers;

namespace BOAPlugins.Messaging
{
    public class MessagingExporterData
    {
        #region Public Properties
        public string                      ErrorMessage           { get; set; }
        public string                      GeneratedCode          { get; set; }
        public bool                        RemoveUnusedProperties { get; set; }
        public string                      SolutionFilePath       { get; set; }
        public string                      TargetFilePath         { get; set; }
        public IDictionary<string, string> UsedPropertyNames      { get; set; }
        #endregion
    }

    public class MessagingExporter
    {
        #region Public Methods
        public static void ExportAsCSharpCode(MessagingExporterData data)
        {
            var solutionFilePath = data.SolutionFilePath;
            var directory        = Path.GetDirectoryName(solutionFilePath);

            if (directory == null)
            {
                data.ErrorMessage = "directory is null.@solutionFilePath:" + solutionFilePath;

                return;
            }

            var messageFileName = "Message.designer.cs";
            var messageFilePath = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).LastOrDefault(f => f.EndsWith(messageFileName));

            if (messageFilePath == null)
            {
                data.ErrorMessage = $"messageFilePath not found.You must specify named file like:{messageFileName}";
                return;
            }

            if (data.RemoveUnusedProperties)
            {
                data.UsedPropertyNames = new Dictionary<string, string>();

                MessagesCleaner.SearchPropertyNamesForCs(directory, data.UsedPropertyNames, messageFilePath);
            }

            var firstLine = File.ReadAllLines(messageFilePath).FirstOrDefault(line => string.IsNullOrWhiteSpace(line) == false);

            var config = MessagingExporterInputLineParser.Parse(firstLine);

            data.GeneratedCode  = ExportAsCSharpCode(config.GroupName, config.NamespaceName, data.UsedPropertyNames);
            data.TargetFilePath = messageFilePath;
        }

        public static void ExportAsTypeScriptCode(MessagingExporterData data)
        {
            var directory = Path.GetDirectoryName(data.SolutionFilePath);

            if (directory == null)
            {
                data.ErrorMessage = "directory is null.@solutionFilePath:" + data.SolutionFilePath;
                return;
            }

            const string messageFileName = "Message.tsx";
            var          messageFilePath = Directory.GetFiles(directory, "*.tsx", SearchOption.AllDirectories).LastOrDefault(f => f.EndsWith(messageFileName));

            if (messageFilePath == null)
            {
                data.ErrorMessage = $"messageFilePath not found.You must specify named file like:{messageFileName}";
                return;
            }

            var firstLine = File.ReadAllLines(messageFilePath).FirstOrDefault(line => string.IsNullOrWhiteSpace(line) == false);

            var config = MessagingExporterInputLineParser.Parse(firstLine);

            if (data.RemoveUnusedProperties)
            {
                data.UsedPropertyNames = new Dictionary<string, string>();

                MessagesCleaner.SearchPropertyNamesForTsx(directory, data.UsedPropertyNames, messageFilePath);
            }

            data.GeneratedCode  = ExportGroupAsTypeScriptCode(config.GroupName, data.UsedPropertyNames);
            data.TargetFilePath = messageFilePath;
        }

        public static string ExportGroupAsTypeScriptCode(string groupName, IDictionary<string, string> usedPropertyNames = null)
        {
            var builder = new PaddedStringBuilder();

            var propertyInfos = DataSource.GetPropertyNames(groupName);

            if (usedPropertyNames != null)
            {
                var usedPropertyInfos = new List<PropertyInfo>();
                foreach (var property in propertyInfos)
                {
                    if (usedPropertyNames.ContainsKey(property.PropertyName))
                    {
                        usedPropertyInfos.Add(property);
                    }
                }

                propertyInfos = usedPropertyInfos;
            }

            builder.AppendLine($"// GroupName: {groupName}");

            builder.AppendLine("import { getMessage } from \"b-framework\"");
            builder.AppendLine("");

            builder.AppendLine("function M(propertyName : string) : string");
            builder.AppendLine("{");
            builder.AppendLine("    return getMessage(\"" + groupName + "\", propertyName);");
            builder.AppendLine("}");

            builder.AppendLine("export class Message");
            builder.AppendLine("{");
            builder.PaddingCount++;

            foreach (var item in propertyInfos)
            {
                var comment = GetComment(item);
                if (comment.HasValue())
                {
                    if (comment.Contains('/'))
                    {
                        builder.AppendLine($"/**'{comment}'*/");
                    }
                    else
                    {
                        builder.AppendLine($"/**{comment}*/");
                    }
                }

                var propertyName = item.PropertyName;

                builder.AppendLine($"static get {propertyName}():string" + "{" + $"return M(\"{propertyName}\");" + "}");
            }

            builder.PaddingCount--;
            builder.AppendLine("}");

            return builder.ToString();
        }
        #endregion

        #region Methods
        static string ExportAsCSharpCode(string groupName, string namespaceFullName, IDictionary<string, string> usedPropertyNames = null)
        {
            if (string.IsNullOrWhiteSpace(namespaceFullName))
            {
                throw new ArgumentException(nameof(namespaceFullName));
            }

            var builder = new PaddedStringBuilder();

            var propertyNames = DataSource.GetPropertyNames(groupName);

            if (usedPropertyNames != null)
            {
                var usedPropertyInfos = new List<PropertyInfo>();
                foreach (var property in propertyNames)
                {
                    if (usedPropertyNames.ContainsKey(property.PropertyName))
                    {
                        usedPropertyInfos.Add(property);
                    }
                }

                propertyNames = usedPropertyInfos;
            }

            builder.AppendLine($"// GroupName: {groupName} , NamespaceName: {namespaceFullName}");
            builder.AppendLine("");
            builder.AppendLine("using BOA.Messaging;");
            builder.AppendLine($"namespace {namespaceFullName}");
            builder.AppendLine("{");
            builder.PaddingCount++;

            builder.AppendLine("/// <summary>");
            builder.AppendLine("     The message");
            builder.AppendLine("/// </summary>");
            builder.AppendLine("public static partial class Message");
            builder.AppendLine("{");
            builder.PaddingCount++;

            builder.AppendLine("/// <summary>");
            builder.AppendLine("///     Gets the message from property name.");
            builder.AppendLine("/// </summary>");
            builder.AppendLine("static string M(string propertyName)");
            builder.AppendLine("{");
            builder.AppendLine("    return MessagingHelper.GetMessage(\"" + groupName + "\", propertyName);");
            builder.AppendLine("}");

            foreach (var item in propertyNames)
            {
                var propertyName = item.PropertyName;

                var comment = GetComment(item);
                if (comment.HasValue())
                {
                    builder.AppendLine($"/// <summary>{comment}</summary>");
                }

                builder.AppendLine($"public static string {propertyName} => M(nameof({propertyName}));");
            }

            builder.PaddingCount--;
            builder.AppendLine("}");

            builder.PaddingCount--;
            builder.AppendLine("}");

            return builder.ToString();
        }

        static string GetComment(PropertyInfo item)
        {
            var comment = new StringBuilder();

            if (item.TR_Description.HasValue() || item.EN_Description.HasValue())
            {
                var hasTR = false;
                if (item.TR_Description?.Trim().HasValue() == true)
                {
                    hasTR = true;
                    comment.Append(item.TR_Description.Replace(Environment.NewLine, "").Trim());
                }

                if (item.EN_Description?.Trim().HasValue() == true)
                {
                    if (hasTR)
                    {
                        comment.Append(" | ");
                    }

                    comment.Append(item.EN_Description.Replace(Environment.NewLine, "").Trim());
                }

                return comment.ToString();
            }

            return null;
        }
        #endregion

        class MessagingExporterInputLineParser
        {
            #region Methods
            internal static MessagingExporterInput Parse(string line)
            {
                var config = new MessagingExporterInput();

                if (line == null)
                {
                    throw new ArgumentNullException(nameof(line));
                }

                var arr = line.Trim().RemoveFromStart("//").Trim().Split(',');

                foreach (var param in arr)
                {
                    if (param.Trim().StartsWith(nameof(MessagingExporterInput.GroupName)))
                    {
                        config.GroupName = param.Trim().RemoveFromStart(nameof(MessagingExporterInput.GroupName)).Trim().RemoveFromStart(":").Trim();
                    }

                    if (param.Trim().StartsWith(nameof(MessagingExporterInput.NamespaceName)))
                    {
                        config.NamespaceName = param.Trim().RemoveFromStart(nameof(MessagingExporterInput.NamespaceName)).Trim().RemoveFromStart(":").Trim();
                    }
                }

                if (config.GroupName == null)
                {
                    throw new ArgumentException(nameof(MessagingExporterInput.GroupName));
                }

                return config;
            }
            #endregion

            internal class MessagingExporterInput
            {
                #region Public Properties
                public string GroupName     { get; set; }
                public string NamespaceName { get; set; }
                #endregion
            }
        }
    }
}