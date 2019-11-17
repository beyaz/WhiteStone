using System;
using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.DataAccess;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ProjectExporters;
using BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.Util;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel.Creators;

namespace BOA.EntityGeneration.BOACardDatabaseSchemaToDllExporting.ClassWriters
{
    static class EntityFileExporter
    {

        
        static void ExportFileToDirectory(IDataContext context)
        {
            var sb            = context.Get(File);
            var namingPattern = context.Get(NamingPattern.Id);
            var processInfo = context.Get(Data.SchemaGenerationProcess);

            processInfo.Text = "Exporting Entity classes.";

            var filePath = namingPattern.EntityProjectDirectory + "All.cs";

            FileSystem.WriteAllText(context, filePath, sb.ToString());
        }


        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(File));

        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(DataEvent.StartToExportSchema,InitializeOutput);
            context.AttachEvent(DataEvent.StartToExportSchema, WriteUsingList);
            context.AttachEvent(DataEvent.StartToExportSchema, BeginNamespace);
            context.AttachEvent(DataEvent.StartToExportSchema, EndNamespace);

            context.AttachEvent(DataEvent.StartToExportTable, WriteClass);

            context.AttachEvent(DataEvent.FinishingExportingSchema,ExportFileToDirectory);
            context.AttachEvent(DataEvent.FinishingExportingSchema,ClearOutput);
        }
        #region Public Methods
         static void BeginNamespace(IDataContext context)
        {
            var sb         = context.Get<PaddedStringBuilder>(File);

            var namingPattern = context.Get(NamingPattern.Id);
            

            sb.AppendLine();
            sb.BeginNamespace(namingPattern.EntityNamespace);
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get<PaddedStringBuilder>(File);
            sb.EndNamespace();
        }

       static void WriteClass(IDataContext context)
        {
            var sb        = context.Get<PaddedStringBuilder>(File);
            var config    = context.Get(Data.Config);
            var tableInfo = context.Get(Data.TableInfo);

            ContractCommentInfoCreator.Write(sb, tableInfo);

            var inheritancePart = String.Empty;

            if (config.EntityContractBase != null)
            {
                inheritancePart = ": " + config.EntityContractBase;
            }

            sb.AppendLine("[Serializable]");
            sb.AppendLine($"public sealed class {tableInfo.TableName.ToContractName()}Contract {inheritancePart}");
            sb.AppendLine("{");
            sb.PaddingCount++;

            ContractCommentInfoCreator.Write(sb, tableInfo);
            sb.AppendLine("// ReSharper disable once EmptyConstructor");
            sb.AppendLine($"public {tableInfo.TableName.ToContractName()}Contract()");
            sb.AppendLine("{");
            sb.AppendLine("}");
            sb.AppendLine();

            sb.AppendAll(ContractBodyDbMembersCreator.Create(tableInfo).PropertyDefinitions);
            sb.AppendLine();

            sb.PaddingCount--;
            sb.AppendLine("}"); // end of class
        }

         static void WriteUsingList(IDataContext context)
        {
            var sb     = context.Get<PaddedStringBuilder>(File);
            var namingPattern = context.Get(NamingPattern.Id);
            
            foreach (var line in namingPattern.EntityUsingLines)
            {
                sb.AppendLine(line);
            }
        }
        #endregion
    }
}