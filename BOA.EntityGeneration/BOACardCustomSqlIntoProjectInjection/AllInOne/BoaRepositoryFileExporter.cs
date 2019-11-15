using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.MethodWriters;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;

namespace BOA.EntityGeneration.BOACardCustomSqlIntoProjectInjection.AllInOne
{
   
    public static class BoaRepositoryFileExporter
    {



        public static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(BoaRepositoryFileExporter) +"->"+ nameof(File));



        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }
        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }



        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, InitializeOutput);
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, BeginNamespace);
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, WriteProxy);
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, EndNamespace);
            
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, ExportFileToDirectory);
            context.AttachEvent(CustomSqlExportingEvent.ProfileIdExportingIsStarted, ClearOutput);

            context.AttachEvent(CustomSqlExportingEvent.ObjectIdExportIsStarted, WriteBoaRepositoryClass);
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb         = context.Get(File);
            var data       = context.Get(CustomSqlExporter.CustomSqlInfoProject);
            var fileAccess = context.Get(Data.FileAccess);

            
            fileAccess.WriteAllText(data.BusinessProjectPath + "\\Generated\\CustomSql.cs", sb.ToString());
        }

        static void BeginNamespace(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlExporter.CustomSqlInfoProject);

            sb.AppendLine("using BOA.Base;");
            sb.AppendLine("using BOA.Base.Data;");
            sb.AppendLine("using BOA.Common.Types;");
            sb.AppendLine($"using {data.NamespaceNameOfType};");
            sb.AppendLine("using System.Data;");
            sb.AppendLine("using System.Collections.Generic;");

            sb.AppendLine();
            sb.AppendLine($"namespace {data.NamespaceNameOfBusiness}");
            sb.AppendLine("{");
            sb.PaddingCount++;
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.CloseBracket();
        }


        static void WriteProxy(IDataContext context)
        {
            var sb   = context.Get(File);
            var project = context.Get(CustomSqlExporter.CustomSqlInfoProject);

           

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



        

        #region Methods
      
        static void WriteBoaRepositoryClass(IDataContext context)
        {
            var sb      = context.Get(File);
            var projectCustomSqlInfo = context.Get(CustomSqlExporter.CustomSqlInfoProject);
            var data = context.Get(CustomSqlExporter.CustomSqlInfo);

            var key = $"{projectCustomSqlInfo.NamespaceNameOfBusiness}.{data.BusinessClassName}.Execute";

            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public sealed class {data.BusinessClassName} : ObjectHelper");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine($"const string CallerMemberPath = \"{key}\";");
            sb.AppendLine($"const string ProfileId        = \"{projectCustomSqlInfo.ProfileId}\";");
            sb.AppendLine($"const string ObjectId         = \"{data.Name}\";");
            sb.AppendLine();
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            sb.AppendLine($"public {data.BusinessClassName}(ExecutionDataContext context) : base(context) {{}}");
            sb.AppendLine();

            if (data.SqlResultIsCollection)
            {
                ExecuteForListMethodWriter.Write(sb, data);
            }
            else
            {
                ExecuteForSingleContractMethodWriter.Write(sb, data);
            }

            sb.AppendLine();
            ReadContractMethodWriter.Write(sb, data);
            sb.AppendLine();
            CreateSqlInfoMethodWriter.Write(sb, data);

            sb.PaddingCount--;
            sb.AppendLine("}");
        }
        #endregion
    }
}