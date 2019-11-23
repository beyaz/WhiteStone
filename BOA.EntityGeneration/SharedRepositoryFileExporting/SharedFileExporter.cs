using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.DbModel.Interfaces;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using BOA.EntityGeneration.SharedRepositoryFileExporting.MethodWriters;
using BOA.EntityGeneration.Util;
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

         void WriteSelectByIndexMethods()
        {

            var allIndexes = new List<IIndexInfo>();
            allIndexes.AddRange(tableInfo.NonUniqueIndexInfoList);
            allIndexes.AddRange(tableInfo.UniqueIndexInfoList);

            foreach (var indexIdentifier in allIndexes)
            {
                var indexInfo     = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);
                var methodName    = "SelectBy" + String.Join(String.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));
                var parameterPart = String.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

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
                SelectAllByValidFlagMethodWriter.Write(Context);
            }

            InsertMethodWriter.Write(Context);

            ReadContractMethodWriter.Write(Context);

            file.CloseBracket();
        }
          void WriteSelectByKeyMethod()
        {
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

            var sqlParameters = selectByPrimaryKeyInfo.SqlParameters;
            var parameterPart = String.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

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