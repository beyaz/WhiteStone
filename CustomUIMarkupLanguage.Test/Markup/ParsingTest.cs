using System;
using CustomUIMarkupLanguage.Markup;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomUIMarkupLanguage.Test.Markup
{
    [TestClass]
    public class ParsingTest
    {


        [TestMethod]
        public  void Parse_ViewInvocationExpressionInfo()
        {

            var info = ViewInvocationExpressionInfo.Parse("this.AAA(5,'uy kğş',7)");
            Assert.AreEqual("AAA", info.MethodName);
            Assert.IsTrue(info.IsStartsWithThis);
            Assert.AreEqual(3M, info.Parameters.Count);
            Assert.AreEqual(5M, Convert.ToDecimal(info.Parameters[0]));
            Assert.AreEqual("uy kğş", info.Parameters[1] as string);
            Assert.AreEqual(7M, Convert.ToDecimal(info.Parameters[2]));


            info = ViewInvocationExpressionInfo.Parse("AAA");
            Assert.AreEqual("AAA",info.MethodName);

            info = ViewInvocationExpressionInfo.Parse("this.AAA()");
            Assert.AreEqual("AAA", info.MethodName);
            Assert.IsTrue(info.IsStartsWithThis);

            info = ViewInvocationExpressionInfo.Parse("this.AAA(5)");
            Assert.AreEqual("AAA", info.MethodName);
            Assert.IsTrue(info.IsStartsWithThis);
            Assert.AreEqual(1M, info.Parameters.Count);
            Assert.AreEqual(5M, Convert.ToDecimal(info.Parameters[0]));


            


        }

        #region Public Methods
        [TestMethod]
        public void Should_Transform_Array_Values()
        {
            const string ui = @"
        {
            view:'A',
            Rows:[
                {R:'g',y:6.8},
                {R2:'{Binding  A.y,Mode=OneWay}',yt:6.9}
            ]
        }";

            var node = TransformHelper.Transform(ui);

            Assert.AreEqual("view", node.Properties[0].Name);

            Assert.AreEqual("A", node.Properties[0].ValueAsString);

            Assert.IsTrue(node.Properties[0].ValueIsString);

            Assert.AreEqual("Rows", node.Properties[1].Name);

            Assert.IsTrue(node.Properties[1].ValueIsArray);

            var prop1 = node.Properties[1].ValueAsArray[0];

            Assert.IsNotNull(prop1);

            Assert.AreEqual("R", prop1.Properties[0].Name);

            Assert.AreEqual("g", prop1.Properties[0].ValueAsString);

            Assert.AreEqual("y", prop1.Properties[1].Name);

            Assert.AreEqual(6.8M, prop1.Properties[1].ValueAsNumber);

            var prop2 = node.Properties[1].ValueAsArray[1];
            Assert.IsNotNull(prop2);

            TransformHelper.TransformBindingInformation(prop2);

            Assert.AreEqual("R2", prop2.Properties[0].Name);

            Assert.AreEqual("A.y", prop2.Properties[0].ValueAsBindingInfo.SourcePath);
        }

        [TestMethod]
        public void Should_Transform_Primitive_Attribute_Values_Boolean()
        {
            const string ui = "{view:true}";

            var node = TransformHelper.Transform(ui);

            Assert.AreEqual("view", node.Properties[0].Name);

            Assert.AreEqual(true, node.Properties[0].ValueAsBoolean);

            Assert.IsTrue(node.Properties[0].ValueIsBoolean);
        }

        [TestMethod]
        public void Should_Transform_Primitive_Attribute_Values_Number()
        {
            const string ui = "{view:56.7}";

            var node = TransformHelper.Transform(ui);

            Assert.AreEqual("view", node.Properties[0].Name);

            Assert.AreEqual(56.7M, node.Properties[0].ValueAsNumber);

            Assert.IsTrue(node.Properties[0].ValueIsNumber);
        }

        [TestMethod]
        public void Should_Transform_Primitive_Attribute_Values_String()
        {
            const string ui = "{view:'A'}";

            var node = TransformHelper.Transform(ui);

            Assert.AreEqual("view", node.Properties[0].Name);

            Assert.AreEqual("A", node.Properties[0].ValueAsString);

            Assert.IsTrue(node.Properties[0].ValueIsString);
        }
        #endregion
    }
}