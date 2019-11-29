using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class EntityFileExporter : ContextContainer
    {
        static readonly EntityFileExporterConfig EntityFileExporterConfig = EntityFileExporterConfig.CreateFromFile();

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
            file.BeginNamespace(NamingPattern.EntityNamespace);
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
            ProcessInfo.Text = "Exporting Entity classes.";

            const string fileName = "All.cs";

            Context.EntityProjectSourceFileNames.Add(fileName);

            var filePath = NamingPattern.EntityProjectDirectory + fileName;

            var content = file.ToString();

            Context.OnEntityFileContentCompleted(content);

            FileSystem.WriteAllText(filePath, content);
        }

        void WriteClass()
        {
            ContractCommentInfoCreator.Write(file, TableInfo);

            var inheritancePart = string.Empty;

            if (EntityFileExporterConfig.EntityContractBase != null)
            {
                inheritancePart = ": " + EntityFileExporterConfig.EntityContractBase;
            }

            file.AppendLine("[Serializable]");
            file.AppendLine($"public sealed class {TableNamingPattern.EntityClassName} {inheritancePart}");
            file.OpenBracket();

            ContractCommentInfoCreator.Write(file, TableInfo);
            file.AppendLine("// ReSharper disable once EmptyConstructor");
            file.AppendLine($"public {TableInfo.TableName.ToContractName()}Contract()");
            file.OpenBracket();
            file.CloseBracket();
            file.AppendLine();

            file.AppendAll(ContractBodyDbMembersCreator.Create(TableInfo).PropertyDefinitions);
            file.AppendLine();

            file.CloseBracket(); // end of class
        }

        void WriteUsingList()
        {
            foreach (var line in NamingPattern.EntityUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}