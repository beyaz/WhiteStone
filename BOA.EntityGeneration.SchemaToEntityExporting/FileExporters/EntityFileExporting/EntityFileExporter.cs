using System.IO;
using BOA.Common.Helpers;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters.EntityFileExporting
{
    class EntityFileExporter : ContextContainer
    {
        internal static readonly EntityFileExporterConfig Config = EntityFileExporterConfig.LoadFromFile();
        

        #region Fields
        readonly PaddedStringBuilder file = new PaddedStringBuilder();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportStarted += InitializeNamingForSchema;
            SchemaExportStarted += WriteUsingList;
            SchemaExportStarted += EmptyLine;
            SchemaExportStarted += BeginNamespace;

            TableExportStarted += InitializeNamingForTable;
            TableExportStarted += WriteClass;

            SchemaExportFinished += EndNamespace;
            SchemaExportFinished += ExportFileToDirectory;
        }
        #endregion

        void InitializeNamingForSchema()
        {
            NamingMap.Push(NamingMapKey.EntityNamespaceName, NamingMap.Resolve(Config.NamespaceName));
        }
        void InitializeNamingForTable()
        {
            NamingMap.Push(NamingMapKey.EntityClassName, NamingMap.Resolve(Config.ClassName));
        }
        #region Methods
        void BeginNamespace()
        {
            file.BeginNamespace(NamingMap.Resolve(NamingMapKey.EntityNamespaceName));
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

            var filePath = Resolve(Config.OutputFilePath);


            Context.EntityProjectSourceFileNames.Add(Path.GetFileName(filePath));

            

            var content = file.ToString();

            Context.OnEntityFileContentCompleted(content);

            FileSystem.WriteAllText(filePath, content);
        }

        void WriteClass()
        {
            ContractCommentInfoCreator.Write(file, TableInfo);

            var inheritancePart = string.Empty;

            if (Config.EntityContractBase != null)
            {
                inheritancePart = ": " + Config.EntityContractBase;
            }

            file.AppendLine("[Serializable]");
            file.AppendLine($"public sealed class {NamingMap.Resolve(NamingMapKey.EntityClassName)} {inheritancePart}");
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
            foreach (var line in Config.UsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}