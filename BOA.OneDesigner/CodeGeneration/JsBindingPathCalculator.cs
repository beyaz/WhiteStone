using System;
using System.Collections.Generic;
using System.Linq;
using BOA.Common.Helpers;
using BOA.OneDesigner.CodeGenerationModel;
using BOA.OneDesigner.Helpers;
using BOA.OneDesigner.JsxElementModel;
using BOAPlugins.Utility;

namespace BOA.OneDesigner.CodeGeneration
{
    class JsBindingPathCalculatorData
    {
        #region Constructors
        public JsBindingPathCalculatorData()
        {
        }

        public JsBindingPathCalculatorData(WriterContext writerContext, string bindingPathInCSharpInDesigner)
        {
            RenderMethodRequestRelatedVariables = writerContext.RenderMethodRequestRelatedVariables;
            BindingPathInCSharpInDesigner       = bindingPathInCSharpInDesigner;
        }
        #endregion

        #region Public Properties
        public string       BindingPathInCSharpInDesigner       { get; set; }
        public bool         EvaluateInsStateVersion             { get; set; }
        public List<string> RenderMethodRequestRelatedVariables { get; set; }
        #endregion

        #region Properties
        internal string BindingPathInJs        { get; set; }
        internal string BindingPathInJsInState { get; set; }
        #endregion
    }

    static class JsBindingPathCalculator
    {
        #region Public Methods
        public static void CalculateBindingPathInRenderMethod(JsBindingPathCalculatorData data)
        {
            var variables = data.RenderMethodRequestRelatedVariables;

            var bindingPathInJs = TypescriptNaming.NormalizeBindingPath(Config.BindingPrefixInCSharp + data.BindingPathInCSharpInDesigner);

            var list = bindingPathInJs.SplitAndClear(".");

            if (list.Count < 2)
            {
                throw Error.InvalidOperation();
            }

            if (list.Count == 2)
            {
                

                data.BindingPathInJs = bindingPathInJs;

                if (data.EvaluateInsStateVersion)
                {
                    var paths = bindingPathInJs.Trim().Split('.');

                    paths[0] = paths[0] + "InState";

                    data.BindingPathInJsInState = string.Join(".", paths);
                }

                return;
            }

            var len = list.Count - 2;

            for (var i = 0; i < len; i++)
            {
                var assignments        = new string[2];
                var assignmentsInState = new string[2];

                Array.Copy(list.ToArray(), i, assignments, 0, 2);
                Array.Copy(list.ToArray(), i, assignmentsInState, 0, 2);

                assignmentsInState[0] = assignmentsInState[0] + "InState";

                var assignmentValue        = string.Join(".", assignments);
                var assignmentValueInState = string.Join(".", assignmentsInState);

                var variable       = $"const {list[i + 1]} = {assignmentValue} || {{}};";
                var variableIsSate = $"const {list[i + 1]}InState = {assignmentValueInState} || {{}};";

                if (variables.Contains(variable) == false)
                {
                    variables.Add(variable);
                }

                if (data.EvaluateInsStateVersion)
                {
                    if (variables.Contains(variableIsSate) == false)
                    {
                        variables.Add(variableIsSate);
                    }
                }
            }

            data.BindingPathInJs = string.Join(".", list.Reverse().Take(2).Reverse());
            if (data.EvaluateInsStateVersion)
            {
                var array = list.Reverse().Take(2).Reverse().ToArray();

                array[0] = array[0] + "InState";

                data.BindingPathInJsInState = string.Join(".", array);
            }
        }
        #endregion
    }
}