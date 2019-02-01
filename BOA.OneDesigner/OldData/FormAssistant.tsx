import { BrowsePage } from "b-framework"
import { BFormManager } from "b-form-manager"
import RequestBase = BOA.Common.Types.RequestBase;
import DataGridInfo = BOA.Types.CardGeneral.DebitCard.DataGridInfo;
import DataGridColumnInfo = BOA.Types.CardGeneral.DebitCard.DataGridColumnInfo;

/**
 *  Manages window request of form.
 * Sends form request to server and receive and maps incoming request to form request.
 */
export class FormAssistant
{
    /**
      *  Initializes assistant specific fields.
      * @param form
      * @param windowRequestFullName
      */
    static initialize(form: BFramework.BasePage, windowRequestFullName: string)
    {
        form.state.$windowRequestFullName = windowRequestFullName;
        FormAssistant.setDataContractAccessPathInWindowRequest(form, "data");
    }

    /**
      *  Sets the data contract access path in window request.
      * @param form
      * @param dataContractAccessPathInWindowRequest
      */
    static setDataContractAccessPathInWindowRequest(form: BFramework.BasePage, dataContractAccessPathInWindowRequest: string)
    {
        form.state.$dataContractAccessPathInWindowRequest = dataContractAccessPathInWindowRequest;
    }

    /**
      * if response is success then updates WindowRequest of form
      * otherwise
      * shows the error message in response.result
      */
    static receiveResponse(form: BFramework.BasePage,proxyResponse: ProxyResponse)
    {
        let isSuccess: boolean = true;

        const key = proxyResponse.key;
        const response = proxyResponse.response;

        if (!response.success)
        {
            isSuccess = false;

            const results = response.results;
            if (results == null)
            {
                form.showStatusMessage(ErrorMessage.DeveloperErrorTypeNotFoundInBOAOneTypeFolder);
                return false;
            }

            const businessResult = results.find(r => r.severity === BOA.Common.Types.Severity.BusinessError);
            if (businessResult != undefined)
            {
                form.showStatusMessage(businessResult.errorMessage);
            }
            else
            {
                BFormManager.showStatusErrorMessage(ErrorMessage.UnexpectedErrorOccured + JSON.stringify(results, null, 2), []);
            }
        }

        // handle incoming windowRequest
        const windowRequest: IFormRequest = (response as any).value;

        if (windowRequest == null)
        {
            throw new Error(`Orch method:${key} must return windowRequest. @windowRequestFullName:${form.state.$windowRequestFullName}`);
        }

        const currentWindowRequestInForm: IFormRequest = FormAssistant.getWindowRequest(form) as IFormRequest;

        if (currentWindowRequestInForm && currentWindowRequestInForm.dataSource)
        {
            // update data source with incoming data source
            FormAssistant.copyProperties(windowRequest.dataSource, currentWindowRequestInForm.dataSource);

            windowRequest.dataSource = currentWindowRequestInForm.dataSource;
        }

        if (FormAssistant.hasWorkflow(windowRequest))
        {
            (windowRequest as any).hasWorkflow = (currentWindowRequestInForm as any).hasWorkflow;
            if (windowRequest.methodName === key)
            {
                windowRequest.methodName = currentWindowRequestInForm.methodName;
            }
        }

        const state: any =
        {
            windowRequest: windowRequest,
            $isInitialStateEvaluated: true,
            $isWaitingResponse:false
        };

        if (form instanceof BrowsePage)
        {
            const listFormDataSource: IListFormDataSource = windowRequest.dataSource;

            if (listFormDataSource.dataGridRecords == null)
            {
                console.log(ErrorMessage.RecordPropertyNotFoundInFormDataSourceInstance);
            }

            const dataGridInfo = listFormDataSource.dataGridInfo;

            if (dataGridInfo == null)
            {
                throw new Error(ErrorMessage.DataGridInfoPropertyValueCanNotBeEmpty);
            }

            // init browse page specific fields
            state.dataSource = listFormDataSource.dataGridRecords || [];
            state.columns = DataGridHelper.convertToDataGridColumn(dataGridInfo.columns);
        }
        
        if ((windowRequest as any).statusMessage)
        {
            BFormManager.showStatusMessage((windowRequest as any).statusMessage);
            (windowRequest as any).statusMessage = null;
        }


        form.setWindowRequest({
            body: windowRequest,
            type: form.state.$windowRequestFullName
        });

        form.setState(state);

        if (form.constructor.prototype.evaluateActionStates)
        {
            (form as any).evaluateActionStates();
        }

        if (isSuccess)
        {
            const executeWorkFlow = (form as any).executeWorkFlow;
            if (executeWorkFlow)
            {
                executeWorkFlow();
            }
        }

        return isSuccess;
    }


