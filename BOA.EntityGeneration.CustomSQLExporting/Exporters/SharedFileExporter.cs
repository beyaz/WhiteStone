using System.Collections.Generic;
using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;
using static BOA.EntityGeneration.CustomSQLExporting.CustomSqlNamingPatternContract;
using static BOA.EntityGeneration.CustomSQLExporting.Data;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public static class SharedFileExporter
    {
        #region Static Fields
        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(EntityFileExporter) + "->" + nameof(File));
        
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeOutput);
            context.AttachEvent(OnProfileInfoInitialized, WriteUsingList);
            context.AttachEvent(OnProfileInfoInitialized, EmptyLine);
            context.AttachEvent(OnProfileInfoInitialized, BeginNamespace);
            context.AttachEvent(OnProfileInfoInitialized, WriteEmbeddedClasses);

            context.AttachEvent(OnCustomSqlInfoInitialized, EmptyLine);
            context.AttachEvent(OnCustomSqlInfoInitialized, BeginClass);
            context.AttachEvent(OnCustomSqlInfoInitialized, CreateSqlInfo);
            context.AttachEvent(OnCustomSqlInfoInitialized, EmptyLine);
            context.AttachEvent(OnCustomSqlInfoInitialized, WriteReadContract);
            context.AttachEvent(OnCustomSqlInfoInitialized, EndClass);

            context.AttachEvent(OnProfileInfoRemove, EndNamespace);
            context.AttachEvent(OnProfileInfoRemove, ExportFileToDirectory);
            
        }
        #endregion

        #region Methods
        static void BeginClass(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlInfo);
            var customSqlNamingPattern = context.Get(CustomSqlNamingPattern);

            sb.AppendLine($"public static class {customSqlNamingPattern.RepositoryClassName}");
            sb.OpenBracket();
        }

        static void BeginNamespace(IDataContext context)
        {
            var sb            = context.Get(File);
            var profileNamingPattern = context.Get(ProfileNamingPattern);


            sb.AppendLine($"namespace {profileNamingPattern.RepositoryNamespace}.Shared");
            sb.OpenBracket();
        }

        static void WriteEmbeddedClasses(IDataContext context)
        {
            var sb            = context.Get(File);

            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

       

        static void CreateSqlInfo(IDataContext context)
        {
            var sb            = context.Get(File);
            var customSqlInfo = context.Get(CustomSqlInfo);
            var customSqlNamingPattern = context.Get(CustomSqlNamingPattern);
            

            sb.AppendLine($"public static SqlInfo CreateSqlInfo({customSqlNamingPattern.InputClassName} request)");
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
            var sb            = context.Get(File);
            var processInfo   = context.Get(ProcessInfo);
            var profileNamingPattern = context.Get(ProfileNamingPattern);

            processInfo.Text = "Exporting Shared repository.";

            var filePath = profileNamingPattern.RepositoryProjectDirectory + "Shared.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            File[context] = new PaddedStringBuilder();
        }

        static void WriteReadContract(IDataContext context)
        {
            var sb   = context.Get(File);
            var customSqlInfo = context.Get(CustomSqlInfo);
            var customSqlNamingPattern = context.Get(CustomSqlNamingPattern);
            var repositoryAssemblyReferenceList = RepositoryAssemblyReferences[context];
            

            if (customSqlInfo.ResultContractIsReferencedToEntity)
            {
                return;
            }

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Maps reader columns to contract for '{customSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public static void ReadContract(IDataRecord reader, {customSqlNamingPattern.ResultClassName} contract)");
            sb.OpenBracket();

            foreach (var item in customSqlInfo.ResultColumns)
            {

                if (item.IsReferenceToEntity)
                {
                    repositoryAssemblyReferenceList.Add(customSqlNamingPattern.ReferencedRepositoryAssemblyPath);
                    sb.AppendLine($"contract.{item.NameInDotnet} = new {customSqlNamingPattern.ReferencedEntityAccessPath}();");
                    sb.AppendLine($"{customSqlNamingPattern.ReferencedEntityReaderMethodPathPath}(reader, contract.{item.NameInDotnet});");
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

        static void WriteUsingList(IDataContext context)
        {
            var sb            = context.Get(File);
            var profileNamingPattern = context.Get(ProfileNamingPattern);

            sb.AppendLine("using System;");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Data.SqlClient;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine($"using {profileNamingPattern.EntityNamespace};");
        }
        #endregion
    }
}