using System;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.TypescriptModelGeneration;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BDataGridRenderer
    {

        internal static string EvaluateMethodNameOfGridColumns(WriterContext writerContext, BDataGrid data)
        {
            var last = data?.DataSourceBindingPath?.SplitAndClear(".")?.LastOrDefault();

            return "getDataGridColumnsOf" + last;
        }

        internal static string EvaluateMethodBodyOfGridColumns(string methodName, WriterContext writerContext, BDataGrid data)
        {
            var sb = writerContext.Output;
            sb.AppendLine(methodName+"() : any[]");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("let columns = [];");
            foreach (var bDataGridColumnInfo in data.Columns)
            {
                
                sb.AppendLine("// "+bDataGridColumnInfo.BindingPath);

                if (bDataGridColumnInfo.IsVisibleBindingPath.HasValue())
                {
                    var bindingPath = TypescriptNaming.NormalizeBindingPath(BindingPrefix.Value + bDataGridColumnInfo.IsVisibleBindingPath);
                    sb.AppendLine($"if( !{bindingPath} )");
                    sb.AppendLine("{");
                    sb.AppendLine("    continue;");
                    sb.AppendLine("}");
                }


                sb.AppendLine("let column = {};");
                sb.AppendLine($"column.key = \"{TypescriptNaming.NormalizeBindingPath(bDataGridColumnInfo.BindingPath)}\";");

                var labelValue = RenderHelper.GetLabelValue(writerContext.ScreenInfo, bDataGridColumnInfo.Label);
                if (labelValue != null)
                {
                    sb.AppendLine($"column.name = {labelValue}");
                }


                var solutionInfo = SolutionInfo.CreateFromTfsFolderPath(writerContext.ScreenInfo.TfsFolderName);

                var propertyDefinition = CecilHelper.FindPropertyInfo(solutionInfo.TypeAssemblyPathInServerBin, writerContext.ScreenInfo.RequestName, data.BindingPath);

                var isInt32 = propertyDefinition.PropertyType.FullName == typeof(int).FullName;
                var isDecimal = propertyDefinition.PropertyType.FullName == typeof(decimal).FullName;
                var isDate = propertyDefinition.PropertyType.FullName == typeof(DateTime).FullName;


                if (isInt32)
                {
                    sb.AppendLine("column.type = 'number';");    
                }
                else if (isDecimal)
                {
                    sb.AppendLine("column.type = 'number';"); 
                    sb.AppendLine("column.numberFormat = 'M';"); 
                    
                }
                else if (isDate)
                {
                    sb.AppendLine("column.type = 'date';");    
                }

                sb.AppendLine("columns.push(column);"); 
                
            }


            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine("return columns;");

            return sb.ToString();
        }

        #region Public Methods
        public static void Write(WriterContext writerContext, BDataGrid data)
        {
            writerContext.Imports.Add("import { BDataGrid } from \"b-data-grid\"");

            var sb         = writerContext.Output;
            var screenInfo = writerContext.ScreenInfo;

            SnapNamingHelper.InitSnapName(data);

            
            var methodNameOfGridColumns = EvaluateMethodNameOfGridColumns(writerContext,data);
            writerContext.ClassBody.Add(EvaluateMethodBodyOfGridColumns(methodNameOfGridColumns,writerContext,data));


            sb.AppendLine($"<BDataGrid  dataSource = {{{data.DataSourceBindingPathInTypeScript}}}");

            sb.AppendLine("selectable={'single'}");
            sb.AppendLine("ref = {(r: any) => this.snaps." + data.SnapName + " = r}");

            



            sb.AppendLine("columns = {this."+methodNameOfGridColumns+"(request)}");
            sb.AppendLine("onRowSelectionChanged={this.onRowSelectionChanged}");

            sb.AppendLine("headerBarOptions={{");

            sb.PaddingCount++;

            var labelValue = RenderHelper.GetLabelValue(screenInfo, data.TitleInfo);
            if (labelValue != null)
            {
                sb.Append("showTitle:true,");
                sb.Append($"title = {{{labelValue}}},");
            }

            sb.AppendLine("show: true,");
            sb.AppendLine("showMoreOptions: true,");
            sb.AppendLine("showFiltering: true,");
            sb.AppendLine("showGrouping: true");
            sb.PaddingCount--;
            sb.AppendLine("}}");

            sb.AppendLine("context = {context}/>");
        }
        #endregion
    }
}