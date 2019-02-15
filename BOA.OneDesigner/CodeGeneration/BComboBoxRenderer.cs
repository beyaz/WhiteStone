﻿using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    internal static class BComboBoxRenderer
    {

       
        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BComboBox data)
        {
            var sb = new PaddedStringBuilder();
            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Gets the column definition of "+ data.SnapName+".");
                sb.AppendLine("  */");
            }
            sb.AppendLine(methodName + "(request:any) : any[]");
            sb.AppendLine("{");
            sb.PaddingCount++;


            BDataGridRenderer.WriteColumns(writerContext,sb,data.DataGrid);

            

            sb.AppendLine();
            sb.AppendLine("return columns;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        #region Public Methods

        public static void Write(WriterContext writerContext, BComboBox data)
        {
            writerContext.Imports.Add("import { BComboBox } from \"b-combo-box\"");

            var sb = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            if (data.SelectedValueBindingPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(data.SelectedValueBindingPath));
            }
            SnapNamingHelper.InitSnapName(writerContext, data);

            writerContext.AddClassBody(EvaluateMethodBodyOfGridColumns(data.TypeScriptMethodNameOfGetGridColumns, writerContext, data));

            var propertyDefinition = CecilHelper.FindPropertyInfo(writerContext.SolutionInfo.TypeAssemblyPathInServerBin, screenInfo.RequestName, data.SelectedValueBindingPath);

            if (data.IsMultiSelect)
            {
                if (CecilHelper.IsCollection(propertyDefinition?.PropertyType) == false)
                {
                    throw Error.InvalidOperation($"Is Multi Select true  ise {data.SelectedValueBindingPath} değeri collection tipinde olmalıdır.");
                }
            }
            

            var selectedValueBindingPath = RenderHelper.NormalizeBindingPathInRenderMethod( writerContext, data.SelectedValueBindingPath);
            var valueMemberPath = TypescriptNaming.NormalizeBindingPath(data.ValueMemberPath);
            var displayMemberPath = TypescriptNaming.NormalizeBindingPath(data.DisplayMemberPath);

            if (valueMemberPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(valueMemberPath));
            }

            if (displayMemberPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(displayMemberPath));
            }

            var dataSourceBindingPathInTypeScript = RenderHelper.NormalizeBindingPathInRenderMethod( writerContext, data.DataGrid.DataSourceBindingPath);

            sb.AppendLine($"<BComboBox  dataSource = {{{dataSourceBindingPathInTypeScript}}}");
            sb.PaddingCount++;

            
            if (data.IsMultiSelect)
            {
                sb.AppendLine($"value={{{selectedValueBindingPath}}}");
                sb.AppendLine("onSelect={(selectedIndexes: any[], selectedItems: any[], selectedValues: any[]) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine($"{selectedValueBindingPath} = selectedValues;");

                if (data.ValueChangedOrchestrationMethod.HasValue())
                {
                    sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                }

                sb.PaddingCount--;
                sb.AppendLine("}}");
            }
            else
            {
                sb.AppendLine($"value={{[({selectedValueBindingPath}||\"\")+\"\"]}}");
                sb.AppendLine("onSelect={(selectedIndexes: any[], selectedItems: any[]) =>");
                sb.AppendLine("{");
                sb.PaddingCount++;

                sb.AppendLine("if (selectedItems && selectedItems.length === 1)");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{selectedValueBindingPath} = selectedItems[0].{valueMemberPath};");
                sb.PaddingCount--;
                sb.AppendLine("}");
                sb.AppendLine("else");
                sb.AppendLine("{");
                sb.PaddingCount++;
                sb.AppendLine($"{selectedValueBindingPath} = null;");
                sb.PaddingCount--;
                sb.AppendLine("}");


                if (data.ValueChangedOrchestrationMethod.HasValue())
                {
                    sb.AppendLine($"this.executeWindowRequest(\"{data.ValueChangedOrchestrationMethod}\");");
                }

                sb.PaddingCount--;
                sb.AppendLine("}}");
            }

           

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            RenderHelper.WriteLabelInfo(writerContext, data.LabelInfo,sb.AppendLine,"labelText");

            sb.AppendLine("columns = {this." + data.TypeScriptMethodNameOfGetGridColumns + "(request)}");
            sb.AppendLine("multiColumn={true}");
            if (data.IsMultiSelect)
            {
                sb.AppendLine("multiSelect={true}");
            }
            else
            {
                sb.AppendLine("multiSelect={false}");
            }
            sb.AppendLine($"valueMemberPath=\"{valueMemberPath}\"");
            sb.AppendLine($"displayMemberPath=\"{displayMemberPath}\"");

            if (data.SizeInfo.HasValue())
            {
                sb.AppendLine("size = {"+ RenderHelper.GetJsValue(data.SizeInfo) +"}");
            }

            
            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);

            

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }

        #endregion Public Methods
    }
}