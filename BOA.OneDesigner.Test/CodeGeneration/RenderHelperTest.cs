using System.Collections.Generic;
using BOA.OneDesigner.CodeGenerationHelper;
using BOA.OneDesigner.CodeGenerationModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.CodeGeneration
{
    [TestClass]
    public class RenderHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void Long_binding_paths_should_be_shorten_in_variables_0()
        {
            var variables = new List<string>();

            var jsBindingPath = new JsBindingPathCalculatorData
            {
                RenderMethodRequestRelatedVariables = variables,
                BindingPathInCSharpInDesigner       = "dataContract.UserName",
                EvaluateInsStateVersion             = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            jsBindingPath.BindingPathInJs.Should().Be("dataContract.userName");

            variables.Should().BeEquivalentTo("const dataContract = request.dataContract || {};",
                                              "const dataContractInState = requestInState.dataContract || {};");
        }

        [TestMethod]
        public void Long_binding_paths_should_be_shorten_in_variables_1()
        {
            var variables = new List<string>();

            var jsBindingPath = new JsBindingPathCalculatorData
            {
                RenderMethodRequestRelatedVariables = variables,
                BindingPathInCSharpInDesigner       = "dataContract.User.Info.UserName",
                EvaluateInsStateVersion             = true
            };
            JsBindingPathCalculator.CalculateBindingPathInRenderMethod(jsBindingPath);

            jsBindingPath.BindingPathInJs.Should().Be("info.userName");

            variables.Should().BeEquivalentTo("const dataContract = request.dataContract || {};",
                                              "const dataContractInState = requestInState.dataContract || {};",
                                              "const user = dataContract.user || {};",
                                              "const userInState = dataContractInState.user || {};",
                                              "const info = user.info || {};",
                                              "const infoInState = userInState.info || {};");
        }

        [TestMethod]
        public void TransformBindingPathInJsToStateAccessedVersion()
        {
            RenderHelper.TransformBindingPathInJsToStateAccessedVersion("request.userName")
                        .Should()
                        .Be("requestInState.userName");

            RenderHelper.TransformBindingPathInJsToStateAccessedVersion("x.y")
                        .Should()
                        .Be("xInState.y");
        }
        #endregion
    }
}