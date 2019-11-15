using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.CustomSQLExporting.Models.Interfaces;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public static class SharedFileExporter
    {
        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(TypeFileExporter) +"->"+ nameof(File));

        public static void AttachEvents(IDataContext context)
        {

            context.AttachEvent(ProfileInfoIsAvailable, InitializeOutput);
            context.AttachEvent(ProfileInfoIsAvailable, WriteUsingList);
            context.AttachEvent(ProfileInfoIsAvailable, BeginClass);
            context.AttachEvent(ProfileInfoIsAvailable, EndClass);
            
            context.AttachEvent(ProfileInfoIsAvailable, ExportFileToDirectory);
            context.AttachEvent(ProfileInfoIsAvailable, ClearOutput);

            context.AttachEvent(CustomSqlInfoIsAvailable, CreateSqlInfo);
            context.AttachEvent(CustomSqlInfoIsAvailable, EmptyLine);
            context.AttachEvent(CustomSqlInfoIsAvailable, WriteReadContract);
            
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }
        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb         = context.Get(File);
            var data       = context.Get(CustomSqlExporter.CustomSqlProfileInfo);
            var fileAccess = context.Get(Data.FileAccess);

            
            fileAccess.WriteAllText(data.BusinessProjectPath + "\\Generated\\Shared.cs", sb.ToString());
        }

        static void WriteReadContract(IDataContext context)
        {
            PaddedStringBuilder sb = context.Get(File);
            ICustomSqlInfo data = context.Get(CustomSqlExporter.CustomSqlInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Maps reader columns to contract for '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"static void ReadContract(IDataRecord reader, {data.ResultContractName} contract)");
            sb.OpenBracket();

            foreach (var item in data.ResultColumns)
            {
                sb.AppendLine($"contract.{item.NameInDotnet} = SQLDBHelper.{item.SqlReaderMethod}(reader[\"{item.Name}\"]);");
            }

            sb.CloseBracket();
        }

        static void CreateSqlInfo(IDataContext context)
        {
            PaddedStringBuilder sb   = context.Get(File);
            ICustomSqlInfo      data = context.Get(CustomSqlExporter.CustomSqlInfo);
        
            sb.AppendLine($"static SqlInfo CreateSqlInfo({data.ParameterContractName} request)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("var sqlInfo = new SqlInfo();");
            if (data.Parameters.Any())
            {
                sb.AppendLine();
                foreach (var item in data.Parameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{item.Name}\", SqlDbType.{item.SqlDbTypeName}, request.{item.ValueAccessPathForAddInParameter});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }


        static void WriteUsingList(IDataContext context)
        {
            var sb   = context.Get(File);

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");

        }

        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        static void BeginClass(IDataContext context)
        {
            var sb      = context.Get(File);
            var data = context.Get(CustomSqlExporter.CustomSqlInfo);


           
            sb.AppendLine($"public static class {data.BusinessClassName}Shared");
            sb.OpenBracket();

            

        }

        static void EndClass(IDataContext context)
        {
            context.Get(File).CloseBracket();
        }
    }
}