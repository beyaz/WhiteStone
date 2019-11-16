using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.CustomSQLExporting.Wrapper;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public static class SharedFileExporter
    {
        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(TypeFileExporter) + "->" + nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeOutput);
            context.AttachEvent(OnProfileInfoInitialized, WriteUsingList);
            context.AttachEvent(OnProfileInfoInitialized, BeginNamespace);

            context.AttachEvent(OnCustomSqlInfoInitialized, EmptyLine);
            context.AttachEvent(OnCustomSqlInfoInitialized, BeginClass);
            context.AttachEvent(OnCustomSqlInfoInitialized, CreateSqlInfo);
            context.AttachEvent(OnCustomSqlInfoInitialized, EmptyLine);
            context.AttachEvent(OnCustomSqlInfoInitialized, WriteReadContract);
            context.AttachEvent(OnCustomSqlInfoInitialized, EndClass);

            context.AttachEvent(OnProfileInfoRemove, EndNamespace);
            context.AttachEvent(OnProfileInfoRemove, ExportFileToDirectory);
            context.AttachEvent(OnProfileInfoRemove, ClearOutput);
        }
        #endregion

        #region Methods
        static void BeginClass(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlInfo);

            sb.AppendLine($"public static class {data.BusinessClassName}Shared");
            sb.OpenBracket();
        }

        static void BeginNamespace(IDataContext context)
        {
            var sb   = context.Get(File);
            var namingPattern = context.Get(NamingPattern.Id);

            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine($"using {namingPattern.EntityNamespace};");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {namingPattern.RepositoryNamespace}.Shared");
            sb.OpenBracket();
        }

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void CreateSqlInfo(IDataContext context)
        {
            var sb   = context.Get(File);
            var customSqlInfo = context.Get(CustomSqlInfo);

            sb.AppendLine($"public static SqlInfo CreateSqlInfo({customSqlInfo.ParameterContractName} request)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(customSqlInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();
            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");


            if (customSqlInfo.Parameters.Any())
            {
                sb.AppendLine();
                foreach (var item in customSqlInfo.Parameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{item.Name}\", SqlDbType.{item.SqlDbTypeName}, request.{item.ValueAccessPathForAddInParameter});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        static void EmptyLine(IDataContext context)
        {
            context.Get(File).AppendLine();
        }

        static void EndClass(IDataContext context)
        {
            context.Get(File).CloseBracket();
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.CloseBracket();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb          = context.Get(File);
            var processInfo = context.Get(CustomSqlExporter.CustomSqlGenerationOfProfileIdProcess);
            var namingPattern = context.Get(NamingPattern.Id);


            processInfo.Text = "Exporting Shared repository.";

            var filePath = namingPattern.RepositoryProjectDirectory + "Shared.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteReadContract(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlInfo);

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Maps reader columns to contract for '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public static void ReadContract(IDataRecord reader, {data.ResultContractName} contract)");
            sb.OpenBracket();

            foreach (var item in data.ResultColumns)
            {
                sb.AppendLine($"contract.{item.NameInDotnet} = SQLDBHelper.{item.SqlReaderMethod}(reader[\"{item.Name}\"]);");
            }

            sb.CloseBracket();
        }

        static void WriteUsingList(IDataContext context)
        {
            var sb = context.Get(File);

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
        }
        #endregion
    }
}