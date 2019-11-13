﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ___Company___.DataFlow;
using ___Company___.EntityGeneration.DataFlow;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Models.Interfaces;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using InsertInfoCreator = BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess.InsertInfoCreator;
using static ___Company___.EntityGeneration.DataFlow.DataContext;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{

    public class BusinessClassWriterContext
    {
        public string TypeContractName { get; set; }
        public string ClassName { get; set; }
        public string BusinessClassNamespace { get; set; }
        public bool CanWriteDeleteByKeyMethod { get; set; }
        public IDeleteInfo DeleteByKeyInfo { get; set; }
        public ITableInfo TableInfo { get; set; }
        public bool CanWriteSelectByKeyMethod { get; set; }
        public ISelectByPrimaryKeyInfo SelectByPrimaryKeyInfo { get; set; }
        public SharedClassConfig SharedClassConfig { get; set; }
    }

    /// <summary>
    ///     The generator of business class
    /// </summary>
    public class GeneratorOfBusinessClass
    {
        #region Constants
        /// <summary>
        ///     The contract parameter name
        /// </summary>
        const string contractParameterName = "contract";

        /// <summary>
        ///     The parameter identifier
        /// </summary>
        const string ParameterIdentifier = "@";

        /// <summary>
        ///     The parameter name of column names
        /// </summary>
        const string ParameterNameOf_columnNames = "columnNames";

        /// <summary>
        ///     The top count parameter name
        /// </summary>
        const string TopCountParameterName = "$resultCount";
        #endregion

       

        #region Public Methods
        /// <summary>
        ///     Writes the class.
        /// </summary>
        public static void WriteClass(IDataContext Context)
        {
            var sb = Context.Get(Data.BoaRepositoryClassesOutput);
            var tableInfo = Context.Get(Data.TableInfo);
            var config = Context.Get(Data.Config);


            var typeContractName = $"{tableInfo.TableName.ToContractName()}Contract";
            if (typeContractName == "TransactionLogContract" ||
                typeContractName == "BoaUserContract") // resolve conflig
            {
                typeContractName = $"{NamingHelper.GetTypeClassNamespace(tableInfo.SchemaName,config)}.{typeContractName}";
            }

            var className = tableInfo.TableName.ToContractName();

            var businessClassNamespace = NamingHelper.GetBusinessClassNamespace(tableInfo.SchemaName,config);

            var businessClassWriterContext = new BusinessClassWriterContext
            {
                TypeContractName = typeContractName,
                ClassName = className,
                BusinessClassNamespace = businessClassNamespace,
                CanWriteDeleteByKeyMethod = tableInfo.IsSupportSelectByKey,
                CanWriteSelectByKeyMethod = tableInfo.IsSupportSelectByKey,
                TableInfo = tableInfo,
                SharedClassConfig = config.SharedClassConfig
            };
            if (businessClassWriterContext.CanWriteDeleteByKeyMethod)
            {
                businessClassWriterContext.DeleteByKeyInfo = DeleteInfoCreator.Create(tableInfo);
            }
            if (businessClassWriterContext.CanWriteSelectByKeyMethod)
            {
                businessClassWriterContext.SelectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);
            }

            Context.Add(Data.BusinessClassWriterContext,businessClassWriterContext);
           
            
            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public sealed class {className} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public {className}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (businessClassWriterContext.CanWriteDeleteByKeyMethod)
            {
                sb.AppendLine();
                MethodWriters.BoaSystem.DeleteByKeyMethodWriter.Write(sb,businessClassWriterContext);
            }
            #endregion

            #region Insert
            if (true)
            {
                sb.AppendLine();

                Insert(sb, tableInfo, typeContractName);
            }
            #endregion

            #region Update
            if (tableInfo.IsSupportSelectByKey)
            {
                sb.AppendLine();
                Update(sb, tableInfo, typeContractName);

                sb.AppendLine();
                UpdateSpecificColumns(sb, tableInfo, typeContractName);
            }
            #endregion

            #region SelectByKey
            if (businessClassWriterContext.CanWriteSelectByKeyMethod)
            {

                sb.AppendLine();
                MethodWriters.BoaSystem.SelectByKeyMethodWriter.Write(sb,businessClassWriterContext);

                


                

                //var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                //sb.AppendLine();
                //sb.AppendLine("/// <summary>");
                //sb.AppendLine($"///{Padding.ForComment} Selects record by primary keys.");
                //sb.AppendLine("/// </summary>");
                //sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
                //sb.AppendLine("{");
                //sb.PaddingCount++;

                //sb.AppendLine("const string sql = @\"");
                //sb.AppendAll(selectByPrimaryKeyInfo.Sql);
                //sb.AppendLine();
                //sb.AppendLine("\";");
                //sb.AppendLine();
                //sb.AppendLine("var command = this.CreateCommand(sql);");

                //if (selectByPrimaryKeyInfo.SqlParameters.Any())
                //{
                //    sb.AppendLine();
                //    foreach (var columnInfo in selectByPrimaryKeyInfo.SqlParameters)
                //    {
                //        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
                //    }
                //}

                //sb.AppendLine();
                //sb.AppendLine($"return this.ExecuteReaderForOnlyOneRecord<{typeContractName}>(command, ReadContract);");

                //sb.PaddingCount--;
                //sb.AppendLine("}");
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
                    sb.AppendLine("var command = this.CreateCommand(sql);");

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
                    sb.AppendLine("var command = this.CreateCommand(sql);");

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
                var readerLine = config.ReadLineDefault;

                if (columnInfo.SqlDbType == SqlDbType.Char &&
                    columnInfo.DotNetType == DotNetTypeName.DotNetBool)
                {
                    readerLine = config.ReadLineCharToBool;
                }
                else if (columnInfo.SqlDbType == SqlDbType.Char &&
                         columnInfo.DotNetType == DotNetTypeName.DotNetBoolNullable)
                {
                    readerLine = config.ReadLineCharToBoolNullable;
                }

                readerLine = readerLine
                             .Replace("{Contract}", contractParameterName)
                             .Replace("{PropertyName}", columnInfo.ColumnName.ToContractName())
                             .Replace("{ColumnName}", columnInfo.ColumnName)
                             .Replace("{SqlReaderMethod}", columnInfo.SqlReaderMethod.ToString());

                sb.AppendLine(readerLine);
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
            GetDbColumnInfo(sb, tableInfo, typeContractName);
            sb.AppendLine();
            SelectByWhereConditions(sb, typeContractName, businessClassNamespace, className, selectAllInfo.Sql);

            sb.AppendLine();
            var selectTopNRecordsSql = new PaddedStringBuilder();
            SelectAllInfoCreator.WriteSql(tableInfo, selectTopNRecordsSql, ParameterIdentifier + TopCountParameterName);
            SelectTopNByWhereConditions(sb, typeContractName, businessClassNamespace, className, selectTopNRecordsSql.ToString());

            sb.PaddingCount--;
            sb.AppendLine("}");

            
            Context.Remove(Data.BusinessClassWriterContext);
        }

        /// <summary>
        ///     Writes the using list.
        /// </summary>
        public static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(Data.BoaRepositoryClassesOutput);
            var schemaName = context.Get(Data.SchemaName);
            var config = context.Get(Data.Config);

            foreach (var line in config.DaoUsingLines)
            {
                sb.AppendLine(line.Replace("{SchemaName}", schemaName));
            }

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Runtime.CompilerServices;");
            sb.AppendLine("using System.Text;");
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Gets the database column information.
        /// </summary>
        static void GetDbColumnInfo(PaddedStringBuilder sb, ITableInfo tableInfo, string typeContractName)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Gets the database column information.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine("static DbColumnInfo GetDbColumnInfo(string propertyNameInContract)");
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

        /// <summary>
        ///     Selects all.
        /// </summary>
        static void SelectAll(PaddedStringBuilder sb, ITableInfo tableInfo, string typeContractName)
        {
            var selectAllInfo = SelectAllInfoCreator.Create(tableInfo);


            var template = new SelectAllMethodTemplate
            {
                Session = new Dictionary<string, object>
                {
                    { nameof(selectAllInfo),selectAllInfo},
                    {nameof(tableInfo),tableInfo },
                    { nameof(typeContractName),typeContractName}
                }
            };
            template.Initialize();

            var methodText = template.TransformText();

            sb.AppendAll(methodText);
            sb.AppendLine();

            //sb.AppendLine("/// <summary>");
            //sb.AppendLine($"///{Padding.ForComment} Selects all records in table {tableInfo.SchemaName}{tableInfo.TableName}");
            //sb.AppendLine("/// </summary>");
            //sb.AppendLine($"public GenericResponse<List<{typeContractName}>> Select()");
            //sb.AppendLine("{");
            //sb.PaddingCount++;

            //sb.AppendLine("const string sql = @\"");
            //sb.AppendAll(selectAllInfo.Sql);
            //sb.AppendLine();
            //sb.AppendLine("\";");
            //sb.AppendLine();
            //sb.AppendLine("var command = this.CreateCommand(sql);");

            //if (selectAllInfo.SqlParameters.Any())
            //{
            //    sb.AppendLine();
            //    foreach (var columnInfo in selectAllInfo.SqlParameters)
            //    {
            //        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {columnInfo.ColumnName.AsMethodParameter()});");
            //    }
            //}

            //sb.AppendLine();
            //sb.AppendLine($"return this.ExecuteReader<{typeContractName}>(command, ReadContract);");

            //sb.PaddingCount--;
            //sb.AppendLine("}");
        }

        /// <summary>
        ///     Selects the by valid flag.
        /// </summary>
        static void SelectByValidFlag(PaddedStringBuilder sb, ITableInfo tableInfo, string typeContractName, SelectAllInfo selectAllInfo)
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
            sb.AppendLine("var command = this.CreateCommand(sql);");

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

        /// <summary>
        ///     Selects the by where conditions.
        /// </summary>
        static void SelectByWhereConditions(PaddedStringBuilder sb, string typeContractName, string businessClassNamespace, string className, string sql)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Selects records by given  <paramref name=\"whereConditions\"/>.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> Select(params WhereCondition[] whereConditions)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"InitializeGenericResponse<List<{typeContractName}>>(\"{businessClassNamespace}.{className}.Select\");");

            sb.AppendAll(@"

if (whereConditions == null || whereConditions.Length == 0)
{
    return this.WhereConditionsCannotBeNullOrEmpty<" + typeContractName + @">();
}

const string sqlSelectPart = @""
" + sql + @"
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

var command = this.CreateCommand(sql.ToString());

foreach (var parameter in parameters)
{
    DBLayer.AddInParameter(command, ""@""+parameter.Name, parameter.SqlDbType, parameter.Value );    
}

return this.ExecuteReader<" + typeContractName + @">(command, ReadContract);

".Trim());
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        /// <summary>
        ///     Selects the top n by where conditions.
        /// </summary>
        static void SelectTopNByWhereConditions(PaddedStringBuilder sb, string typeContractName, string businessClassNamespace, string className, string sql)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Selects top resultCount records by given  <paramref name=\"whereConditions\"/>.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{typeContractName}>> SelectTop(int resultCount, params WhereCondition[] whereConditions)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"InitializeGenericResponse<List<{typeContractName}>>(\"{businessClassNamespace}.{className}.SelectTop\");");

            sb.AppendAll(@"

if (whereConditions == null || whereConditions.Length == 0)
{
    return this.WhereConditionsCannotBeNullOrEmpty<" + typeContractName + @">();
}

const string sqlSelectPart = @""
" + sql + @"
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

var command = this.CreateCommand(sql.ToString());

foreach (var parameter in parameters)
{
    DBLayer.AddInParameter(command, ""@""+parameter.Name, parameter.SqlDbType, parameter.Value );    
}

return this.ExecuteReader<" + typeContractName + @">(command, ReadContract);

".Trim());
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        /// <summary>
        ///     Updates the specified sb.
        /// </summary>
        static void Update(PaddedStringBuilder sb, ITableInfo tableInfo, string typeContractName)
        {
            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Updates only one record by primary keys.");
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
            sb.AppendLine("var command = this.CreateCommand(sql);");

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

        /// <summary>
        ///     Updates the specific columns.
        /// </summary>
        static void UpdateSpecificColumns(PaddedStringBuilder sb, ITableInfo tableInfo, string typeContractName)
        {
            var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Updates only one record's specific columns by primary keys.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> UpdateSpecificColumns({typeContractName} {contractParameterName}, params string[] {ParameterNameOf_columnNames})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"if ({contractParameterName} == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull();");
            sb.AppendLine("}");

            sb.AppendLine($"if ({ParameterNameOf_columnNames} == null || {ParameterNameOf_columnNames}.Length == 0)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ColumnNamesCannotBeNullOrEmpty();");
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("var sql = new StringBuilder();");

            sb.AppendLine($"sql.Append(\"UPDATE [{tableInfo.SchemaName}].[{tableInfo.TableName}] SET \");");

            foreach (var columnInfo in updateInfo.ColumnsWillBeUpdate)
            {
                sb.AppendLine();
                if (columnInfo.ColumnName == Names2.UPDATE_DATE ||
                    columnInfo.ColumnName == Names2.UPDATE_USER_ID ||
                    columnInfo.ColumnName == Names2.UPDATE_TOKEN_ID)
                {
                    sb.AppendLine($"sql.Append(\"[{columnInfo.ColumnName}] = @{columnInfo.ColumnName},\");");
                    continue;
                }

                sb.AppendLine($"if(Array.IndexOf({ParameterNameOf_columnNames}, nameof({typeContractName}.{columnInfo.ColumnName.ToContractName()})) >= 0)");
                sb.AppendLine("{");
                sb.AppendLine($"    sql.Append(\"[{columnInfo.ColumnName}] = @{columnInfo.ColumnName},\");");
                sb.AppendLine("}");
            }

            sb.AppendLine("sql.Remove(sql.Length - 1, 1);");
            sb.AppendLine("sql.Append(\" WHERE \");");

            foreach (var columnInfo in updateInfo.WhereParameters)
            {
                sb.AppendLine($"sql.Append(\"[{columnInfo.ColumnName}] = @{columnInfo.ColumnName} AND\");");
            }

            sb.AppendLine("sql.Remove(sql.Length - 4, 4);");

            sb.AppendLine();
            sb.AppendLine("var command = this.CreateCommand(sql.ToString());");

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
                    var addParameterLine = $"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlUpdate(columnInfo)});";

                    if (updateInfo.WhereParameters.Any(x => x.ColumnName == columnInfo.ColumnName))
                    {
                        sb.AppendLine(addParameterLine);
                        continue;
                    }

                    if (columnInfo.ColumnName == Names2.UPDATE_DATE ||
                        columnInfo.ColumnName == Names2.UPDATE_USER_ID ||
                        columnInfo.ColumnName == Names2.UPDATE_TOKEN_ID)
                    {
                        sb.AppendLine(addParameterLine);
                        continue;
                    }

                    sb.AppendLine();
                    sb.AppendLine($"if(Array.IndexOf({ParameterNameOf_columnNames}, nameof({typeContractName}.{columnInfo.ColumnName.ToContractName()})) >= 0)");
                    sb.AppendLine("{");
                    sb.AppendLine($"    {addParameterLine}");
                    sb.AppendLine("}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return this.ExecuteNonQuery(command);");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        /// <summary>
        ///     Inserts the specified sb.
        /// </summary>
        static void Insert(PaddedStringBuilder sb, ITableInfo tableInfo, string typeContractName)
        {
            var insertInfo = new InsertInfoCreator().Create(tableInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} Inserts new record into table.");
            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                sb.AppendLine($"///{Padding.ForComment} <para>Automatically initialize '{sequenceInfo.TargetColumnName.ToContractName()}' property by using '{sequenceInfo.Name}' sequence.</para>");
            }

            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("if (contract == null)");
            sb.AppendLine("{");
            sb.AppendLine("    return this.ContractCannotBeNull();");
            sb.AppendLine("}");

            foreach (var sequenceInfo in tableInfo.SequenceList)
            {
                sb.AppendLine();

                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"// Init sequence for {sequenceInfo.TargetColumnName.ToContractName()}");
                sb.AppendLine();
                sb.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {sequenceInfo.Name}\";");
                sb.AppendLine();
                sb.AppendLine("var commandNextSequence = this.CreateCommand(sqlNextSequence);");
                sb.AppendLine();
                sb.AppendLine("var responseSequence = DBLayer.ExecuteScalar<object>(commandNextSequence);");
                sb.AppendLine("if (!responseSequence.Success)");
                sb.AppendLine("{");
                sb.AppendLine("    return this.SequenceFetchError(responseSequence);");
                sb.AppendLine("}");
                sb.AppendLine();

                var columnInfo = tableInfo.Columns.First(x => x.ColumnName == sequenceInfo.TargetColumnName);
                if (columnInfo.DotNetType == DotNetTypeName.DotNetInt32 || columnInfo.DotNetType == DotNetTypeName.DotNetInt32Nullable)
                {
                    sb.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt32(responseSequence.Value);");
                }
                else if (columnInfo.DotNetType == DotNetTypeName.DotNetStringName)
                {
                    sb.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToString(responseSequence.Value);");
                }
                else
                {
                    sb.AppendLine($"{contractParameterName}.{sequenceInfo.TargetColumnName.ToContractName()} = Convert.ToInt64(responseSequence.Value);");
                }

                sb.PaddingCount--;
                sb.AppendLine("}");
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
            sb.AppendLine("var command = this.CreateCommand(sql);");

            if (insertInfo.SqlParameters.Any())
            {
                var contractInitializations = new List<string>();

                if (insertInfo.SqlParameters.Any(c => c.ColumnName == Names2.ROW_GUID))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.ROW_GUID.ToContractName()} = Guid.NewGuid().ToString().ToUpper(new System.Globalization.CultureInfo(\"en-US\", false));");
                }

                if (insertInfo.SqlParameters.Any(c => c.ColumnName == Names2.INSERT_DATE))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.INSERT_DATE.ToContractName()} = DateTime.Now;");
                }

                if (insertInfo.SqlParameters.Any(c => c.ColumnName == Names2.INSERT_USER_ID))
                {
                    contractInitializations.Add($"{contractParameterName}.{Names2.INSERT_USER_ID.ToContractName()} = Context.ApplicationContext.Authentication.UserName;");
                }

                var tokenIdColumn = insertInfo.SqlParameters.FirstOrDefault(c => c.ColumnName == Names2.INSERT_TOKEN_ID);
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
                foreach (var columnInfo in insertInfo.SqlParameters)
                {
                    sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
                }
            }

            sb.AppendLine();
            if (tableInfo.HasIdentityColumn)
            {
                sb.AppendLine("var response = this.ExecuteScalar(command);");
                sb.AppendLine();
                sb.AppendLine($"{contractParameterName}.{tableInfo.IdentityColumn.ColumnName.ToContractName()} = response.Value;");
                sb.AppendLine();
                sb.AppendLine("return response;");
            }
            else
            {
                sb.AppendLine("return this.ExecuteNonQuery(command);");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}