    /**
      * Sends window request to orchestration
      */
    static executeWindowRequest(form: BFramework.BasePage,orchestrationMethodName: string)
    {
        const windowRequest: IFormRequest = FormAssistant.getWindowRequest(form) as IFormRequest;
        if (windowRequest == null)
        {
            throw new Error(ErrorMessage.WindowRequestMustBeInitializeBeforeCallingSendWindowRequest);
        }

        if (orchestrationMethodName == null)
        {
            throw new Error(ErrorMessage.ArgumentOrchestrationMethodNameIsNull);
        }

        // form should be re render because form component value changes must be handled
        FormAssistant.updateState(form,() =>
        {
            // Note: Object.assign is not makes any deep copy.
            const clonedWindowRequest = Object.assign({}, windowRequest);

            if (FormAssistant.hasWorkflow(clonedWindowRequest))
            {
                (clonedWindowRequest as any).hasWorkflow = false;
            }

            // Note:null heavy data because no need to post to server side.
            clonedWindowRequest.dataSource = {};

            FormAssistant.sendWindowRequestToServer(form,clonedWindowRequest, orchestrationMethodName);
        });
    }

    /**
      * evaluates initial states if required.
      */
    static componentDidMount(form: BFramework.BasePage)
    {
        if (form.state.$isInitialStateEvaluated)
        {
            return;
        }
        
        if (!FormAssistant.isReadyToRender(form))
        {
            FormAssistant.loadData(form);
        }
    }

    /**
      * Gets the window request of form.
      */
    static getWindowRequest(page: BFramework.BasePage):RequestBase
    {
        const windowRequest = page.getWindowRequest();

        if (windowRequest && windowRequest.body)
        {
            return windowRequest.body as RequestBase;
        }

        if (page.state && page.state.windowRequest)
        {
            return page.state.windowRequest as RequestBase;
        }

        return null as RequestBase;
    }

    /**
      *  Returns true if form is ready to render.
      */
    static isReadyToRender(form: BFramework.BasePage): boolean
    {
        if (form.state.$isWaitingResponse)
        {
            return false;
        }
        if (form.state.$isInitialStateEvaluated)
        {
            return true;
        }

        return false;
    }

    /**
      * Sets the action state of form.
      * Example: this.assistant.setActionState(CommandName.Save, state.isNewActionEnabled);
      */
    static setActionState(form: BFramework.BasePage,commandName: string, isEnabled: boolean)
    {
        if (isEnabled)
        {
            form.enableAction(commandName);
            return;
        }

        form.disableAction(commandName);
    }

    /**
      *  Clones window request object and sets states with this new cloned window request object.
      */
    static updateState(form: BFramework.BasePage,callback?: () => void)
    {
        const windowRequest = FormAssistant.getWindowRequest(form);

        // Note: Object.assign is not makes any deep copy.
        const clonedWindowRequest = Object.assign({}, windowRequest);

        form.setState({ windowRequest: clonedWindowRequest }, callback);
    }

    /**
      *  Returns true if request has workflow.
      */
    private static  hasWorkflow(windowRequest:any): boolean
    {
        if (windowRequest && windowRequest.workFlowInternalData && windowRequest.workFlowInternalData.instanceId > 0)
        {
            return true;
        }

        return false;
    }

    /**
      *  Evaluates initial states of form.
      *  Invokes 'LoadData' metod in Orchestration class.
      */
    private static loadData(form: BFramework.BasePage)
    {
        const windowRequest = FormAssistant.getWindowRequest(form);

        // Note: Object.assign is not makes any deep copy.
        const clonedWindowRequest:any = Object.assign({}, windowRequest);

        if (FormAssistant.hasWorkflow(clonedWindowRequest))
        {
            clonedWindowRequest.hasWorkflow = false;
            FormAssistant.sendWindowRequestToServer(form,clonedWindowRequest as RequestBase, OrchestrationMethodName.LoadData);
            return;
        }

        const formData = form.state.pageParams.data;

        if (formData != null)
        {
            clonedWindowRequest[form.state.$dataContractAccessPathInWindowRequest as  string] = formData;
        }

        FormAssistant.sendWindowRequestToServer(form,clonedWindowRequest as RequestBase, OrchestrationMethodName.LoadData);
    }

