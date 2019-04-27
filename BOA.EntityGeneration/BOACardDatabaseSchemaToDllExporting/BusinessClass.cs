using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class BusinessClass
    {
        #region Public Methods
        public string TransformText(GeneratorData Data)
        {
            var tableInfo             = Data.TableInfo;
            var typeContractName      = $"{tableInfo.TableName.ToContractName()}Contract";
            var className             = tableInfo.TableName.ToContractName();
            var namespaceName         = $"BOA.Business.Kernel.Card.{tableInfo.SchemaName}";
            var contractParameterName = "contract";

            var sb = new PaddedStringBuilder();

            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Types.Card.CCO;");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine();

            #region namespace
            sb.AppendLine($"namespace {namespaceName}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            #region class
            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public sealed class {className} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine($"public {className}(ExecutionDataContext context) : base(context) {{ }}");

            #region Delete
            if (Data.IsSupportSelectByKey)
            {
                var deleteInfo = DeleteInfoCreator.Create(tableInfo);

                var parameterPart = string.Join(", ", deleteInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                sb.AppendLine();
                sb.AppendLine($"public GenericResponse<int> Delete({parameterPart})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<int>(\"{namespaceName}.{className}.Delete\");");

                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(deleteInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (deleteInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in deleteInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDatabaseTypeName}, {columnInfo.ColumnName.AsMethodParameter()});");
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
                sb.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<int>(\"{namespaceName}.{className}.Insert\");");

                sb.AppendLine();
                sb.AppendLine("if (contract == null)");
                sb.AppendLine("{");
                sb.AppendLine(@"    returnObject.Results.Add(new Result { ErrorMessage = BOA.Messaging.MessagingHelper.GetMessage(""BOA"", ""CanNotBeNull"")});");
                sb.AppendLine("    return returnObject;");
                sb.AppendLine("}");

                if (Data.SequenceName.HasValue())
                {
                    sb.AppendLine();
                    sb.AppendLine("#region Init Sequence");

                    sb.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {Data.SequenceName}\";");
                    sb.AppendLine();
                    sb.AppendLine($"var commandNextSequence = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sqlNextSequence, null, CommandType.Text);");
                    sb.AppendLine();
                    sb.AppendLine("var responseSequence = DBLayer.ExecuteScalar<long>(commandNextSequence);");
                    sb.AppendLine("if (!responseSequence.Success)");
                    sb.AppendLine("{");
                    sb.AppendLine("    returnObject.Results.AddRange(responseSequence.Results);");
                    sb.AppendLine("    return returnObject;");
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine($"{contractParameterName}.RecordId = responseSequence.Value;");

                    sb.AppendLine("#endregion");
                }

                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(insertInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (insertInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in insertInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDatabaseTypeName}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
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

            #region Update
            if (Data.IsSupportSelectByKey)
            {
                var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

                sb.AppendLine();
                sb.AppendLine($"public GenericResponse<int> Update({typeContractName} {contractParameterName})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<int>(\"{namespaceName}.{className}.Update\");");

                sb.AppendLine();
                sb.AppendLine("if (contract == null)");
                sb.AppendLine("{");
                sb.AppendLine(@"    returnObject.Results.Add(new Result { ErrorMessage = BOA.Messaging.MessagingHelper.GetMessage(""BOA"", ""CanNotBeNull"")});");
                sb.AppendLine("    return returnObject;");
                sb.AppendLine("}");

                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(updateInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (updateInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in updateInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDatabaseTypeName}, {ParameterHelper.GetValueForSqlUpdate(columnInfo)});");
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

            #region SelectByKey
            if (Data.IsSupportSelectByKey)
            {
                var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

                var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                sb.AppendLine();
                sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<{typeContractName}>(\"{namespaceName}.{className}.SelectByKey\");");

                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(selectByPrimaryKeyInfo.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (selectByPrimaryKeyInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in selectByPrimaryKeyInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDatabaseTypeName}, {columnInfo.ColumnName.AsMethodParameter()});");
                    }
                }

                sb.AppendLine();
                sb.AppendLine("var response = DBLayer.ExecuteReader(command);");
                sb.AppendLine("if (!response.Success)");
                sb.AppendLine("{");
                sb.AppendLine("    returnObject.Results.AddRange(response.Results);");
                sb.AppendLine("    return returnObject;");
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("var reader = response.Value;");

                sb.AppendLine();
                sb.AppendLine("#region Fill from SqlDataReader to DataContract");
                sb.AppendLine();
                sb.AppendLine($"{typeContractName} dataContract = null;");
                sb.AppendLine();
                sb.AppendLine("while (reader.Read())");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"dataContract = new {typeContractName}();");

                sb.AppendLine();

                sb.AppendLine("ReadContract(dataContract,reader);");

                sb.AppendLine();
                sb.AppendLine("break;");
                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("reader.Close();");
                sb.AppendLine();
                sb.AppendLine("#endregion");

                sb.AppendLine();
                sb.AppendLine("returnObject.Value = dataContract;");

                sb.AppendLine();
                sb.AppendLine("return returnObject;");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            #endregion

            #region SelectByUniqueIndex
            if (Data.IsSupportSelectByUniqueIndex)
            {
                foreach (var indexIdentifier in Data.UniqueIndexInfoList)
                {
                    var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                    var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                    var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                    sb.AppendLine();
                    sb.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine($"var returnObject = InitializeGenericResponse<{typeContractName}>(\"{namespaceName}.{className}.{methodName}\");");

                    sb.AppendLine();
                    sb.AppendLine("const string sql = @\"");
                    sb.AppendAll(indexInfo.Sql);
                    sb.AppendLine();
                    sb.AppendLine("\";");
                    sb.AppendLine();
                    sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sql, null, CommandType.Text);");

                    if (indexInfo.SqlParameters.Any())
                    {
                        sb.AppendLine();
                        foreach (var columnInfo in indexInfo.SqlParameters)
                        {
                            sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDatabaseTypeName}, {columnInfo.ColumnName.AsMethodParameter()});");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("var response = DBLayer.ExecuteReader(command);");
                    sb.AppendLine("if (!response.Success)");
                    sb.AppendLine("{");
                    sb.AppendLine("    returnObject.Results.AddRange(response.Results);");
                    sb.AppendLine("    return returnObject;");
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine("var reader = response.Value;");

                    sb.AppendLine();
                    sb.AppendLine("#region Fill from SqlDataReader to DataContract");
                    sb.AppendLine();
                    sb.AppendLine($"{typeContractName} dataContract = null;");
                    sb.AppendLine();
                    sb.AppendLine("while (reader.Read())");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"dataContract = new {typeContractName}();");

                    sb.AppendLine();

                    sb.AppendLine("ReadContract(dataContract,reader);");

                    sb.AppendLine();
                    sb.AppendLine("break;");
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine("reader.Close();");
                    sb.AppendLine();
                    sb.AppendLine("#endregion");

                    sb.AppendLine();
                    sb.AppendLine("returnObject.Value = dataContract;");

                    sb.AppendLine();
                    sb.AppendLine("return returnObject;");

                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }
            }
            #endregion


            #region SelectByNonUniqueIndex
            if (Data.IsSupportSelectByIndex)
            {
                foreach (var indexIdentifier in Data.NonUniqueIndexInfoList)
                {
                    var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                    var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                    var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                    sb.AppendLine();
                    sb.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine($"var returnObject = InitializeGenericResponse<List<{typeContractName}>>(\"{namespaceName}.{className}.{methodName}\");");

                    sb.AppendLine();
                    sb.AppendLine("const string sql = @\"");
                    sb.AppendAll(indexInfo.Sql);
                    sb.AppendLine();
                    sb.AppendLine("\";");
                    sb.AppendLine();
                    sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{Data.DatabaseEnumName}, sql, null, CommandType.Text);");

                    if (indexInfo.SqlParameters.Any())
                    {
                        sb.AppendLine();
                        foreach (var columnInfo in indexInfo.SqlParameters)
                        {
                            sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDatabaseTypeName}, {columnInfo.ColumnName.AsMethodParameter()});");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("var response = DBLayer.ExecuteReader(command);");
                    sb.AppendLine("if (!response.Success)");
                    sb.AppendLine("{");
                    sb.AppendLine("    returnObject.Results.AddRange(response.Results);");
                    sb.AppendLine("    return returnObject;");
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine("var reader = response.Value;");

                    sb.AppendLine();
                    sb.AppendLine("#region Fill from SqlDataReader to DataContract List");
                    sb.AppendLine();
                    sb.AppendLine("var listOfDataContract = new {0}();");
                    sb.AppendLine();
                    sb.AppendLine("while (reader.Read())");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"var dataContract = new {typeContractName}();");

                    sb.AppendLine();

                    sb.AppendLine("ReadContract(dataContract,reader);");

                    sb.AppendLine();
                    sb.AppendLine("listOfDataContract.Add(dataContract);");
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                    sb.AppendLine();
                    sb.AppendLine("reader.Close();");
                    sb.AppendLine();
                    sb.AppendLine("#endregion");

                    sb.AppendLine();
                    sb.AppendLine("returnObject.Value = listOfDataContract;");

                    sb.AppendLine();
                    sb.AppendLine("return returnObject;");

                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }
            }
            #endregion

            #region ReadContract
            sb.AppendLine();
            sb.AppendLine($"static void ReadContract(SqlDataReader reader, {typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var c in tableInfo.Columns)
            {
                if (c.ColumnName == Names2.VALID_FLAG)
                {
                    sb.AppendLine($"{contractParameterName}.{c.ColumnName.ToContractName()} = SQLDBHelper.{c.SqlReaderMethod}(reader, \"{c.ColumnName}\") == \"1\";");
                }
                else
                {
                    sb.AppendLine($"{contractParameterName}.{c.ColumnName.ToContractName()} = SQLDBHelper.{c.SqlReaderMethod}(reader, \"{c.ColumnName}\");");
                }
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            return sb.ToString();
        }
        #endregion
    }
}