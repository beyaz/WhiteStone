﻿using System;

namespace BOAPlugins.FormApplicationGenerator
{
    static class DefinitionFormTsxFile
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            return null;

//            var tsxCodeInfo = TsxCodeGeneration.EvaluateTSCodeInfo(Model, true);

//            var tabFormDeclaration = Model.IsTabForm ? Environment.NewLine + "this.tabEnabled = true;" : "";

//            return @"

//import * as React from ""react""
//import { TransactionPage, TransactionPageComposer } from ""b-framework""
//import { BAccountComponent } from ""b-account-component""
//import { BInput } from ""b-input""
//import { BComboBox } from ""b-combo-box""
//import { BCheckBox } from ""b-check-box""
//import { BGridSection } from ""b-grid-section""
//import { BGridRow } from ""b-grid-row""
//import { BInputMask } from ""b-input-mask""
//import { BDateTimePicker } from ""b-datetime-picker""
//import { BBranchComponent } from ""b-branch-component""
//import { BParameterComponent } from ""b-parameter-component""
//import { BInputNumeric } from ""b-input-numeric"";
//import { BCard } from ""b-card""
//import { BCardSection } from ""b-card-section""

//import { Helper } from ""../utils/Helper"";
//import { FormAssistant } from ""../utils/FormAssistant"";
//import { Message } from ""../utils/Message"";
//import { RequestName} from ""../utils/AutoGenerated"";
//import { " + Model.FormName + @"FormCommand as CommandName} from ""../utils/AutoGenerated"";


//import Common = BOA.Common.Types;
//import BasePageProps = BFramework.BasePageProps;
//import " + Model.RequestNameForDefinition + @" = " + Model.NamespaceNameForType + @"." + Model.RequestNameForDefinition + @";

//" + tsxCodeInfo.SnapDefinition + @"

//class " + Model.FormName + @"Form extends TransactionPage
//{
//    executeWorkFlow: () => void;

//    " + tsxCodeInfo.SnapDeclaration + @"

//    constructor(props: BasePageProps)
//    {
//        super(props);

//        this.connect(this);

//        FormAssistant.initialize(this, RequestName." + Model.RequestNameForDefinition + @");
//        " + tabFormDeclaration + @"
//    }

//    evaluateActionStates()
//    {
//        if (this.state.pageParams.data)
//        {
//            this.disableAction(CommandName.New);
//        }
//    }

//    onActionClick(command: Common.ResourceActionContract, executeWorkFlow: () => void)
//    {
//        this.executeWorkFlow = executeWorkFlow;

//        FormAssistant.executeWindowRequest(this,command.commandName);

//        return /*isCompleted*/false;
//    }

//    componentDidMount()
//    {
//        super.componentDidMount();

//        FormAssistant.componentDidMount(this);
//    }

//    proxyDidRespond(proxyResponse: ProxyResponse)
//    {
//        return FormAssistant.receiveResponse(this,proxyResponse);
//    }

//    " + (Model.IsTabForm ? "renderTab" : "render") + @"()
//    {
//        if (!FormAssistant.isReadyToRender(this))
//        {
//            return  " + (Model.IsTabForm ? "[]" : "<div/>") + @";
//        }

//        const context = this.state.context;

//        const windowRequest: " + Model.RequestNameForDefinition + @" = FormAssistant.getWindowRequest(this);

//        const data       = windowRequest.data;
//        const dataSource = windowRequest.dataSource;

//        return " + (Model.IsTabForm ? "[" : "(") + @"
//" + tsxCodeInfo.RenderCodeForJsx + @"
//        " + (Model.IsTabForm ? "]" : ")") + @";
//    }
//}

//export default TransactionPageComposer(" + Model.FormName + @"Form);

//";
        }
        #endregion
    }
}