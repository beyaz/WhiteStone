using System;
using System.Data;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.Models;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.ClassWriters
{
    public class BusinessClassWriter
    {
        
        public void Write_CustomSqlClass(PaddedStringBuilder sb, ProjectCustomSqlInfo project)
        {
            sb.AppendLine("public static class CustomSql");
            sb.AppendLine("{");
            sb.PaddingCount++;


            sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            sb.AppendLine("{");
            sb.PaddingCount++;


            sb.AppendLine("switch (input.Index)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in project.CustomSqlInfoList)
            {
                sb.AppendLine($"case {item.SwitchCaseIndex}:");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"return (TOutput) (object) new {item.BusinessClassName}(objectHelper.Context).Execute(({item.ParameterContractName})(object) input);");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("throw new System.InvalidOperationException(input.GetType().FullName);");

            sb.PaddingCount--;
            sb.AppendLine("}");


            sb.PaddingCount--;
            sb.AppendLine("}");

        }

        #region Public Methods
        public void Write(PaddedStringBuilder sb, CustomSqlInfo data,ProjectCustomSqlInfo projectCustomSqlInfo)
        {

            var key = $"{projectCustomSqlInfo.NamespaceNameOfBusiness}.{data.BusinessClassName}.Execute";

            WriteComment(sb, data);
            sb.AppendLine($"public sealed class {data.BusinessClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            WriteComment(sb, data);
            sb.AppendLine($"public {data.BusinessClassName}(ExecutionDataContext context) : base(context) {{ }}");

            sb.AppendLine();
            WriteComment(sb, data);
            if (data.SqlResultIsCollection)
            {


                sb.AppendLine($"public GenericResponse<List<{data.ResultContractName}>> Execute({data.ParameterContractName} request)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<List<{data.ResultContractName}>>(\"{key}\");");


                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(data.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();

                sb.AppendLine("var command = DBLayer.GetDBCommand(Databases.BOACard, sql, null, CommandType.Text);");

                if (data.Parameters.Any())
                {
                    sb.AppendLine();
                    
                    foreach (var item in data.Parameters)
                    {
                        AddInParameter(sb, item);
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
                sb.AppendLine($"var listOfDataContract = new List<{data.ResultContractName}>();");
                sb.AppendLine();
                sb.AppendLine("while (reader.Read())");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"var dataContract = new {data.ResultContractName}();");

                sb.AppendLine();

                sb.AppendLine("ReadContract(reader, dataContract);");

                sb.AppendLine();
                sb.AppendLine("listOfDataContract.Add(dataContract);");
                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("reader.Close();");

                sb.AppendLine();
                sb.AppendLine("returnObject.Value = listOfDataContract;");

                sb.AppendLine();
                sb.AppendLine("return returnObject;");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }
            else
            {
                sb.AppendLine($"public GenericResponse<{data.ResultContractName}> Execute({data.ParameterContractName} request)");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"var returnObject = InitializeGenericResponse<{data.ResultContractName}>(\"{key}\");");


                sb.AppendLine();
                sb.AppendLine("const string sql = @\"");
                sb.AppendAll(data.Sql);
                sb.AppendLine();
                sb.AppendLine("\";");
                sb.AppendLine();

                sb.AppendLine("var command = DBLayer.GetDBCommand(Databases.BOACard, sql, null, CommandType.Text);");

                if (data.Parameters.Any())
                {
                    sb.AppendLine();
                    foreach (var item in data.Parameters)
                    {
                        AddInParameter(sb, item);
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
                sb.AppendLine($"{data.ResultContractName} dataContract = null;");

                sb.AppendLine();
                sb.AppendLine("while (reader.Read())");
                sb.AppendLine("{");
                sb.PaddingCount++;
                

                sb.AppendLine($"dataContract = new {data.ResultContractName}();");
                sb.AppendLine();
                sb.AppendLine("ReadContract(reader, dataContract);");
                sb.AppendLine();
                sb.AppendLine("break;");
                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("reader.Close();");

                sb.AppendLine();
                sb.AppendLine("returnObject.Value = dataContract;");

                sb.AppendLine();
                sb.AppendLine("return returnObject;");

                sb.PaddingCount--;
                sb.AppendLine("}");
            }



            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Maps reader columns to contract for '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataRecord reader, {data.ResultContractName} contract)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            foreach (var item in data.ResultColumns)
            {
                sb.AppendLine($"contract.{item.NameInDotnet} = SQLDBHelper.{item.SqlReaderMethod}(reader[\"{item.Name}\"]);");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");



            sb.PaddingCount--;
            sb.AppendLine("}");
            





            
        }

        static void AddInParameter(PaddedStringBuilder sb, CustomSqlInfoParameter item)
        {
            var value = $"request.{item.CSharpPropertyName}";

            if (item.Name.EndsWith("_FLAG", StringComparison.OrdinalIgnoreCase) &&
                item.SqlDbTypeName == SqlDbType.Char &&
                (item.CSharpPropertyTypeName == DotNetTypeName.DotNetBool || item.CSharpPropertyTypeName == DotNetTypeName.DotNetBool + "?"))
            {
                value = $"request.{item.CSharpPropertyName} ? \"1\" : \"0\"";
            }

            sb.AppendLine($"DBLayer.AddInParameter(command, \"@{item.Name}\", SqlDbType.{item.SqlDbTypeName}, {value});");
        }

        static void WriteComment(PaddedStringBuilder sb, CustomSqlInfo data)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
        }
        #endregion
    }
}