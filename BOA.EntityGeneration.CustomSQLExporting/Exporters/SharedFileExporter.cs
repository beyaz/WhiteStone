﻿using System.IO;
using System.Linq;
using BOA.Common.Helpers;
using BOA.EntityGeneration.DbModel;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    class SharedFileExporter : ContextContainer
    {
        

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
            sb.AppendLine($"public static class {customSqlNamingPattern.RepositoryClassName}");
            sb.OpenBracket();
        }

        void BeginNamespace()
        {
            sb.AppendLine($"namespace {profileNamingPattern.RepositoryNamespace}.Shared");
            sb.OpenBracket();
        }

        void CreateSqlInfo()
        {
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
            processInfo.Text = "Exporting Shared repository.";

            var filePath = profileNamingPattern.RepositoryProjectDirectory + "Shared.cs";

            FileSystem.WriteAllText(filePath, sb.ToString());
        }

       

        void WriteEmbeddedClasses()
        {
            var path = Path.GetDirectoryName(typeof(SharedFileExporter).Assembly.Location) + Path.DirectorySeparatorChar + "SharedRepositoryFileEmbeddedCodes.txt";

            sb.AppendAll(System.IO.File.ReadAllText(path));
            sb.AppendLine();
        }

        void WriteReadContract()
        {
            if (customSqlInfo.ResultContractIsReferencedToEntity)
            {
                return;
            }

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Maps reader columns to contract for '{customSqlInfo.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public void ReadContract(IDataReader reader, {customSqlNamingPattern.ResultClassName} contract)");
            sb.OpenBracket();

            foreach (var item in customSqlInfo.ResultColumns)
            {
                if (item.IsReferenceToEntity)
                {
                    repositoryAssemblyReferences.Add(customSqlNamingPattern.ReferencedRepositoryAssemblyPath);
                    repositoryAssemblyReferences.Add(customSqlNamingPattern.ReferencedEntityAssemblyPath);

                    sb.AppendLine($"contract.{item.NameInDotnet} = new {customSqlNamingPattern.ReferencedEntityAccessPath}();");
                    sb.AppendLine($"{customSqlNamingPattern.ReferencedEntityReaderMethodPath}(reader, contract.{item.NameInDotnet});");
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
            sb.AppendLine($"using {profileNamingPattern.EntityNamespace};");
        }
        #endregion
    }
}