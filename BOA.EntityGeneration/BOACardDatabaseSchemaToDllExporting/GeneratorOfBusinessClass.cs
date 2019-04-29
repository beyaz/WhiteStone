﻿using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.ScriptModel;
using BOA.EntityGeneration.ScriptModel.Creators;
using Ninject;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting
{
    public class GeneratorOfBusinessClass
    {
        [Inject]
        public InsertInfoCreator InsertInfoCreator { get; set; }

        [Inject]
        public NamingHelper NamingHelper { get; set; }

        #region Public Methods

        public void WriteUsingList(PaddedStringBuilder sb,  TableInfo tableInfo)
        {
            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine($"using {NamingHelper.GetTypeClassNamespace(tableInfo.SchemaName)};");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data;");

        }

        public void WriteClass(PaddedStringBuilder sb,TableInfo tableInfo)
        {
            var typeContractName      = $"{tableInfo.TableName.ToContractName()}Contract";
            var className             = tableInfo.TableName.ToContractName();
            const string contractParameterName = "contract";

            if (typeContractName =="TransactionLogContract") // resolve conflig
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
                sb.AppendLine($"///{Padding.ForComment}Deletes only one record from '{tableInfo.SchemaName}.{tableInfo.TableName}' by using '{string.Join(" and ", deleteInfo.SqlParameters.Select(x=>x.ColumnName.AsMethodParameter()))}'");
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
                sb.AppendLine($"///{Padding.ForComment} TODO:");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<int> Insert({typeContractName} {contractParameterName})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<int>(\"{businessClassNamespace}.{className}.Insert\");");

                sb.AppendLine();
                sb.AppendLine("if (contract == null)");
                sb.AppendLine("{");
                sb.AppendLine(@"    returnObject.Results.Add(new Result { ErrorMessage = Messaging.MessagingHelper.GetMessage(""BOA"", ""CanNotBeNull"")});");
                sb.AppendLine("    return returnObject;");
                sb.AppendLine("}");

                if (tableInfo.SequenceName.HasValue())
                {
                    sb.AppendLine();
                    sb.AppendLine("#region Init Sequence");

                    sb.AppendLine($"const string sqlNextSequence = @\"SELECT NEXT VALUE FOR {tableInfo.SequenceName}\";");
                    sb.AppendLine();
                    sb.AppendLine($"var commandNextSequence = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sqlNextSequence, null, CommandType.Text);");
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
                sb.AppendLine($"var command = DBLayer.GetDBCommand(Databases.{tableInfo.DatabaseEnumName}, sql, null, CommandType.Text);");

                if (insertInfo.SqlParameters.Any())
                {
                    sb.AppendLine();
                    foreach (var columnInfo in insertInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlInsert(columnInfo)});");
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
            if (tableInfo.IsSupportSelectByKey)
            {
                var updateInfo = UpdateByPrimaryKeyInfoCreator.Create(tableInfo);

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} TODO:");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<int> Update({typeContractName} {contractParameterName})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<int>(\"{businessClassNamespace}.{className}.Update\");");

                sb.AppendLine();
                sb.AppendLine("if (contract == null)");
                sb.AppendLine("{");
                sb.AppendLine(@"    returnObject.Results.Add(new Result { ErrorMessage = Messaging.MessagingHelper.GetMessage(""BOA"", ""CanNotBeNull"")});");
                sb.AppendLine("    return returnObject;");
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
                    sb.AppendLine();
                    foreach (var columnInfo in updateInfo.SqlParameters)
                    {
                        sb.AppendLine($"DBLayer.AddInParameter(command, \"@{columnInfo.ColumnName}\", SqlDbType.{columnInfo.SqlDbType}, {ParameterHelper.GetValueForSqlUpdate(columnInfo)});");
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
            if (tableInfo.IsSupportSelectByKey)
            {
                var selectByPrimaryKeyInfo = SelectByPrimaryKeyInfoCreator.Create(tableInfo);

                var parameterPart = string.Join(", ", selectByPrimaryKeyInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                sb.AppendLine();
                sb.AppendLine("/// <summary>");
                sb.AppendLine($"///{Padding.ForComment} TODO:");
                sb.AppendLine("/// </summary>");
                sb.AppendLine($"public GenericResponse<{typeContractName}> SelectByKey({parameterPart})");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<{typeContractName}>(\"{businessClassNamespace}.{className}.SelectByKey\");");

                sb.AppendLine();
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

                sb.AppendLine("ReadContract(reader, dataContract);");

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
            if (tableInfo.IsSupportSelectByUniqueIndex)
            {
                foreach (var indexIdentifier in tableInfo.UniqueIndexInfoList)
                {
                    var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                    var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                    var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine($"///{Padding.ForComment} TODO:");
                    sb.AppendLine("/// </summary>");
                    sb.AppendLine($"public GenericResponse<{typeContractName}> {methodName}({parameterPart})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine($"var returnObject = InitializeGenericResponse<{typeContractName}>(\"{businessClassNamespace}.{className}.{methodName}\");");

                    sb.AppendLine();
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

                    sb.AppendLine("ReadContract(reader, dataContract);");

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
            if (tableInfo.IsSupportSelectByIndex)
            {
                foreach (var indexIdentifier in tableInfo.NonUniqueIndexInfoList)
                {
                    var indexInfo = SelectByIndexInfoCreator.Create(tableInfo, indexIdentifier);

                    var parameterPart = string.Join(", ", indexInfo.SqlParameters.Select(x => $"{x.DotNetType} {x.ColumnName.AsMethodParameter()}"));

                    var methodName = "SelectBy" + string.Join(string.Empty, indexInfo.SqlParameters.Select(x => $"{x.ColumnName.ToContractName()}"));

                    sb.AppendLine();
                    sb.AppendLine("/// <summary>");
                    sb.AppendLine($"///{Padding.ForComment} TODO:");
                    sb.AppendLine("/// </summary>");
                    sb.AppendLine($"public GenericResponse<List<{typeContractName}>> {methodName}({parameterPart})");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    sb.AppendLine($"var returnObject = InitializeGenericResponse<List<{typeContractName}>>(\"{businessClassNamespace}.{className}.{methodName}\");");

                    sb.AppendLine();
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
                    sb.AppendLine($"var listOfDataContract = new List<{typeContractName}>();");
                    sb.AppendLine();
                    sb.AppendLine("while (reader.Read())");
                    sb.AppendLine("{");
                    sb.PaddingCount++;
                    sb.AppendLine($"var dataContract = new {typeContractName}();");

                    sb.AppendLine();

                    sb.AppendLine("ReadContract(reader, dataContract);");

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
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment} TODO:");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataReader reader, {typeContractName} {contractParameterName})");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var c in tableInfo.Columns)
            {
                sb.AppendLine($"{contractParameterName}.{c.ColumnName.ToContractName()} = SQLDBHelper.{c.SqlReaderMethod}(reader[\"{c.ColumnName}\"]);");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");
            #endregion

            sb.PaddingCount--;
            sb.AppendLine("}");
          

         
        }
        #endregion
    }
}