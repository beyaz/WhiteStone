﻿using BOA.OneDesigner.CodeGenerationModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data = BOA.OneDesigner.CodeGeneration.ButtonActionInfo;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class ButtonActionInfoFunctionTest
    {
        #region Fields
        ButtonActionInfoFunction api;
        #endregion

        #region Public Methods
        [TestMethod]
        public void _0_only_OpenFormWithResourceCode()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode = "x"
            };

            OutputShouldBe("BFormManager.show(\"x\", /*data*/null, true,null);");
        }

        [TestMethod]
        public void _1_only_ExtensionMethodName()
        {
            api.Data = new Data
            {
                ExtensionMethodName = "x"
            };

            OutputShouldBe("Extension.x(this);");

            ChangeToTabPage();

            OutputShouldBe("Extension.x(this.state.pageInstance);");
        }

        [TestMethod]
        public void _2_only_OrchestrationMethodName()
        {
            api.Data = new Data
            {
                OrchestrationMethodName = "x"
            };

            OutputShouldBe("this.executeWindowRequest(\"x\");");

            ChangeToTabPage();

            OutputShouldBe("this.state.pageInstance.executeWindowRequest(\"x\");");
        }

        [TestMethod]
        public void _3_OpenFormWithResourceCode_with_data()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OpenFormWithResourceCodeDataParameterBindingPath = "o.f"
            };

            OutputShouldBe("BFormManager.show(\"x\", /*data*/this.state.windowRequest.o.f, true,null);");

            ChangeToTabPage();

            OutputShouldBe("BFormManager.show(\"x\", /*data*/this.state.windowRequest.o.f, true,null);");
        }

        [TestMethod]
        public void _4_OpenFormWithResourceCode_after_orch_call()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode = "x",
                OrchestrationMethodName  = "GetInfo"
            };

            OutputShouldBe(@"

this.internalProxyDidRespondCallback = () =>
{
    const data:any = null;

    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", data, showAsNewPage,menuItemSuffix);
}

this.executeWindowRequest(""GetInfo"");
");

            ChangeToTabPage();

            OutputShouldBe(@"

this.state.pageInstance.internalProxyDidRespondCallback = () =>
{
    const data:any = null;

    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", data, showAsNewPage,menuItemSuffix);
}

this.state.pageInstance.executeWindowRequest(""GetInfo"");
");
        }

        [TestMethod]
        public void _5_OpenForm_with_data_after_orch_call()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OrchestrationMethodName                          = "GetInfo",
                OpenFormWithResourceCodeDataParameterBindingPath = "p.t"
            };

            OutputShouldBe(@"

this.internalProxyDidRespondCallback = () =>
{
    const data:any = this.state.windowRequest.p.t;

    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", data, showAsNewPage,menuItemSuffix);
}

this.executeWindowRequest(""GetInfo"");
");

            ChangeToTabPage();

            OutputShouldBe(@"

this.state.pageInstance.internalProxyDidRespondCallback = () =>
{
    const data:any = this.state.windowRequest.p.t;

    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", data, showAsNewPage,menuItemSuffix);
}

this.state.pageInstance.executeWindowRequest(""GetInfo"");
");
        }

        [TestMethod]
        public void _6_open_form_in_dialogBox_with_data_after_orch_call()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OrchestrationMethodName                          = "GetInfo",
                OpenFormWithResourceCodeDataParameterBindingPath = "p.t",
                OpenFormWithResourceCodeIsInDialogBox            = true
            };

            OutputShouldBe(@"
this.internalProxyDidRespondCallback = () =>
{
    const data:any = this.state.windowRequest.p.t;

    const onClose: any = null;

    const style:any = ;

    BFormManager.showDialog(""x"", data, /*title*/null, onClose, style );
}

this.executeWindowRequest(""GetInfo"");

");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.internalProxyDidRespondCallback = () =>
{
    const data:any = this.state.windowRequest.p.t;

    const onClose: any = null;

    const style:any = ;

    BFormManager.showDialog(""x"", data, /*title*/null, onClose, style );
}

this.state.pageInstance.executeWindowRequest(""GetInfo"");

");
        }

        [TestMethod]
        public void _7_open_form_in_dialogBox_with_data_after_orch_call_then_recall_if_success()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OrchestrationMethodName                          = "GetInfo",
                OpenFormWithResourceCodeDataParameterBindingPath = "p.t",
                OpenFormWithResourceCodeIsInDialogBox            = true,
                OrchestrationMethodOnDialogResponseIsOK          = "pp"
            };

            OutputShouldBe(@"
this.internalProxyDidRespondCallback = () =>
{
    const data:any = this.state.windowRequest.p.t;

    const onClose: any = (dialogResponse:number) =>
    {
        if(dialogResponse === 1)
        {
            this.executeWindowRequest(""pp"");
        }
    };

    const style:any = ;

    BFormManager.showDialog(""x"", data, /*title*/null, onClose, style );
}

this.executeWindowRequest(""GetInfo"");


");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.internalProxyDidRespondCallback = () =>
{
    const data:any = this.state.windowRequest.p.t;

    const onClose: any = (dialogResponse:number) =>
    {
        if(dialogResponse === 1)
        {
            this.state.pageInstance.executeWindowRequest(""pp"");
        }
    };

    const style:any = ;

    BFormManager.showDialog(""x"", data, /*title*/null, onClose, style );
}

this.state.pageInstance.executeWindowRequest(""GetInfo"");


");
        }

        [TestInitialize]
        public void ZTestInitialize()
        {
            api = new ButtonActionInfoFunction
            {
                WriterContext = new WriterContext
                {
                    ExecuteWindowRequestFunctionAccessPath = "this.executeWindowRequest"
                }
            };
        }
        #endregion

        #region Methods
        void ChangeToTabPage()
        {
            api.WriterContext.IsTabPage                              = true;
            api.WriterContext.ExecuteWindowRequestFunctionAccessPath = "this.state.pageInstance.executeWindowRequest";
        }

        void OutputShouldBe(string expected)
        {
            var code = api.GetCode().Trim().Replace(" ", "");
            expected = expected.Trim().Replace(" ", "");

            code.Should().BeEquivalentTo(expected);
        }
        #endregion
    }
}