    private static copyProperties(source: any, target: any): any
    {
        return jQuery.extend(target, source);
    }

    private static  sendWindowRequestToServer(form: BFramework.BasePage,windowRequest: RequestBase, orchestrationMethodName: string)
    {
        form.state.$isWaitingResponse = true;

        windowRequest.methodName = orchestrationMethodName;
        const proxyRequest: any =
        {
            requestClass: form.state.$windowRequestFullName,
            key: orchestrationMethodName,
            requestBody: windowRequest,
            showProgress: false
        };

        if (form instanceof BrowsePage)
        {
            form.proxyExecute(proxyRequest);
        }
        else
        {
            form.proxyTransactionExecute(proxyRequest);
        }
    }

    /**
      * Converts given dataGridColumnInfoContracts values to data grid column definition.
      */
    static convertToDataGridColumns(dataGridColumnInfoContracts: DataGridColumnInfo[]): any[]
    {
        return DataGridHelper.convertToDataGridColumn(dataGridColumnInfoContracts);
    }
}

/**
 * Utility methods for DataGrid component
 */
class DataGridHelper
{
    static convertToDataGridColumn(dataGridColumnInfoContracts: DataGridColumnInfo[]): any[]
    {
        if (dataGridColumnInfoContracts == null)
        {
            return null;
        }

        var columns: any[] = [];

        dataGridColumnInfoContracts.forEach((info: DataGridColumnInfo) =>
        {
            var key = this.formatKey(info.bindingPath);
            var name = info.label;

            var column: any =
            {
                key: key,
                name: name
            };

            if (info.isInt32)
            {
                column.type = DataGridColumnType.Number;
            }
            else if (info.isDecimal)
            {
                column.type = DataGridColumnType.Number;
                column.numberFormat = DataGridColumnNumberFormat.Decimal;
            }
            else if (info.isDate)
            {
                column.type = DataGridColumnType.Date;
            }

            columns.push(column);
        });

        return columns;
    }

    private static formatKey(bindingPath: string): string
    {
        const Dot = ".";

        var arr = bindingPath.split(Dot);

        arr.forEach((item, index) =>
        {
            arr[index] = item[0].toLowerCase() + item.substring(1);
        });

        return arr.join(Dot);
    }
}

enum DataGridColumnDataType
{
    Int32 = 0,
    String = 1,
    Date = 2,
    Decimal = 3,
    Boolean = 4
}

interface IFormRequest extends RequestBase
{
    dataSource: any;
    data: any;
}

interface IListFormDataSource
{
    dataGridInfo?: DataGridInfo;
    dataGridRecords: any[];
}

class OrchestrationMethodName
{
    static readonly LoadData = "LoadData";
}

class DataGridColumnType
{
    static readonly Number = "number";
    static readonly Date = "date";
}

class DataGridColumnNumberFormat
{
    static readonly Decimal = "M";
}

class ErrorMessage
{
    static readonly WindowRequestMustBeInitializeBeforeCallingSendWindowRequest = "windowRequest must be initialize before calling 'sendWindowRequest'.";
    static readonly ArgumentOrchestrationMethodNameIsNull = "Argument 'orchestrationMethodName' is null.";
    static readonly DeveloperErrorTypeNotFoundInBOAOneTypeFolder = "DeveloperError: Genellikle type dll'inin 'd:\boa\one\' folderında olmamasından kaynaklı olabilir. Build eventsleri check edin.Check request headers from Developer Tools -> Network tab.";
    static readonly UnexpectedErrorOccured = "Beklenmedik bir hata oluştu.";
    static readonly RecordPropertyNotFoundInFormDataSourceInstance = "'records' property value not found in '...FormDataSource' instance.";
    static readonly DataGridInfoPropertyValueCanNotBeEmpty = "'dataGridInfo' property value can not be empty.";
}