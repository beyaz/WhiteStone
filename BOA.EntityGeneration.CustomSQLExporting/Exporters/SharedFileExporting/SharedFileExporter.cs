using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters.SharedFileExporting
{
    class SharedFileExporter : ContextContainer
    {

        internal static readonly SharedFileExporterConfig _config = SharedFileExporterConfig.CreateFromFile();

        #region Properties
        readonly PaddedStringBuilder sb =new PaddedStringBuilder();
        #endregion

        #region Public Methods
        public void AttachEvents()
        {
            Context.ProfileInfoInitialized += WriteUsingList;
            Context.ProfileInfoInitialized += EmptyLine;
            Context.ProfileInfoInitialized += BeginNamespace;
            Context.ProfileInfoInitialized += WriteEmbeddedClasses;

            Context.CustomSqlInfoInitialized += EmptyLine;
            Context.CustomSqlInfoInitialized += BeginClass;
            Context.CustomSqlInfoInitialized += CreateSqlInfo;
            Context.CustomSqlInfoInitialized += EmptyLine;
            Context.CustomSqlInfoInitialized += WriteReadContract;
            Context.CustomSqlInfoInitialized += EndClass;

            Context.ProfileInfoRemove += EndNamespace;
            Context.ProfileInfoRemove += ExportFileToDirectory;
        }
        #endregion

        #region Methods
        void BeginClass()
        {
            sb.AppendLine($"public static class {CustomSqlNamingPattern.RepositoryClassName}");
            sb.OpenBracket();
        }

        void BeginNamespace()
        {
            sb.AppendLine($"namespace {ProfileNamingPattern.RepositoryNamespace}.Shared");
            sb.OpenBracket();
        }

        void CreateSqlInfo()
        {
            sb.AppendLine($"public static SqlInfo CreateSqlInfo({CustomSqlNamingPattern.InputClassName} request)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("const string sql = @\"");
            sb.AppendAll(CustomSqlInfo.Sql);
            sb.AppendLine();
            sb.AppendLine("\";");
            sb.AppendLine();
            sb.AppendLine("var sqlInfo = new SqlInfo { CommandText = sql };");

            if (CustomSqlInfo.Parameters.Any())
            {
                sb.AppendLine();
                foreach (var item in CustomSqlInfo.Parameters)
                {
                    sb.AppendLine($"sqlInfo.AddInParameter(\"@{item.Name}\", SqlDbType.{item.SqlDbTypeName}, request.{item.ValueAccessPathForAddInParameter});");
                }
            }

            sb.AppendLine();
            sb.AppendLine("return sqlInfo;");

            sb.PaddingCount--;
            sb.AppendLine("}");
        }

        void EmptyLine()
        {
            sb.AppendLine();
        }

        void EndClass()
        {
            sb.CloseBracket();
        }

        void EndNamespace()
        {
            sb.CloseBracket();
        }

        void ExportFileToDirectory()
        {
            ProcessInfo.Text = "Exporting Shared repository.";

            var filePath = ProfileNamingPattern.RepositoryProjectDirectory + "Shared.cs";

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

       

        void WriteEmbeddedClasses()
        {
            sb.AppendAll(_config.EmbeddedCodes);
            sb.AppendLine();
        }

        void WriteReadContract()
        {
            if (CustomSqlInfo.ResultContractIsReferencedToEntity)
            {
                return;
            }

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Maps reader columns to contract for '{CustomSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public static void ReadContract(IDataReader reader, {CustomSqlNamingPattern.ResultClassName} contract)");
            sb.OpenBracket();

            foreach (var item in CustomSqlInfo.ResultColumns)
            {
                if (item.IsReferenceToEntity)
                {
                    Context.ExtraAssemblyReferencesForRepositoryProject.Add(CustomSqlNamingPattern.ReferencedRepositoryAssemblyPath);
                    Context.ExtraAssemblyReferencesForRepositoryProject.Add(CustomSqlNamingPattern.ReferencedEntityAssemblyPath);

                    sb.AppendLine($"contract.{item.NameInDotnet} = new {CustomSqlNamingPattern.ReferencedEntityAccessPath}();");
                    sb.AppendLine($"{CustomSqlNamingPattern.ReferencedEntityReaderMethodPath}(reader, contract.{item.NameInDotnet});");
                    continue;
                }

                var readerMethodName = item.SqlReaderMethod.ToString();
                if (item.SqlReaderMethod == SqlReaderMethods.GetGUIDValue)
                {
                    readerMethodName = "GetGuidValue";
                }

                sb.AppendLine($"contract.{item.NameInDotnet} = reader.{readerMethodName}(\"{item.Name}\");");
            }

            sb.CloseBracket();
        }

        void WriteUsingList()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine($"using {ProfileNamingPattern.EntityNamespace};");
        }
        #endregion
    }
}