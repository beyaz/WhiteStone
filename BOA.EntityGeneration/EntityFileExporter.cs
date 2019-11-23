using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.Models.Interfaces;
using BOA.EntityGeneration.Naming;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.Data;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;
using static BOA.EntityGeneration.Naming.NamingPatternContract;
using static BOA.EntityGeneration.Naming.TableNamingPatternContract;

namespace BOA.EntityGeneration
{
    class ContextContainer : BOA.DataFlow.ContextContainer
    {
        #region Properties
        protected ConfigContract             config             => Config[Context];
        protected NamingPatternContract      namingPattern      => NamingPattern[Context];
        protected ProcessContract            processInfo        => ProcessInfo[Context];
        protected ITableInfo                 tableInfo          => TableInfo[Context];
        protected TableNamingPatternContract tableNamingPattern => TableNamingPattern[Context];
        protected string tableEntityClassNameForMethodParametersInRepositoryFiles => TableEntityClassNameForMethodParametersInRepositoryFiles[Context];
        protected string schemaName => SchemaName[Context];
        
        #endregion
    }

    class EntityFileExporter : ContextContainer
    {
        #region Static Fields
        static readonly Property<PaddedStringBuilder> File = Property.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Properties
        PaddedStringBuilder file => File[Context];
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            AttachEvent(SchemaExportStarted, InitializeOutput);
            AttachEvent(SchemaExportStarted, WriteUsingList);
            AttachEvent(SchemaExportStarted, EmptyLine);
            AttachEvent(SchemaExportStarted, BeginNamespace);

            AttachEvent(TableExportStarted, WriteClass);

            AttachEvent(SchemaExportFinished, EndNamespace);
            AttachEvent(SchemaExportFinished, ExportFileToDirectory);
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

            FileSystem.WriteAllText(Context, filePath, file.ToString());
        }

        void InitializeOutput()
        {
            File[Context] = new PaddedStringBuilder();
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