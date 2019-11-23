using System;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.ScriptModel.Creators;
using BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;

namespace BOA.EntityGeneration.SharedRepositoryFileExporting
{
    class SharedFileExporter: ContextContainer
    {
        #region Static Fields
        internal static readonly Property<PaddedStringBuilder> File = Property.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        PaddedStringBuilder file => File[Context];

        #region Public Methods
        public void AttachEvents()
        {
            AttachEvent(SchemaExportStarted, InitializeOutput);
            AttachEvent(SchemaExportStarted, WriteUsingList);
            AttachEvent(SchemaExportStarted, EmptyLine);
            AttachEvent(SchemaExportStarted, BeginNamespace);
            AttachEvent(SchemaExportStarted, WriteEmbeddedClasses);

            AttachEvent(TableExportStarted, WriteClass);

            AttachEvent(SchemaExportFinished, EndNamespace);
            AttachEvent(SchemaExportFinished, ExportFileToDirectory);
        }
        #endregion

        #region Methods
        void BeginNamespace()
        {

            file.BeginNamespace(namingPattern.RepositoryNamespace + ".Shared");
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
            var sourceCode    = file.ToString();

            processInfo.Text = "Exporting Shared repository...";

            FileSystem.WriteAllText(Context, namingPattern.RepositoryProjectDirectory + "Shared.cs", sourceCode);
        }

        void InitializeOutput()
        {
            File[Context] = new PaddedStringBuilder();
        }

        void WriteClass()
        {

            file.AppendLine($"public sealed class {tableNamingPattern.SharedRepositoryClassName}");
            file.OpenBracket();

            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                WriteDeleteByKeyMethod();

                file.AppendLine();
                SelectByKeyMethodWriter.Write(Context);

                UpdateByPrimaryKeyMethodWriter.Write(Context);
            }

            SelectByIndexMethodWriter.Write(Context);

            SelectAllMethodWriter.Write(Context);

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                SelectAllByValidFlagMethodWriter.Write(Context);
            }

            InsertMethodWriter.Write(Context);

            ReadContractMethodWriter.Write(Context);

            file.CloseBracket();
        }

        void WriteDeleteByKeyMethod()
        {
            var deleteInfo = DeleteInfoCreator.Create(tableInfo);

            var sqlParameters = deleteInfo.SqlParameters;

            var parameterPart = String.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine($"public static SqlInfo Delete({parameterPart})");
            file.OpenBracket();

            file.AppendLine($"const string sql = \"{deleteInfo.Sql}\";");
            file.AppendLine();

            file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                file.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            file.AppendLine();
            file.AppendLine("return sqlInfo;");

            file.CloseBracket();
        }

        void WriteEmbeddedClasses()
        {

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            file.AppendAll(System.IO.File.ReadAllText(path));
            file.AppendLine();
        }

        void WriteUsingList()
        {

            foreach (var line in namingPattern.SharedRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}