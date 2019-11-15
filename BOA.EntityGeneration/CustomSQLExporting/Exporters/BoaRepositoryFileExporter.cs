﻿using BOA.Common.Helpers;
using BOA.DataFlow;
using BOA.EntityGeneration.DataFlow;
using BOA.EntityGeneration.ScriptModel;
using static BOA.EntityGeneration.CustomSQLExporting.Wrapper.CustomSqlExporter;

namespace BOA.EntityGeneration.CustomSQLExporting.Exporters
{
    public static class BoaRepositoryFileExporter
    {
        #region Static Fields
        public static readonly IDataConstant<PaddedStringBuilder> File = DataConstant.Create<PaddedStringBuilder>(nameof(BoaRepositoryFileExporter) + "->" + nameof(File));
        #endregion

        #region Public Methods
        public static void AttachEvents(IDataContext context)
        {
            context.AttachEvent(OnProfileInfoInitialized, InitializeOutput);
            context.AttachEvent(OnProfileInfoInitialized, BeginNamespace);
            context.AttachEvent(OnProfileInfoInitialized, WriteProxyClass);

            context.AttachEvent(OnCustomSqlInfoInitialized, WriteBoaRepositoryClass);

            context.AttachEvent(OnProfileInfoRemove, EndNamespace);
            context.AttachEvent(OnProfileInfoRemove, ExportFileToDirectory);
            context.AttachEvent(OnProfileInfoRemove, ClearOutput);
        }
        #endregion

        #region Methods
        static void BeginNamespace(IDataContext context)
        {
            var sb   = context.Get(File);
            var data = context.Get(CustomSqlProfileInfo);

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

        static void ClearOutput(IDataContext context)
        {
            context.Remove(File);
        }

        static void EndNamespace(IDataContext context)
        {
            var sb = context.Get(File);
            sb.CloseBracket();
        }

        static void ExportFileToDirectory(IDataContext context)
        {
            var sb         = context.Get(File);
            var config     = context.Get(Data.Config);
            var fileAccess = context.Get(Data.FileAccess);

            var filePath = config.CustomSQLOutputFilePathForBoaRepository.Replace("{ProfileId}", context.Get(ProfileId));

            fileAccess.WriteAllText(filePath, sb.ToString());
        }

        static void InitializeOutput(IDataContext context)
        {
            context.Add(File, new PaddedStringBuilder());
        }

        static void WriteBoaRepositoryClass(IDataContext context)
        {
            var sb                   = context.Get(File);
            var projectCustomSqlInfo = context.Get(CustomSqlProfileInfo);
            var data                 = context.Get(CustomSqlInfo);

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
            sb.AppendLine("/// <summary>");
            sb.AppendLine($"///{Padding.ForComment}Data access part of '{data.Name}' sql.");
            sb.AppendLine("/// </summary>");
            if (data.SqlResultIsCollection)
            {
                sb.AppendLine($"public GenericResponse<List<{data.ResultContractName}>> Execute({data.ParameterContractName} request)");
                sb.OpenBracket();
                sb.AppendLine($"var sqlInfo = Shared.{data.BusinessClassName}.CreateSqlInfo(request);");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteCustomSql<{data.ResultContractName}>(CallerMemberPath, sqlInfo, ReadContract, ProfileId, ObjectId);");
                sb.CloseBracket();
            }
            else
            {
                sb.AppendLine($"public GenericResponse<{data.ResultContractName}> Execute({data.ParameterContractName} request)");
                sb.OpenBracket();

                sb.AppendLine($"var sqlInfo = Shared.{data.BusinessClassName}.CreateSqlInfo(request);");
                sb.AppendLine();
                sb.AppendLine($"return this.ExecuteCustomSqlForOneRecord<{data.ResultContractName}>(CallerMemberPath, sqlInfo, ReadContract, ProfileId, ObjectId);");

                sb.CloseBracket();
            }

            sb.AppendLine();

            sb.CloseBracket();
        }

        static void WriteProxyClass(IDataContext context)
        {
            var sb             = context.Get(File);
            var customSqlInfos = context.Get(ProcessedCustomSqlInfoListInProfile);

            sb.AppendLine("public static class CustomSql");
            sb.OpenBracket();

            sb.AppendLine("public static TOutput Execute<TOutput, T>(ObjectHelper objectHelper, ICustomSqlProxy<TOutput, T> input) where TOutput : GenericResponse<T>");
            sb.OpenBracket();

            sb.AppendLine("switch (input.Index)");
            sb.OpenBracket();

            foreach (var customSqlInfo in customSqlInfos)
            {
                sb.AppendLine($"case {customSqlInfo.SwitchCaseIndex}:");
                sb.OpenBracket();
                sb.AppendLine($"return (TOutput) (object) new {customSqlInfo.BusinessClassName}(objectHelper.Context).Execute(({customSqlInfo.ParameterContractName})(object) input);");
                sb.CloseBracket();
            }

            sb.CloseBracket(); // end of switch

            sb.AppendLine();
            sb.AppendLine("throw new System.InvalidOperationException(input.GetType().FullName);");

            sb.CloseBracket(); // end of method

            sb.CloseBracket(); // end of class
        }
        #endregion
    }
}