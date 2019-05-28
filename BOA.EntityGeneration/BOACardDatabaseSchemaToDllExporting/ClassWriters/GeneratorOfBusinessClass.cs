using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using Ninject;
using InsertInfoCreator = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess.InsertInfoCreator;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    public class GeneratorOfBusinessClass
    {
        #region Public Properties
        [Inject]
        public InsertInfoCreator InsertInfoCreator { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }
        #endregion

        #region Public Methods
        public void WriteClass(PaddedStringBuilder sb, TableInfo tableInfo)
        {
            var          typeContractName      = $"{tableInfo.TableName.ToContractName()}Contract";
            var          className             = tableInfo.TableName.ToContractName();
            const string contractParameterName = "contract";

            if (typeContractName == "TransactionLogContract") // resolve conflig
            {
                typeContractName = $"{NamingHelper.GetTypeClassNamespace(tableInfo.SchemaName)}.TransactionLogContract";
            }

            var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(tableInfo.SchemaName);

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public sealed class {className} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public {className}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (tableInfo.IsSupportSelectByKey)
            {
                var deleteInfo = DeleteInfoCreator.Create(tableInfo);

                var parameterPart = string.Join(", ", deleteInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{tableInfo.SchemaName}.{tableInfo.TableName}' by using '{string.Join(" and ", deleteInfo.SqlParameters.Select(x => x.ColumnName.AsMethodParameter()))}'");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<int>(\"{businessClassNamespace}.{className}.Delete\");");

                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(deleteInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (deleteInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in deleteInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                    }
                }

                sb.AppendLine();
                sb.AppendLine("var response = DBLayer.ExecuteNonQuery(command);");
                sb.AppendLine("if (!response.Success)");
                sb.AppendLine("{");
                sb.AppendLine("    returnObject.Results.AddRange(response.Results);");
                sb.AppendLine("    return returnObject;");
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("returnObject.Value = response.Value;");
                sb.AppendLine();
                sb.AppendLine("return returnObject;");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            #endregion

            #region Insert
            if (true)
            {
                var insertInfo = InsertInfoCreator.Create(tableInfo);

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
                if (tableInfo.SequenceName.HasValue())
                {
                    sb.AppendLine($"///{Padding.ForComment} <para>Automatically initialize RecordId property by using {tableInfo.SequenceName} sequence.</para>");
                }

                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("if (contract == null)");
                sb.AppendLine("{");
                sb.AppendLine("    return this.ContractCannotBeNull();");
                sb.AppendLine("}");

                if (tableInfo.SequenceName.HasValue())
                {
                    sb.AppendLine();
                    sb.AppendLine("// Init Sequence");
                    sb.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {tableInfo.SequenceName}\";");
                    sb.AppendLine();
                    sb.AppendLine($"var commandNextSequence = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sqlNextSequence, null, CommandType.Text);");
                    sb.AppendLine();
                    sb.AppendLine("var responseSequence = DBLayer.ExecuteScalar<long>(commandNextSequence);");
                    sb.AppendLine("if (!responseSequence.Success)");
                    sb.AppendLine("{");
                    sb.AppendLine("    return this.SequenceFetchError(responseSequence);");
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine($"{contractParameterName}.RecordId = responseSequence.Value;");

                }

                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(insertInfo.Sql);
                sb.AppendLine();
                if (tableInfo.HasIdentityColumn)
                {
                    sb.AppendLine("SELECT CAST(SCOPE_IDENTITY() AS INT)");
                }
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (insertInfo.SqlParameters.Any())
                {
                    var contractInitializations = new List<string>();

                    if (insertInfo.SqlParameters.Any(c=>c.ColumnName == Names2.ROW_GUID))
                    {
                        contractInitializations.Add($"{contractParameterName}.{Names2.ROW_GUID.ToContractName()} = Guid.NewGuid().ToString().ToUpper(new System.Globalization.CultureInfo(\"en-US\", false));");
                    }
                    if (insertInfo.SqlParameters.Any(c=>c.ColumnName == Names2.INSERT_DATE))
                    {
                        contractInitializations.Add($"{contractParameterName}.{Names2.INSERT_DATE.ToContractName()} = DateTime.Now;");
                    }
                    if (insertInfo.SqlParameters.Any(c=>c.ColumnName == Names2.INSERT_USER_ID))
                    {
                        contractInitializations.Add($"{contractParameterName}.{Names2.INSERT_USER_ID.ToContractName()} = Context.ApplicationContext.Authentication.UserName;");
                    }

                    var tokenIdColumn = insertInfo.SqlParameters.FirstOrDefault(c => c.ColumnName == Names2.INSERT_TOKEN_ID);
                    if (tokenIdColumn!= null)
                    {
                        if (tokenIdColumn.DotNetType == DotNetTypeName.DotNetInt32||
                            tokenIdColumn.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                        {
                            contractInitializations.Add($"{contractParameterName}.{tokenIdColumn.ColumnName.ToContractName()} = decimal.ToInt32(Context.EngineContext.MainBusinessKey);");    
                        }
                        else if (tokenIdColumn.DotNetType == DotNetTypeName.DotNetStringName)
                        {
                            contractInitializations.Add($"{contractParameterName}.{tokenIdColumn.ColumnName.ToContractName()} = Context.EngineContext.MainBusinessKey.ToString();");    
                        }
                        else
                        {
                            throw new NotImplementedException(tokenIdColumn.DotNetType);
                        }
                    }

                    if (contractInitializations.Any())
                    {
                        sb.AppendLine();
                        foreach (var item in contractInitializations)
                        {
                            sb.AppendLine(item);
                        }
                    }

                    sb.AppendLine();
                    foreach (var columnInfo in insertInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
                    }
                }

                sb.AppendLine();
                if (tableInfo.HasIdentityColumn)
                {
                    sb.AppendLine("return this.ExecuteScalar(command);");
                }
                else
                {
                    sb.AppendLine("return this.ExecuteNonQuery(command);");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            #endregion

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();

                Update(sb, tableInfo, typeContractName, contractParameterName);
            }
            #endregion

            #region SelectByKey
            if (tableInfo.IsSupportSelectByKey)
            {
                var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

                var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(selectByPrimaryKeyInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (selectByPrimaryKeyInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in selectByPrimaryKeyInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                    }
                }

                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteReaderForOnlyOneRecord<{typeContractName}>(command, ReadContract);");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            #endregion

            #region SelectByUniqueIndex
            if (tableInfo.IsSupportSelectByUniqueIndex)
            {
                foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
                {
                    var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                    var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                    var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine($"///{Padding.ForComment} Selects records by given parameters.");
                    sb.AppendLine("/// </summary>");
                    sb.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine("const string sql = @\"");
                    sb.AppendAll(indexInfo.Sql);
                    sb.AppendLine();
                    sb.AppendLine("\";");
                    sb.AppendLine();
                    sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

                    if (indexInfo.SqlParameters.Any())
                    {
                        sb.AppendLine();
                        foreach (var columnInfo in indexInfo.SqlParameters)
                        {
                            sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine($"return this.ExecuteReaderForOnlyOneRecord<{typeContractName}>(command, ReadContract);");


                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }
            }
            #endregion

            #region SelectByNonUniqueIndex
            if (tableInfo.IsSupportSelectByIndex)
            {
                foreach (var indexIdentifier in tableInfo.NonUniqueIndexInfoList)
                {
                    var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                    var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                    var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine($"///{Padding.ForComment} Selects records by given parameters");
                    sb.AppendLine("/// </summary>");
                    sb.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine("const string sql = @\"");
                    sb.AppendAll(indexInfo.Sql);
                    sb.AppendLine();
                    sb.AppendLine("\";");
                    sb.AppendLine();
                    sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

                    if (indexInfo.SqlParameters.Any())
                    {
                        sb.AppendLine();
                        foreach (var columnInfo in indexInfo.SqlParameters)
                        {
                            sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine($"return this.ExecuteReader<{typeContractName}>(command, ReadContract);");


                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }
            }
            #endregion

            #region ReadContract
            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Reads one record from reader");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataReader reader, {typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var columnInfo in tableInfo.Columns)
            {
                if (columnInfo.SqlDbType == SqlDbType.Char &&
                    columnInfo.DotNetType == DotNetTypeName.DotNetBool)
                {
                    sb.AppendLine($"{contractParameterName}.{columnInfo.ColumnName.ToContractName()} = SQLDBHelper.GetStringValue(reader[\"{columnInfo.ColumnName}\"]) == \"1\";");
                }
                else if (columnInfo.SqlDbType == SqlDbType.Char &&
                         columnInfo.DotNetType == DotNetTypeName.DotNetBoolNullable)
                {
                    sb.AppendLine($"{contractParameterName}.{columnInfo.ColumnName.ToContractName()} = Util.ReadNullableFlag(SQLDBHelper.GetStringValue(reader[\"{columnInfo.ColumnName}\"]));");
                }
                else
                {
                    sb.AppendLine($"{contractParameterName}.{columnInfo.ColumnName.ToContractName()} = SQLDBHelper.{columnInfo.SqlReaderMethod}(reader[\"{columnInfo.ColumnName}\"]);");
                }
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            var selectAllInfo = SelectAllInfoCreator.Create(tableInfo);

            sb.AppendLine();
            SelectAll(sb, tableInfo, typeContractName);
                
            if (tableInfo.ShouldGenerateSelectAllByValidFlagMethodInBusinessClass)
            {
                    
                sb.AppendLine();
                SelectByValidFlag(sb, tableInfo, typeContractName, selectAllInfo);
            }

            sb.AppendLine();
            GetDbColumnInfo(sb,tableInfo,typeContractName);
            sb.AppendLine();
            SelectByWhereConditions(sb, typeContractName, businessClassNamespace, className, selectAllInfo.Sql);

            sb.AppendLine();
            var selectTopNRecordsSql = new PaddedStringBuilder();
            SelectAllInfoCreator.WriteSql(tableInfo, selectTopNRecordsSql, ParameterIdentifier + TopCountParameterName);
            SelectTopNByWhereConditions(sb, typeContractName, businessClassNamespace, className, selectTopNRecordsSql.ToString());


            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        static void Update(PaddedStringBuilder sb, TableInfo tableInfo, string typeContractName, string contractParameterName)
        {
            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Updates records by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Update({typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (contract == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull();");
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(updateInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();
            sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

            if (updateInfo.SqlParameters.Any())
            {
                var contractInitializations = new List<string>();

                if (updateInfo.SqlParameters.Any(c => c.ColumnName == Names2.UPDATE_DATE))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.UPDATE_DATE.ToContractName()} = DateTime.Now;");
                }

                if (updateInfo.SqlParameters.Any(c => c.ColumnName == Names2.UPDATE_USER_ID))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.UPDATE_USER_ID.ToContractName()} = Context.ApplicationContext.Authentication.UserName;");
                }

                var tokenIdColumn = updateInfo.SqlParameters.FirstOrDefault(c => c.ColumnName == Names2.UPDATE_TOKEN_ID);
                if (tokenIdColumn != null)
                {
                    if (tokenIdColumn.DotNetType == DotNetTypeName.DotNetInt32 ||
                        tokenIdColumn.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                    {
                        contractInitializations.Add($"{contractParameterName}.{tokenIdColumn.ColumnName.ToContractName()} = decimal.ToInt32(Context.EngineContext.MainBusinessKey);");
                    }
                    else if (tokenIdColumn.DotNetType == DotNetTypeName.DotNetStringName)
                    {
                        contractInitializations.Add($"{contractParameterName}.{tokenIdColumn.ColumnName.ToContractName()} = Context.EngineContext.MainBusinessKey.ToString();");
                    }
                    else
                    {
                        throw new NotImplementedException(tokenIdColumn.DotNetType);
                    }
                }

                if (contractInitializations.Any())
                {
                    sb.AppendLine();
                    foreach (var item in contractInitializations)
                    {
                        sb.AppendLine(item);
                    }
                }

                sb.AppendLine();
                foreach (var columnInfo in updateInfo.SqlParameters)
                {
                    sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlUpdate(columnInfo)});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return this.ExecuteNonQuery(command);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        const string TopCountParameterName = "$resultCount";
        const string ParameterIdentifier = "@";

        public void WriteUsingList(PaddedStringBuilder sb, TableInfo tableInfo)
        {
            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine($"using {NamingHelper.GetTypeClassNamespace(tableInfo.SchemaName)};");
            sb.AppendLine("using BOA.Types.Kernel.Card.EntityConditions;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Runtime.CompilerServices;");
            sb.AppendLine("using System.Text;");
            

            
            
        }
        #endregion

        #region Methods
        static void SelectAll(PaddedStringBuilder sb, TableInfo tableInfo, string typeContractName)
        {
            var selectAllInfo = SelectAllInfoCreator.Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName}");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(selectAllInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();
            sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

            if (selectAllInfo.SqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in selectAllInfo.SqlParameters)
                {
                    sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteReader<{typeContractName}>(command, ReadContract);");
           

            sb.PaddingCount--;
            sb.AppendLine("}");
        }


        static void SelectByValidFlag(PaddedStringBuilder sb, TableInfo tableInfo, string typeContractName, SelectAllInfo selectAllInfo)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName} where ValidFlag is true.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectByValidFlag()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(selectAllInfo.Sql + " WHERE [VALID_FLAG] = '1'");
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();
            sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

            if (selectAllInfo.SqlParameters.Any())
            {
                sb.AppendLine();
                foreach (var columnInfo in selectAllInfo.SqlParameters)
                {
                    sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"return this.ExecuteReader<{typeContractName}>(command, ReadContract);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion

        

        static void GetDbColumnInfo(PaddedStringBuilder sb, TableInfo tableInfo, string typeContractName)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Gets the database column information.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static DbColumnInfo GetDbColumnInfo(string propertyNameInContract)");
            sb.AppendLine("{");
            sb.PaddingCount++;
            
            foreach (var columnInfo in tableInfo.Columns)
            {
                sb.AppendAll($@"

if (propertyNameInContract == nameof({typeContractName}.{columnInfo.ColumnName.ToContractName()}))
{{
    return new DbColumnInfo
    {{
        Name      = ""{columnInfo.ColumnName}"",
        SqlDbType = SqlDbType.{columnInfo.SqlDbType.ToString()}
    }};
}}

".Trim());
                sb.AppendLine();
            }


            sb.AppendLine();
            sb.AppendLine("throw new ArgumentException(propertyNameInContract);");
           

            sb.PaddingCount--;
            sb.AppendLine("}");
        }


        static void SelectByWhereConditions(PaddedStringBuilder sb, string typeContractName, string businessClassNamespace, string className, string sql)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Selects records by given  <paramref name=\"whereConditions\"/>.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> Select(params WhereCondition[] whereConditions)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var returnObject = InitializeGenericResponse<List<{typeContractName}>>(\"{businessClassNamespace}.{className}.Select\");");
            
            sb.AppendAll(@"

if (whereConditions == null || whereConditions.Length == 0)
{
    return this.WhereConditionsCannotBeNullOrEmpty<"+typeContractName+@">();
}

const string sqlSelectPart = @""
"+sql+@"
"";

var sql = new StringBuilder(sqlSelectPart);

var parameters = new List<DbParameterInfo>();

var whereLines = new List<string>();

foreach (var whereCondition in whereConditions)
{
    var dbColumn = GetDbColumnInfo(whereCondition.ColumnName);

    Util.ProcessCondition(whereCondition, whereLines, dbColumn, parameters);
}

sql.AppendLine(Util.WhereSymbol);

sql.AppendLine(whereLines[0]);

for (var i = 1; i < whereLines.Count; i++)
{
    sql.AppendLine(Util.AndSymbol + whereLines[i]);
}

var command = DBLayer.GetDBCommand(Databases.BOACard, sql.ToString(), null, CommandType.Text);

foreach (var parameter in parameters)
{
    DBLayer.AddInParameter(command, ""@""+parameter.Name, parameter.SqlDbType, parameter.Value );    
}

return this.ExecuteReader<"+typeContractName+@">(command, ReadContract);

".Trim());
            sb.AppendLine();

           

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        

        static void SelectTopNByWhereConditions(PaddedStringBuilder sb, string typeContractName, string businessClassNamespace, string className, string sql)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Selects top resultCount records by given  <paramref name=\"whereConditions\"/>.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectTop(int resultCount, params WhereCondition[] whereConditions)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var returnObject = InitializeGenericResponse<List<{typeContractName}>>(\"{businessClassNamespace}.{className}.SelectTop\");");
            
            sb.AppendAll(@"

if (whereConditions == null || whereConditions.Length == 0)
{
    return this.WhereConditionsCannotBeNullOrEmpty<"+typeContractName+@">();
}

const string sqlSelectPart = @""
"+sql+@"
"";

var sql = new StringBuilder(sqlSelectPart);

var parameters = new List<DbParameterInfo>
{
    new DbParameterInfo
	{
		Name      = ""$resultCount"",
		SqlDbType = SqlDbType.Int,
		Value     = resultCount
	}
};

var whereLines = new List<string>();

foreach (var whereCondition in whereConditions)
{
    var dbColumn = GetDbColumnInfo(whereCondition.ColumnName);

    Util.ProcessCondition(whereCondition, whereLines, dbColumn, parameters);
}

sql.AppendLine(Util.WhereSymbol);

sql.AppendLine(whereLines[0]);

for (var i = 1; i < whereLines.Count; i++)
{
    sql.AppendLine(Util.AndSymbol + whereLines[i]);
}

var command = DBLayer.GetDBCommand(Databases.BOACard, sql.ToString(), null, CommandType.Text);

foreach (var parameter in parameters)
{
    DBLayer.AddInParameter(command, ""@""+parameter.Name, parameter.SqlDbType, parameter.Value );    
}

return this.ExecuteReader<"+typeContractName+@">(command, ReadContract);

".Trim());
            sb.AppendLine();

           

            sb.PaddingCount--;
            sb.AppendLine("}");
        }


    }
}