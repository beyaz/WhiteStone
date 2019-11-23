using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BoaRepositoryFileExporting.MethodWriters;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using static BOA.EntityGeneration.DataFlow.SchemaExportingEvent;
using static BOA.EntityGeneration.DataFlow.TableExportingEvent;

namespace BOA.EntityGeneration.BoaRepositoryFileExporting
{
    class BoaRepositoryFileExporter:ContextContainer
    {


        void WriteSelectByIndexMethods()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;

            foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>( CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }

            foreach (var indexIdentifier in tableInfo.NonUniqueIndexInfoList)
            {
                var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                file.AppendLine();
                file.AppendLine("/// <summary>");
                file.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                file.AppendLine("/// </summary>");
                file.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                file.OpenBracket();

                file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.{methodName}({string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");

                file.AppendLine();
                file.AppendLine($"const string CallerMemberPath = \"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.{methodName}\";");
                file.AppendLine();
                file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

                file.CloseBracket();
            }
        }


        void WriteSelectByKeyMethod()
        {
            var typeContractName = tableEntityClassNameForMethodParametersInRepositoryFiles;
            var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.SelectByKey";

            var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.SelectByKey({string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine($"return this.ExecuteReaderToContract<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

            file.PaddingCount--;
            file.AppendLine("}");
        }


        void WriteDeleteByKeyMethod()
        {
            var deleteByKeyInfo    = DeleteInfoCreator.Create(tableInfo);

            var sqlParameters = deleteByKeyInfo.SqlParameters;

            var tableName = tableInfo.TableName;

            var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Delete";

            var parameterPart = string.Join(", ", sqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

            file.AppendLine();
            file.AppendLine("/// <summary>");
            file.AppendLine($"///{Padding.ForComment}Deletes only one record from '{schemaName}.{tableName}' by using '{string.Join(" and ", sqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
            file.AppendLine("/// </summary>");
            file.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
            file.AppendLine("{");
            file.PaddingCount++;

            file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Delete({string.Join(", ", sqlParameters.Select(x => $"{x.ColumnName.AsMethodParameter()}"))});");
            file.AppendLine();
            file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
            file.AppendLine();
            file.AppendLine("return this.ExecuteNonQuery(CallerMemberPath, sqlInfo);");

            file.PaddingCount--;
            file.AppendLine("}");
        }


        #region Fields
        readonly PaddedStringBuilder file;
        #endregion

        #region Constructors
        public BoaRepositoryFileExporter()
        {
            file = new PaddedStringBuilder();
        }
        #endregion


        #region Static Fields
        internal static readonly Property<PaddedStringBuilder> File = Property.Create<PaddedStringBuilder>(nameof(File));
        #endregion

        #region Public Methods
        public  void AttachEvents()
        {
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
            

            file.BeginNamespace(namingPattern.RepositoryNamespace);
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
            

            processInfo.Text = "Exporting Boa repository...";

            FileSystem.WriteAllText(Context, namingPattern.RepositoryProjectDirectory + "Boa.cs", sourceCode);
        }

        

         void WriteClass()
         {
             var context = Context;

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine($"public sealed class {tableNamingPattern.BoaRepositoryClassName} : ObjectHelper");
            file.AppendLine("{");
            file.PaddingCount++;

            ContractCommentInfoCreator.Write(file, tableInfo);
            file.AppendLine($"public {tableNamingPattern.BoaRepositoryClassName}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                WriteDeleteByKeyMethod();
            }
            #endregion

            InsertMethodWriter.Write(context);

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                UpdateByKeyMethodWriter.Write(context);
            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {
                file.AppendLine();
                WriteSelectByKeyMethod();
            }
            #endregion

            WriteSelectByIndexMethods();

            WriteSelectAllMethod();

            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                SelectAllByValidFlagMethodWriter.Write(context);
            }

            file.CloseBracket();
        }


         void WriteSelectAllMethod()
         {
             var typeContractName   = tableEntityClassNameForMethodParametersInRepositoryFiles;

             var callerMemberPath = $"{namingPattern.RepositoryNamespace}.{tableNamingPattern.BoaRepositoryClassName}.Select";

             file.AppendLine();
             file.AppendLine("/// <summary>");
             file.AppendLine($"///{Padding.ForComment} Selects all records from table '{schemaName}.{tableInfo.TableName}'.");
             file.AppendLine("/// </summary>");
             file.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
             file.OpenBracket();

             file.AppendLine($"var sqlInfo = {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.Select();");
             file.AppendLine();
             file.AppendLine($"const string CallerMemberPath = \"{callerMemberPath}\";");
             file.AppendLine();
             file.AppendLine($"return this.ExecuteReaderToList<{typeContractName}>(CallerMemberPath, sqlInfo, {tableNamingPattern.SharedRepositoryClassNameInBoaRepositoryFile}.ReadContract);");

             file.CloseBracket();
         }

         void WriteEmbeddedClasses()
        {
            

            var path = Path.GetDirectoryName(typeof(BoaRepositoryFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "BoaRepositoryFileEmbeddedCodes.txt";

            file.AppendAll(System.IO.File.ReadAllText(path));
            file.AppendLine();
        }

         void WriteUsingList()
        {
            


            foreach (var line in namingPattern.BoaRepositoryUsingLines)
            {
                file.AppendLine(line);
            }
        }
        #endregion
    }
}