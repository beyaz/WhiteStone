﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BOA.Common.Helpers;

namespace BOAPlugins.ExportingModel
{
    public class ExportAsCSharpCodeData
    {
        #region Fields
        public string                      solutionFilePath;
        public IDictionary<string, string> usedPropertyNames;
        #endregion

        #region Public Properties
        public bool RemoveUnusedProperties { get; set; }
        #endregion
    }

    public class MessagingExporter
    {
        #region Public Methods
        public static MessagingExporterResult ExportAsCSharpCode(ExportAsCSharpCodeData data)
        {
            var solutionFilePath = data.solutionFilePath;
            var directory        = Path.GetDirectoryName(solutionFilePath);

            if (directory == null)
            {
                return new MessagingExporterResult
                {
                    ErrorMessage = "directory is null.@solutionFilePath:" + solutionFilePath
                };
            }

            var messageFileName = "Message.designer.cs";
            var messageFilePath = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).LastOrDefault(f => f.EndsWith(messageFileName));

            if (messageFilePath == null)
            {
                return new MessagingExporterResult
                {
                    ErrorMessage = $"messageFilePath not found.You must specify named file like:{messageFileName}"
                };
            }

            if (data.RemoveUnusedProperties)
            {
                data.usedPropertyNames = new Dictionary<string, string>();

                RemoveUnusedMessagesInTypescriptCodes.Handler.HandleForCs(directory, data.usedPropertyNames, messageFilePath);
            }

            var firstLine = File.ReadAllLines(messageFilePath).FirstOrDefault(line => string.IsNullOrWhiteSpace(line) == false);

            var config = MessagingExporterInputLineParser.Parse(firstLine);

            return new MessagingExporterResult
            {
                GeneratedCode  = ExportAsCSharpCode(config.GroupName, config.NamespaceName, data.usedPropertyNames),
                TargetFilePath = messageFilePath
            };
        }

        public static MessagingExporterResult ExportAsTypeScriptCode(string solutionFilePath, IDictionary<string, string> usedPropertyNames = null)
        {
            var directory = Path.GetDirectoryName(solutionFilePath);

            if (directory == null)
            {
                return new MessagingExporterResult
                {
                    ErrorMessage = "directory is null.@solutionFilePath:" + solutionFilePath
                };
            }

            var messageFileName = "Message.tsx";
            var messageFilePath = Directory.GetFiles(directory, "*.tsx", SearchOption.AllDirectories).LastOrDefault(f => f.EndsWith(messageFileName));

            if (messageFilePath == null)
            {
                return new MessagingExporterResult
                {
                    ErrorMessage = $"messageFilePath not found.You must specify named file like:{messageFileName}"
                };
            }

            var firstLine = File.ReadAllLines(messageFilePath).FirstOrDefault(line => string.IsNullOrWhiteSpace(line) == false);

            var config = MessagingExporterInputLineParser.Parse(firstLine);

            return new MessagingExporterResult
            {
                GeneratedCode  = ExportGroupAsTypeScriptCode(config.GroupName, usedPropertyNames),
                TargetFilePath = messageFilePath
            };
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
    }
}