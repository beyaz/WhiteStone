using BOA.OneDesigner.CodeGenerationModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data = BOA.OneDesigner.JsxElementModel.ButtonActionInfo;


    //orch call
    // condition
    //soru sorma
    //orch call

    // condition
    //ekran açma
    //rresource code
    //title
    //data

    //dialog
    //rresource code
    //title
    //data
    //orch call on Close On Ok

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

this.addToProcessQueue(() =>
{
    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", /*data*/null, showAsNewPage,menuItemSuffix);
});

this.executeWindowRequest(""GetInfo"");
");

            ChangeToTabPage();

            OutputShouldBe(@"

this.state.pageInstance.addToProcessQueue(() =>
{
    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", /*data*/null, showAsNewPage,menuItemSuffix);
});

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

this.addToProcessQueue( () =>
{
    const data:any = this.state.windowRequest.p.t;

    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", data, showAsNewPage,menuItemSuffix);
});

this.executeWindowRequest(""GetInfo"");
");

            ChangeToTabPage();

            OutputShouldBe(@"

this.state.pageInstance.addToProcessQueue( () =>
{
    const data:any = this.state.windowRequest.p.t;

    const showAsNewPage:boolean = true;

    const menuItemSuffix:string = null;

    BFormManager.show(""x"", data, showAsNewPage,menuItemSuffix);
});

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
this.addToProcessQueue( () =>
{
    const data:any = this.state.windowRequest.p.t;

    const style:any = ;

    BFormManager.showDialog(""x"", data, /*title*/null, /*onClose*/null, style );
});

this.executeWindowRequest(""GetInfo"");

");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.addToProcessQueue( () =>
{
    const data:any = this.state.windowRequest.p.t;

    const style:any = ;

    BFormManager.showDialog(""x"", data, /*title*/null, /*onClose*/null, style );
});

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
                OpenFormWithResourceCodeIsInDialogBox            = true,
                OrchestrationMethodOnDialogResponseIsOK          = "pp",
                CssOfDialog                                      = "{h}",
                Title                                            = "'Aloha'"
            };

            OutputShouldBe(@"
this.addToProcessQueue( () =>
{
    const onClose: any = (dialogResponse: number) =>
    {
        if(dialogResponse === 1)
        {
            this.executeWindowRequest(""pp"");
        }
    };

    const style:any = {h};

    const title = 'Aloha';

    BFormManager.showDialog(""x"", /*data*/null, title, onClose, style );
});

this.executeWindowRequest(""GetInfo"");


");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.addToProcessQueue( () =>
{
    const onClose: any = (dialogResponse: number) =>
    {
        if(dialogResponse === 1)
        {
            this.state.pageInstance.executeWindowRequest(""pp"");
        }
    };

    const style:any = {h};
    
    const title = 'Aloha';

    BFormManager.showDialog(""x"", /*data*/null, title, onClose, style );
});

this.state.pageInstance.executeWindowRequest(""GetInfo"");


");
        }

        [TestMethod]
        public void _8_open_form_in_dialogBox_with_data_after_orch_call_then_recall_if_success()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OrchestrationMethodName                          = "GetInfo",
                OpenFormWithResourceCodeDataParameterBindingPath = "p.t",
                OpenFormWithResourceCodeIsInDialogBox            = true,
                OrchestrationMethodOnDialogResponseIsOK          = "pp",
                CssOfDialog = "{h}",
                Title = "'Aloha'"
            };

            OutputShouldBe(@"
this.addToProcessQueue( () =>
{
    const data:any = this.state.windowRequest.p.t;

    const onClose: any = (dialogResponse: number, returnValue: any) =>
    {
        if(dialogResponse === 1)
        {
            this.state.windowRequest.p.t = returnValue;
            this.executeWindowRequest(""pp"");
        }
    };

    const style:any = {h};

    const title = 'Aloha';

    BFormManager.showDialog(""x"", data, title, onClose, style );
});

this.executeWindowRequest(""GetInfo"");


");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.addToProcessQueue( () =>
{
    const data:any = this.state.windowRequest.p.t;

    const onClose: any = (dialogResponse: number, returnValue: any) =>
    {
        if(dialogResponse === 1)
        {
            this.state.windowRequest.p.t = returnValue;
            this.state.pageInstance.executeWindowRequest(""pp"");
        }
    };

    const style:any = {h};
    
    const title = 'Aloha';

    BFormManager.showDialog(""x"", data, title, onClose, style );
});

this.state.pageInstance.executeWindowRequest(""GetInfo"");


