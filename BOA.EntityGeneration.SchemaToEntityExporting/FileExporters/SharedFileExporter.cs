using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.EntityGeneration.SchemaToEntityExporting.Models;
using BOA.EntityGeneration.SchemaToEntityExporting.Util;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.SchemaToEntityExporting.FileExporters
{
    class SharedFileExporter : ContextContainer
    {
        #region Fields
        readonly PaddedStringBuilder file;
        #endregion

        #region Constructors
        public SharedFileExporter()
        {
            file = new PaddedStringBuilder();
        }
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            SchemaExportStarted += WriteUsingList;
            SchemaExportStarted += EmptyLine;
            SchemaExportStarted += BeginNamespace;
            SchemaExportStarted += WriteEmbeddedClasses;

            TableExportStarted += WriteClass;

            SchemaExportFinished += EndNamespace;
            SchemaExportFinished += ExportFileToDirectory;
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
            var sourceCode = file.ToString();

            Context.OnSharedRepositoryFileContentCompleted(sourceCode);

            processInfo.Text = "Exporting Shared repository...";

            FileSystem.WriteAllText(namingPattern.RepositoryProjectDirectory + "Shared.cs", sourceCode);
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
                WriteSelectByKeyMethod();

                WriteUpdateByPrimaryKeyMethod();
            }

            WriteSelectByIndexMethods();

            WriteSelectAllMethod();

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                WriteSelectAllByValidFlagMethod();
            }

            WriteInsertMethod();

            WriteReadContractMethod();

            file.CloseBracket();
        }

        void WriteDeleteByKeyMethod()
        {
            var deleteInfo = DeleteInfoCreator.Create(tableInfo);

            var sqlParameters = deleteInfo.SqlParameters;

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

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

            file.AppendAll(File.ReadAllText(path));
            file.AppendLine();
        }

        void WriteInsertMethod()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            var sqlParameters = insertInfo.SqlParameters;

            file.AppendLine($"public static SqlInfo Insert({typeContractName} contract)");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(insertInfo.Sql);
            file.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                file.AppendLine("SELECT CAST(SCOPE_IDENTITY() AS INT)");
            }

            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                file.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
                }
            }

            file.AppendLine();
            file.AppendLine("return sqlInfo;");

            file.CloseBracket();
        }

        void WriteReadContractMethod()
        {
            const string contractParameterName = "contract";

            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Reads one record from reader");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public static void ReadContract(IDataReader reader, {typeContractName} {contractParameterName})");
            file.AppendLine("{");
            file.PaddingCount++;

            foreach (var columnInfo in tableInfo.Columns)
            {
                var readerMethodName = columnInfo.SqlReaderMethod.ToString();
                if (columnInfo.SqlReaderMethod == SqlReaderMethods.GetGUIDValue)
                {
                    readerMethodName = "GetGuidValue";
                }

                var contractReadLine = config.ContractReadLine
                                             .Replace("$(Contract)", contractParameterName)
                                             .Replace("$(PropertyName)", columnInfo.ColumnName.ToContractName())
                                             .Replace("$(ColumnName)", columnInfo.ColumnName)
                                             .Replace("$(SqlReaderMethod)", readerMethodName);

                file.AppendLine(contractReadLine);
            }

            file.PaddingCount--;
            file.AppendLine("}");
        }

        void WriteSelectAllByValidFlagMethod()
        {
            var sql = SelectAllInfoCreator.Create(tableInfo).Sql;

            file.AppendLine("public static SqlInfo SelectByValidFlag()");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(sql + " WHERE [VALID_FLAG] = '1'");
            file.AppendLine();
            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("return new SqlInfo { CommandText = sql };");
            file.CloseBracket();
        }

        void WriteSelectAllMethod()
        {
            var sql = SelectAllInfoCreator.Create(tableInfo).Sql;

            file.AppendLine("public static SqlInfo Select()");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(sql);
            file.AppendLine();
            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("return new SqlInfo { CommandText = sql };");
            file.CloseBracket();
        }

        void WriteSelectByIndexMethods()
        {
            var allIndexes = new List<IIndexInfo>();
            allIndexes.AddRange(tableInfo.NonUniqueIndexInfoList);
            allIndexes.AddRange(tableInfo.UniqueIndexInfoList);

            foreach (var indexIdentifier in allIndexes)
            {
                var indexInfo     = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);
                var methodName    = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));
                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public static SqlInfo {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine("const string sql = @\"");
                file.AppendAll(indexInfo.Sql);
                file.AppendLine();
                file.AppendLine("\";");
                file.AppendLine();
                file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

                if (indexInfo.SqlParameters.Any())
                {
                    file.AppendLine();
                    foreach (var columnInfo in indexInfo.SqlParameters)
                    {
                        file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                    }
                }

                file.AppendLine();
                file.AppendLine("return sqlInfo;");

                file.CloseBracket();
            }
        }

        void WriteSelectByKeyMethod()
        {
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

            var sqlParameters = selectByPrimaryKeyInfo.SqlParameters;
            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine($"public static SqlInfo SelectByKey({parameterPart})");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(selectByPrimaryKeyInfo.Sql);
            file.AppendLine();
            file.AppendLine("\";");
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

        void WriteUpdateByPrimaryKeyMethod()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            var sqlParameters = updateInfo.SqlParameters;

            file.AppendLine($"public static SqlInfo Update({typeContractName} contract)");
            file.OpenBracket();

            file.AppendLine("const string sql = @\"");
            file.AppendAll(updateInfo.Sql);
            file.AppendLine();
            file.AppendLine("\";");
            file.AppendLine();

            file.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (sqlParameters.Any())
            {
                file.AppendLine();
                foreach (var columnInfo in sqlParameters)
                {
                    file.AppendLine($"sqlInfo.AddInParameter(\"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlUpdate(columnInfo)});");
                }
            }

            file.AppendLine();
            file.AppendLine("return sqlInfo;");

            file.CloseBracket();
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