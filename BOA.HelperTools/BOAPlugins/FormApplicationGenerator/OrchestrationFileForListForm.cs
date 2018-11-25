using System;
using System.Linq;

namespace BOAPlugins.FormApplicationGenerator
{
    static class OrchestrationFileForListForm
    {
        #region Public Methods
        public static string GenerateCode(Model Model)
        {
            var resultGridColumnNames = string.Join("," + Environment.NewLine, Model.FormDataClassFields.Select(fieldInfo => $"nameof({Model.DefinitionFormDataClassName}.{fieldInfo.Name})"));

            return @"
using System.Collections.Generic;
using BOA.Base;
using BOA.Common.Types;
using BOA.Common.Helpers;
using " + Model.NamespaceNameForType + @";

namespace " + Model.NamespaceNameForOrchestration + @"
{   
    public class " + Model.FormName + @"ListForm
    {
        /// <summary>
        ///     Evaluates the initial state.
        /// </summary>
        public GenericResponse<" + Model.RequestNameForList + @"> EvaluateInitialState(" + Model.RequestNameForList + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeResponse(request);
            var data         = request.Data;
            var dataSource   = request.DataSource;
            var state        = request.State;


            dataSource.DataGridInfo = GetDataGridInfo();
            dataSource.Records      = new List<" + Model.DefinitionFormDataClassName + @">();

            return returnObject;
        }

        /// <summary>
        ///     Gets the information.
        /// </summary>
        public GenericResponse<" + Model.RequestNameForList + @"> GetInfo(" + Model.RequestNameForList + @" request, ObjectHelper objectHelper)
        {
            var returnObject = objectHelper.InitializeResponse(request);
            var data         = request.Data;
            var dataSource   = request.DataSource;
            var state        = request.State;

            #region TODO: Expects code
            dataSource.Records = RandomValue.ListOf<" + Model.DefinitionFormDataClassName + @">();
            #endregion

            state.StatusMessage = dataSource.Records.Count + Message.RecordsWereBrought;

            return returnObject;
        }

        /// <summary>
        ///     Gets the data grid information.
        /// </summary>
        static DataGridInfo GetDataGridInfo()
        {
            return DataGridInfo.Create(typeof(" + Model.DefinitionFormDataClassName + @"), new[]
            {
                " + resultGridColumnNames + @"
            });
        }        
    }
}
";
        }
        #endregion
    }
}