");
        }


        
        // [TestMethod]
        public void _9_open_form_in_dialogBox_with_data_after_orch_call_then_recall_if_success()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OrchestrationMethodName                          = "GetInfo",
                OpenFormWithResourceCodeDataParameterBindingPath = "p.t",
                OpenFormWithResourceCodeIsInDialogBox            = true,
                OrchestrationMethodOnDialogResponseIsOK          = "pp",
                CssOfDialog                                      = "{h}",
                Title                                            = "'Aloha'",
                YesNoQuestion = "MMM",
                
            };

            OutputShouldBe(@"
this.addToProcessQueue( () =>
{
    BDialogHelper.show(form.state.context,""MMM"", DialogType.QUESTION, DialogResponseStyle.YESNO, ""Soru"",
        (dialogResponse: any) =>
        {
            if (dialogResponse === 1)
            {
                 const data:any = this.state.windowRequest.p.t;

                const onClose: any = (dialogResponse: number, returnValue: any) =>
                {
                    if(dialogResponse === 1)
                    {
                        this.state.windowRequest.p.t = returnValue;
                        this.executeWindowRequest(""pp"");
                    }
                };

                const style:any = {h};

                const title = 'Aloha';

                BFormManager.showDialog(""x"", data, title, onClose, style );
            }
        }
    );

   
});

this.executeWindowRequest(""GetInfo"");


");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.addToProcessQueue( () =>
{
    BDialogHelper.show(form.state.context,""MMM"", DialogType.QUESTION, DialogResponseStyle.YESNO, ""Soru"",
        (dialogResponse: any) =>
        {
            if (dialogResponse === 1)
            {
                const data:any = this.state.windowRequest.p.t;

                const onClose: any = (dialogResponse: number, returnValue: any) =>
                {
                    if(dialogResponse === 1)
                    {
                        this.state.windowRequest.p.t = returnValue;
                        this.state.pageInstance.executeWindowRequest(""pp"");
                    }
                };

                const style:any = {h};
                
                const title = 'Aloha';

                BFormManager.showDialog(""x"", data, title, onClose, style );
            }
        }
    );    
});

this.state.pageInstance.executeWindowRequest(""GetInfo"");


");
        }



        

        
        // todo:t [TestMethod]
        public void _10_open_form_in_dialogBox_with_data_after_orch_call_then_recall_if_success()
        {
            api.Data = new Data
            {
                OpenFormWithResourceCode                         = "x",
                OrchestrationMethodName                          = "GetInfo",
                OpenFormWithResourceCodeDataParameterBindingPath = "p.t",
                OpenFormWithResourceCodeIsInDialogBox            = true,
                OrchestrationMethodOnDialogResponseIsOK          = "pp",
                CssOfDialog                                      = "{h}",
                Title                                            = "'Aloha'",
                YesNoQuestion = "MMM",
                YesNoQuestionAfterYesOrchestrationCall = "call3"
                
            };

            OutputShouldBe(@"


this.addToProcessQueue( () =>
{
    BDialogHelper.show(form.state.context,""MMM"", DialogType.QUESTION, DialogResponseStyle.YESNO, ""Soru"",
        (dialogResponse: any) =>
        {
            if (dialogResponse === 1)
            {
				this.addToProcessQueue( () =>
				{
					const data:any = this.state.windowRequest.p.t;

					const onClose: any = (dialogResponse: number, returnValue: any) =>
					{
						if(dialogResponse === 1)
						{
							this.state.windowRequest.p.t = returnValue;
							this.executeWindowRequest(""pp"");
						}
					};

					const style:any = {h};

					const title = 'Aloha';

					BFormManager.showDialog(""x"", data, title, onClose, style );
				};
				
				this.executeWindowRequest(""call3"");
                
            }
        }
    );   
});

this.executeWindowRequest(""GetInfo"");


");

            ChangeToTabPage();

            OutputShouldBe(@"
this.state.pageInstance.addToProcessQueue( () =>
{
    BDialogHelper.show(form.state.context,""MMM"", DialogType.QUESTION, DialogResponseStyle.YESNO, ""Soru"",
        (dialogResponse: any) =>
        {
            if (dialogResponse === 1)
            {
                const data:any = this.state.windowRequest.p.t;

                const onClose: any = (dialogResponse: number, returnValue: any) =>
                {
                    if(dialogResponse === 1)
                    {
                        this.state.windowRequest.p.t = returnValue;
                        this.state.pageInstance.executeWindowRequest(""pp"");
                    }
                };

                const style:any = {h};
                
                const title = 'Aloha';

                BFormManager.showDialog(""x"", data, title, onClose, style );
            }
        }
    );    
});

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
            Clear(api.GetCode()).Should().BeEquivalentTo(Clear(expected));
        }

        static string Clear(string value)
        {
            return value.Trim().Replace(" ", "").Replace("\n", "").Replace("\r", "");
        }
        #endregion
    }
}