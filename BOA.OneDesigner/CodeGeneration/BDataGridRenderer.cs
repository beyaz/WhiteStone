using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationComponentGetValueModels;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BDataGridRenderer
    {
        #region Public Methods
        public static void Write(WriterContext writerContext, BDataGrid data)
        {
            var isBrowsePageDataGrid = (data.Container as BCard)?.IsBrowsePageDataGridContainer == true;

            var sb = writerContext.Output;

            SnapNamingHelper.InitSnapName(data);

            var methodNameOfGridColumns = EvaluateMethodNameOfGridColumns(writerContext, data);
            writerContext.AddClassBody(EvaluateMethodBodyOfGridColumns(methodNameOfGridColumns, writerContext, data));

            var canWriteRowSelectionChangedMethod = string.IsNullOrWhiteSpace(data.RowSelectionChangedOrchestrationMethod) == false;

            var rowSelectionChangedMethodName = EvaluateMethodNameOfGridRowSelectionChanged(writerContext, data);

            if (canWriteRowSelectionChangedMethod)
            {
                writerContext.AddClassBody(EvaluateMethodBodyOfGridRowSelectionChanged(rowSelectionChangedMethodName, writerContext, data, isBrowsePageDataGrid));

                if (isBrowsePageDataGrid)
                {
                    writerContext.ConstructorBody.Add($"this.onRowSelectionChanged = this.{rowSelectionChangedMethodName}.bind(this);");
                }
                else
                {
                    writerContext.ConstructorBody.Add($"this.{rowSelectionChangedMethodName} = this.{rowSelectionChangedMethodName}.bind(this);");
                }
            }

            if (isBrowsePageDataGrid)
            {
                var dataSourceBindingPathInTypeScript = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.DataSourceBindingPath);
                writerContext.StateObjectWhenIncomingRequestIsSuccess.Add("dataSource", RenderHelper.ConvertBindingPathToIncomingRequest(dataSourceBindingPathInTypeScript));
                writerContext.StateObjectWhenIncomingRequestIsSuccess.Add("columns", $"this.{methodNameOfGridColumns}({Config.IncomingRequestVariableName})");

                return;
            }

            var jsBindingPath = new JsBindingPathInfo(data.DataSourceBindingPath)
            {
                EvaluateInsStateVersion = false
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);
            writerContext.PushVariablesToRenderScope(jsBindingPath);

            writerContext.Imports.Add("import { BDataGrid } from \"b-data-grid-dx\"");

            sb.AppendLine($"<BDataGrid  dataSource = {{{jsBindingPath.FullBindingPathInJs}}}");

            sb.PaddingCount++;
            sb.AppendLine("selectable={'single'}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            sb.AppendLine("columns = {this." + methodNameOfGridColumns + "(request)}");
            if (canWriteRowSelectionChangedMethod)
            {
                sb.AppendLine("onRowSelectionChanged={this." + rowSelectionChangedMethodName + "}");
            }

            sb.AppendLine("headerBarOptions={{");

            sb.PaddingCount++;

            RenderHelper.WriteLabelInfo(writerContext, data.TitleInfo, sb.AppendLine, "title:");

            sb.PaddingCount--;
            sb.AppendLine("}}");

           
            RenderHelper.WriteSize(data.SizeInfo,sb.AppendLine);

            sb.AppendLine("context = {context}/>");
        }

        public static void WriteColumns(WriterContext writerContext, PaddedStringBuilder sb, BDataGrid data)
        {
            sb.AppendLine("const columns: any[] = [];");
            sb.AppendLine();

            foreach (var bDataGridColumnInfo in data.Columns)
            {
                var jsObject = new JsObject();

                var writeVisibleCondition = bDataGridColumnInfo.IsVisibleBindingPath.HasValue();
                if (writeVisibleCondition)
                {
                    var bindingPath = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + bDataGridColumnInfo.IsVisibleBindingPath);
                    sb.AppendLine($"if( {bindingPath} )");
                    sb.AppendLine("{");

                    sb.PaddingCount++;
                }

                jsObject.Add("key", $"\"{TypescriptNaming.NormalizeBindingPath(bDataGridColumnInfo.BindingPath)}\"");

                var labelValue = RenderHelper.GetLabelValue(writerContext, bDataGridColumnInfo.Label);
                if (labelValue != null)
                {
                    jsObject.Add("name", labelValue);
                }

                var propertyInfo = writerContext.RequestIntellisenseData.FindPropertyInfoInCollectionFirstGenericArgumentType(data.DataSourceBindingPath, bDataGridColumnInfo.BindingPath);

                if (propertyInfo != null)
                {
                    if (propertyInfo.IsDecimal || propertyInfo.IsDecimalNullable)
                    {
                        jsObject.Add("type", "\"decimal\"");
                        jsObject.Add("numberFormat", "\"M\"");
                    }
                    else if (propertyInfo.IsNumber)
                    {
                        jsObject.Add("type", "\"number\"");
                    }
                    else if (propertyInfo.IsDate || propertyInfo.IsDateNullable)
                    {
                        jsObject.Add("type", "\"date\"");
                    }
                    else if (propertyInfo.IsBoolean || propertyInfo.IsBooleanNullable)
                    {
                        jsObject.Add("type", "\"boolean\"");
                    }
                }

                sb.AppendLine($"columns.push({JsObjectInfoSingleLineWriter.ToString(jsObject)});");

                if (writeVisibleCondition)
                {
                    sb.PaddingCount--;
                    sb.AppendLine("}");
                }
            }
        }
        #endregion

        #region Methods
        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BDataGrid data)
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

            WriteColumns(writerContext, sb, data);

            sb.AppendLine();
            sb.AppendLine("return columns;");
            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        internal static string EvaluateMethodBodyOfGridRowSelectionChanged(string methodName, WriterContext writerContext, BDataGrid data, bool isBrowsePageDataGrid)
        {
            var sb = new PaddedStringBuilder();

            if (RenderHelper.IsCommentEnabled)
            {
                sb.AppendLine("/**");
                sb.AppendLine("  *  Occurs when the component of '" + data.SnapName + "' row selection changed.");
                sb.AppendLine("  */");
            }

            sb.AppendLine(methodName + "()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            var fieldPath = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.SelectedRowDataBindingPath);

            var propertyDefinition = CecilHelper.FindPropertyInfo(writerContext.SolutionInfo.TypeAssemblyPathInServerBin, writerContext.ScreenInfo.RequestName, data.SelectedRowDataBindingPath);

            var isCollection = CecilHelper.IsCollection(propertyDefinition?.PropertyType);

            if (isBrowsePageDataGrid)
            {
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoDataGridSelectedValueChangedBindingValueInBrowseForm
                {
                    JsBindingPath = fieldPath,
                    SnapName = data.SnapName,
                    IsCollection = isCollection
                });    
            }
            else
            {
                writerContext.GrabValuesToRequest(new ComponentGetValueInfoDataGridSelectedValueChangedBindingValue
                {
                    JsBindingPath = fieldPath,
                    SnapName = data.SnapName,
                    IsCollection = isCollection
                });    
            }
            

            
           
            sb.AppendLine($"{writerContext.ExecuteWindowRequestFunctionAccessPath}(\"{data.RowSelectionChangedOrchestrationMethod}\");");

            sb.PaddingCount--;
            sb.AppendLine("}");

            return sb.ToString();
        }

        internal static string EvaluateMethodNameOfGridColumns(WriterContext writerContext, BDataGrid data)
        {
            var last = data?.DataSourceBindingPath?.SplitAndClear(".")?.LastOrDefault();

            return "getDataGridColumnsOf" + last;
        }

        internal static string EvaluateMethodNameOfGridRowSelectionChanged(WriterContext writerContext, BDataGrid data)
        {
            var last = data?.DataSourceBindingPath?.SplitAndClear(".")?.LastOrDefault();

            return "on" + last + "RowSelectionChanged";
        }
        #endregion
    }
}