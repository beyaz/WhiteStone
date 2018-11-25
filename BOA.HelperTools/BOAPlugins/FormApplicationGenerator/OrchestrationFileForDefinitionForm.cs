namespace BOAPlugins.FormApplicationGenerator
{
    static class OrchestrationFileForDefinitionForm
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            return @"

using BOA.Base;
using BOA.Common.Types;
using BOA.Common.Helpers;
using " + Model.NamespaceNameForType + @";

namespace " + Model.NamespaceNameForOrchestration + @"
{    
    public class " + Model.FormName + @"Form
    {
        #region Public Methods
        /// <summary>
        ///     Ends the of workflow.
        /// </summary>
        public WorkflowResponse<" + Model.RequestNameForDefinition + @"> EndOfWorkflow(" + Model.RequestNameForDefinition + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeWorkflowResponse(request);

            // TODO: Expects code. ( workflow işleminin en sonunda yapılması gereken işlem burada yazılabilir)

            return returnObject;
        }

        /// <summary>
        ///     Loads the data.
        /// </summary>
        public WorkflowResponse<" + Model.RequestNameForDefinition + @"> LoadData(" + Model.RequestNameForDefinition + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeWorkflowResponse(request);
                 
            if (request.WorkFlowInternalData.InstanceId > 0)
            {
                return returnObject;
            }

            #region TODO: Expects code
            //      default atanması gereken form değerleri 
            //      enable readonly gibi state işlemleri
            //      data source ların dolsurulması mesela özel bir combonun datasource bilgisi
            //      gibi bilgiler burada doldurulmalıdır.

            request.Data = RandomValue.Object<" + Model.DefinitionFormDataClassName + @">();
            request.MethodName = nameof(EndOfWorkflow);
            #endregion
        
            return returnObject;            

        }

        /// <summary>
        ///     Saves the specified request.
        /// </summary>
        public WorkflowResponse<" + Model.RequestNameForDefinition + @"> Save(" + Model.RequestNameForDefinition + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeWorkflowResponse(request);

            #region TODO: Expects code
            // ?
            #endregion

            request.StatusMessage = Message.TransactionSaved;

            return returnObject;
        }

        
        #endregion
    }
}


";
        }
        #endregion
    }
}