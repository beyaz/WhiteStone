using System;
using System.Linq;
using BOA.Common.Helpers;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection
{
    public class BusinessClassWriter
    {
        #region Public Methods
        public void Write(PaddedStringBuilder sb, CustomSqlInfo data)
        {
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {data.BusinessClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {data.BusinessClassName}(ExecutionDataContext context) : base(context) {{ }}");

            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Executes the '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public GenericResponse<List<{data.ResultContractName}>> Execute({data.ParameterContractName} request)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"var returnObject = InitializeGenericResponse<List<{data.ResultContractName}>>(\"{data.BusinessClassNamespace}.{data.BusinessClassName}.Execute\");");


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
                    sb.AppendLine($"DBLayer.AddInParameter(command, \"@{item.Name}\", SqlDbType.{item.SqlDatabaseTypeName}, request.{item.NameInDotnet});");
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




            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///     Maps reader columns to contract for '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataReader reader, {data.ResultContractName} contract)");
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
        #endregion
    }
}