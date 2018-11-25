﻿namespace BOAPlugins.FormApplicationGenerator
{
    static class ListFormTsxFile
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            var tsxCode = TsxCodeGeneration.EvaluateTSCodeInfo(Model, false);

            return @"

import * as React from ""react""
import { BrowsePage, BrowsePageComposer } from ""b-framework""
import { BAccountComponent } from ""b-account-component""
import { BInput } from ""b-input""
import { BComboBox } from ""b-combo-box""
import { BCheckBox } from ""b-check-box""
import { BGridSection } from ""b-grid-section""
import { BGridRow } from ""b-grid-row""
import { BInputMask } from ""b-input-mask""
import { BDateTimePicker } from ""b-datetime-picker""
import { BBranchComponent } from ""b-branch-component""
import { BParameterComponent } from ""b-parameter-component""
import { BInputNumeric } from ""b-input-numeric"";
import { Helper } from ""../utils/Helper"";
import { ResourceCode } from ""../utils/ResourceCode"";
import { FormAssistant } from ""../utils/FormAssistant"";
import { Message } from ""../utils/Message"";
import { BFormManager } from ""b-form-manager""
import { RequestName} from ""../utils/AutoGenerated"";
import { " + Model.FormName + @"ListFormCommand as CommandName} from ""../utils/AutoGenerated"";

import Common = BOA.Common.Types;
import BasePageProps = BFramework.BasePageProps;
import " + Model.RequestNameForList + @" = " + Model.NamespaceNameForType + @"." + Model.RequestNameForList + @";

" + tsxCode.SnapDefinition + @"

class " + Model.FormName + @"ListForm extends BrowsePage
{
    " + tsxCode.SnapDeclaration + @"

    constructor(props: BasePageProps)
    {
        super(props);

        this.connect(this);

        FormAssistant.initialize(this,RequestName. " + Model.RequestNameForList + @");
    }

    onRowSelectionChanged()
    {
        this.evaluateActionStates();
    }

    evaluateActionStates()
    {
        if (Helper.isOnlyOneRecordSelected(this))
        {
            this.enableAction(CommandName.Open);
        }
        else
        {
            this.disableAction(CommandName.Open);
        }
    }

    onActionClick(command: Common.ResourceActionContract)
    {
        switch (command.commandName)
        {
            case CommandName.Open:
            {
                if (!Helper.isOnlyOneRecordSelected(this))
                {
                    this.showStatusMessage(Message.RecordChoosing);
                    return;
                }

                const data = this.getSelectedRows()[0];

                BFormManager.show(ResourceCode." + Model.FormName + @"Form, data, /*showAsNewPage*/true);

                return;
            }


        }

        FormAssistant.executeWindowRequest(this,command.commandName);
    }

    componentDidMount()
    {
        super.componentDidMount();

        FormAssistant.componentDidMount(this);
    }

    proxyDidRespond(proxyResponse: ProxyResponse)
    {
        return FormAssistant.receiveResponse(this,proxyResponse);
    }

    render()
    {
        if (!FormAssistant.isReadyToRender(this))
        {
            return <div/>;
        }

        const context = this.state.context;

        const windowRequest = this.assistant.getWindowRequest();
        
        const windowRequest: " + Model.RequestNameForList + @" = FormAssistant.getWindowRequest(this);

        const data       = windowRequest.data;
        const dataSource = windowRequest.dataSource;

        return (
            " + tsxCode.RenderCodeForJsx + @"
        );
    }
}

export default BrowsePageComposer(" + Model.FormName + @"ListForm);

";
        }
        #endregion
    }
}