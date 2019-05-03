using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.JsxElementModel;

namespace BOA.OneDesigner.CodeGeneration
{
    static class BTabBarRenderer
    {
        #region Public Methods
        public static void DefineTabPage(WriterContext writerContextMain, BTabBarPage tabPage)
        {
            writerContextMain.Imports.Add("import { BPageModule, BPageModuleComposer } from \"b-page-module\";");

            var sb = new PaddedStringBuilder();

            var writerContext = new WriterContext
            {
                ClassName                             = writerContextMain.ClassName,
                HasWorkflow                           = writerContextMain.HasWorkflow,
                ScreenInfo                            = writerContextMain.ScreenInfo,
                IsBrowsePage                          = writerContextMain.IsBrowsePage,
                SolutionInfo                          = writerContextMain.SolutionInfo,
                ThrowExceptionOnEmptyActionDefinition = writerContextMain.ThrowExceptionOnEmptyActionDefinition,
                RequestIntellisenseData = writerContextMain.RequestIntellisenseData,
                IsTabPage = true,
                ExecuteWindowRequestFunctionAccessPath = "this.state.pageInstance.executeWindowRequest"
            };

            var functionRender = new RenderFunctionDefinition {WriterContext = writerContext, Data = tabPage.DivAsCardContainer, WindowRequestAccessPath = "this.state.pageInstance.getWindowRequest().body"};
            var renderMethod   = functionRender.GetCode();

            var functionFillWindowRequest = new FillWindowRequestFunctionDefinition {FillRequestFromUI = writerContext.FillRequestFromUI, HasTabControl = false};
            var fillRequestMethod         = functionFillWindowRequest.GetCode();


            sb.AppendLine($"class {tabPage.ClassName} extends BPageModule");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("constructor(props: any)");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("super(props);");
            sb.AppendLine("this.state.pageInstance.onProxyDidRespond.push(this);");
            sb.AppendLine("this.state.pageInstance.onFillRequestFromUI.push(this);");

            sb.PaddingCount--;
            sb.AppendLine("}");

            sb.AppendLine();
            sb.AppendLine("proxyDidRespond()");
            sb.AppendLine("{");
            sb.PaddingCount++;

            sb.AppendLine("this.setState({ windowRequest: this.state.pageInstance.state.windowRequest });");

            sb.PaddingCount--;
            sb.AppendLine("}");

            foreach (var member in writerContext.ClassBody)
            {
                sb.AppendLine();
                sb.AppendAll(member.Code);
                sb.AppendLine();
            }


           

            sb.AppendLine();
            sb.AppendAll(fillRequestMethod);

            sb.AppendLine();
            sb.AppendAll(renderMethod);

            sb.PaddingCount--;
            sb.AppendLine("}");
            sb.AppendLine($"var {tabPage.ComposedName} = BPageModuleComposer({tabPage.ClassName});");

            writerContextMain.Page.Add(sb.ToString());
            writerContextMain.Imports.AddRange(writerContext.Imports);

            if ( writerContext.HandleProxyDidRespondCallback)
            {
                writerContextMain.HandleProxyDidRespondCallback = true;
            }

            writerContextMain.Imports.AddRange(writerContext.Imports);
            
        }

        public static void Write(WriterContext writerContext, BTabBar data)
        {
            writerContext.HasTabControl = true;

            var sb = writerContext.Output;

            foreach (var tabPage in data.Items)
            {
                DefineTabPage(writerContext, tabPage);
            }

            sb.AppendLine("tabItems =");
            sb.AppendLine("{[");
            sb.PaddingCount++;

            for (var i = 0; i < data.Items.Count; i++)
            {
                var tabItemsValueRenderer = new TabItemsValueRenderer
                {
                    WriterContext             = writerContext,
                    TabBarPage                = data.Items[i],
                    TabIndex                  = i,
                    IsLastTab                 = i == data.Items.Count - 1,
                    ActiveTabIndexBindingPath = data.ActiveTabIndexBindingPath
                };

                var code = tabItemsValueRenderer.GetCode();

                sb.AppendAll(code);
                sb.AppendLine();
            }

            sb.PaddingCount--;
            sb.AppendLine("]}");
            
            
        }
        #endregion
    }

    class TabItemsValueRenderer
    {
        #region Public Properties
        public string        ActiveTabIndexBindingPath { get; set; }
        public bool          IsLastTab                 { get; set; }
        public BTabBarPage   TabBarPage                { get; set; }
        public int           TabIndex                  { get; set; }
        public WriterContext WriterContext             { get; set; }
        #endregion

        #region Public Methods
        public string GetCode()
        {
            var sb = new PaddedStringBuilder();

            sb.AppendLine("{");
            sb.PaddingCount++;

            RenderHelper.WriteLabelInfo(WriterContext, TabBarPage.TitleInfo, sb.AppendLine, "text:", ",");

            sb.AppendLine($"value : {TabIndex},");

            var tabRenderCode = "<" + TabBarPage.ComposedName + " pageInstance={this} ref={(r: any) => this.snaps." + TabBarPage.ComposedName + " = r} />";

            sb.AppendLine($"content: ({tabRenderCode})");

            if (IsLastTab)
            {
                sb.AppendLine(",");

                var pathInState = "this.state.selectedTabIndex";

                if (ActiveTabIndexBindingPath.HasValue())
                {
                    var activeTabIndexBindingPath = new JsBindingPathInfo(ActiveTabIndexBindingPath)
                    {
                        EvaluateInsStateVersion = true
                    };
                    JsBindingPathCalculator.CalculateBindingPathInRenderMethod(activeTabIndexBindingPath);
                    WriterContext.PushVariablesToRenderScope(activeTabIndexBindingPath);

                    pathInState = activeTabIndexBindingPath.FullBindingPathInJs;
                }

                sb.AppendLine($"selectedTabIndex: {pathInState}|0,");
                sb.AppendLine("onTabChange: (tabInfo: any, tabIndex: number)=>");
                sb.AppendLine("{");
                sb.AppendLine($"    {pathInState} = tabIndex;");
                sb.AppendLine("}");
            }

            sb.PaddingCount--;
            sb.AppendLine("}");

            if (!IsLastTab)
            {
                sb.AppendLine(",");
            }

            return sb.ToString();
        }
        #endregion
    }
}