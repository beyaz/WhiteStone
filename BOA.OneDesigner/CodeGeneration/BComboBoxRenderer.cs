﻿using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    internal static class BComboBoxRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, BComboBox data)
        {
            writerContext.Imports.Add("import { BComboBox } from \"b-combo-box\"");

            var sb         = writerContext.Output;
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

            var bindingPathPropertyInfo = writerContext.ScreenInfo.GetBindingPathPropertyInfo(data.SelectedValueBindingPath);
            if (bindingPathPropertyInfo == null)
            {
                throw Error.BindingPathShouldHaveValue(data.LabelInfo.GetDesignerText(), data.SelectedValueBindingPath);
            }

            var jsBindingPath = new JsBindingPathInfo(data.SelectedValueBindingPath)
            {
                EvaluateInsStateVersion = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            var valueMemberPath   = TypescriptNaming.NormalizeBindingPath(data.ValueMemberPath);
            var displayMemberPath = TypescriptNaming.NormalizeBindingPath(data.DisplayMemberPath);

            if (valueMemberPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(valueMemberPath));
            }

            if (displayMemberPath.IsNullOrWhiteSpace())
            {
                throw Error.BindingPathShouldHaveValue(data.Label, nameof(displayMemberPath));
            }

            var dataSourceBindingPath = new JsBindingPathInfo(data.DataGrid.DataSourceBindingPath)
            {
                EvaluateInsStateVersion = false
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(dataSourceBindingPath);
            writerContext.PushVariablesToRenderScope(dataSourceBindingPath);

            sb.AppendLine($"<BComboBox  dataSource = {{{dataSourceBindingPath.FullBindingPathInJs}}}");
            sb.PaddingCount++;

            var fullBindingPathInJs = jsBindingPath.FullBindingPathInJs;

            writerContext.GrabValuesToRequest(new ComponentGetValueInfoComboBox
            {
                JsBindingPath           = fullBindingPathInJs,
                SnapName                = data.SnapName,
                BindingPathPropertyInfo = bindingPathPropertyInfo
            });

            var hasSupportErrorText = writerContext.HasSupportErrorText;

            if (data.IsMultiSelect)
            {
                sb.AppendLine($"value={{{fullBindingPathInJs}}}");

                var shouldWriteOnSelectEvent = data.ValueChangedAction.HasValue() || hasSupportErrorText;
                if (shouldWriteOnSelectEvent)
                {
                    sb.AppendLine("onSelect={(selectedIndexes: any[], selectedItems: any[], selectedValues: any[]) =>");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    if (hasSupportErrorText)
                    {
                        sb.AppendLine($"{Config.ClearErrorTextMethodPathInJs}(\"{fullBindingPathInJs}\");");
                    }

                    if (data.ValueChangedAction?.HasValue() == true)
                    {
                        RenderHelper.InitLabelValues(writerContext, data.ValueChangedAction);

                        var function = new ActionInfoFunction
                        {
                            Data          = data.ValueChangedAction,
                            WriterContext = writerContext
                        };

                        sb.AppendAll(function.GetCode());
                        sb.AppendLine();
                    }

                    sb.PaddingCount--;
                    sb.AppendLine("}}");
                }
            }
            else
            {
                sb.AppendLine($"value={{[({fullBindingPathInJs} || \"\") + \"\"]}}");

                var shouldWriteOnSelectEvent = data.ValueChangedAction.HasValue() || hasSupportErrorText;

                if (shouldWriteOnSelectEvent)
                {
                    sb.AppendLine("onSelect={(selectedIndexes: any[], selectedItems: any[]) =>");
                    sb.AppendLine("{");
                    sb.PaddingCount++;

                    if (hasSupportErrorText)
                    {
                        sb.AppendLine($"{Config.ClearErrorTextMethodPathInJs}(\"{fullBindingPathInJs}\");");
                    }

                    if (data.ValueChangedAction.HasValue())
                    {
                        RenderHelper.InitLabelValues(writerContext, data.ValueChangedAction);

                        var function = new ActionInfoFunction
                        {
                            Data          = data.ValueChangedAction,
                            WriterContext = writerContext
                        };

                        sb.AppendAll(function.GetCode());
                        sb.AppendLine();
                    }

                    sb.PaddingCount--;
                    sb.AppendLine("}}");
                }
            }

            if (hasSupportErrorText)
            {
                RenderHelper.WriteErrorTextProperty(sb, fullBindingPathInJs);
            }

            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            RenderHelper.WriteLabelInfo(writerContext, data.LabelInfo, sb.AppendLine, "labelText");

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

            RenderHelper.WriteSize(data.SizeInfo, sb.AppendLine);
            RenderHelper.WriteIsVisible(writerContext, data.IsVisibleBindingPath, sb);
            RenderHelper.WriteIsDisabled(writerContext, data.IsDisabledBindingPath, sb);

            sb.AppendLine("context = {context}/>");

            sb.PaddingCount--;
        }
        #endregion

        #region Methods
        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BComboBox data)
        {
            var sb = new PaddedStringBuilder();
            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Gets the column definition of " + data.SnapName + ".");
                sb.AppendLine("  */");
            }

            sb.AppendLine(methodName + "(request:any) : any[]");
            sb.AppendLine("{");
            sb.PaddingCount++;

            BDataGridRenderer.WriteColumns(writerContext, sb, data.DataGrid);

            sb.AppendLine();
            sb.AppendLine("return columns;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }
        #endregion
    }
}