using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Jaml.Markup
{
    [TestClass]
    public class NodeTransformTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_Transform_Array_Values()
        {
            const string ui = @"
{
    view:'A',
    Rows:[
        {R:'g',y:6.8},
        {R:'{Binding  A.y,Mode=OneWay}',y:6.9}
    ]
}";

            var node = TransformHelper.Transform(ui);

            Assert.AreEqual("view", node.Properties[0].Name);

            //            Assert.AreEqual("A", node.Properties[0].Value);

            //            Assert.IsTrue(node.Properties[0].ValueIsString);

            //            Assert.AreEqual("Rows", node[1].Name);

            //            Assert.IsTrue(node.Items[1].ValueIsArray);

            //            node = (AttributeInfoCollection) node.Items[1].Value;

            //            Assert.AreEqual("R", node[0].Name);

            //            Assert.AreEqual("g", node[0].Value);

            //            Assert.AreEqual("y", node[1].Name);

            //            Assert.AreEqual(6.8, node[1].Value);


            //            TransformHelper.TransformBindingInformation(node);

            //            Assert.AreEqual("R", node[1].Name);

            //            Assert.AreEqual("A.y", node[1].ValueAsBindingInfo.SourcePath);

            //        }

            // [TestMethod]
            //public void Should_Transform_Primitive_Attribute_Values_Boolean()
            //{
            //    const string ui = "{view:true}";

            //    var collection = TransformHelper.Transform(ui);

            //    Assert.AreEqual("view", collection.Items[0].Name);

            //    Assert.AreEqual(true, collection.Items[0].Value);

            //    Assert.IsTrue(collection.Items[0].ValueIsBoolean);
            //}

            //[TestMethod]
            //public void Should_Transform_Primitive_Attribute_Values_Number()
            //{
            //    const string ui = "{view:56.7}";

            //    var collection = TransformHelper.Transform(ui);

            //    Assert.AreEqual("view", collection.Items[0].Name);

            //    Assert.AreEqual(56.7, collection.Items[0].Value);

            //    Assert.IsTrue(collection.Items[0].ValueIsNumber);
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