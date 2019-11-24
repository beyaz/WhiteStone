using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration
{
    class EntityFileExporter : ContextContainer
    {
        #region Fields
        readonly PaddedStringBuilder file = new PaddedStringBuilder();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportStarted += WriteUsingList;
            SchemaExportStarted += EmptyLine;
            SchemaExportStarted += BeginNamespace;

            TableExportStarted += WriteClass;

            SchemaExportFinished += EndNamespace;
            SchemaExportFinished += ExportFileToDirectory;
        }
        #endregion

        #region Methods
        void BeginNamespace()
        {
            file.BeginNamespace(namingPattern.EntityNamespace);
        }

        void EmptyLine()
        {
            file.AppendLine();
        }

        void EndNamespace()
        {
            file.EndNamespace();
        }

        void ExportFileToDirectory()
        {
            processInfo.Text = "Exporting Entity classes.";

            var filePath = namingPattern.EntityProjectDirectory + "All.cs";

            var content = file.ToString();

            Context.OnEntityFileContentCompleted(content);

            FileSystem.WriteAllText(filePath, content);
        }

        void WriteClass()
        {
            ContractCommentInfoCreator.Write(file, tableInfo);

            var inheritancePart = string.Empty;

            if (config.EntityContractBase != null)
            {
                inheritancePart = ": " + config.EntityContractBase;
            }

            file.AppendLine("[Serializable]");
            file.AppendLine($"public sealed class {tableNamingPattern.EntityClassName} {inheritancePart}");
            file.OpenBracket();

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine("// ReSharper disable once EmptyConstructor");
            file.AppendLine($"public {tableInfo.TableName.ToContractName()}Contract()");
            file.OpenBracket();
            file.CloseBracket();
            file.AppendLine();

            file.AppendAll(ContractBodyDbMembersCreator.Create(tableInfo).PropertyDefinitions);
            file.AppendLine();

            file.CloseBracket(); // end of class
        }

        void WriteUsingList()
        {
            foreach (var line in namingPattern.EntityUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}