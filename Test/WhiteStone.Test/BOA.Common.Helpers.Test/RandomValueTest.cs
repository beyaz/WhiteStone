using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers.Test
{
    public class A
    {
        #region Public Properties
        public A A1 { get; set; }

        public A2 A2 { get; set; }

        public List<A> AList { get; set; }

        public IReadOnlyCollection<A> IReadOnlyCollectionProperty { get; set; }

        public IReadOnlyList<A> ReadOnlyList { get; set; }
        public string stringValue { get; set; }
        #endregion
    }

    public class A2
    {
        #region Public Properties
        public IReadOnlyList<A> ReadOnlyList { get; set; }
        #endregion
    }

    [TestClass]
    public class RandomValueTest
    {

        [TestMethod]
        public void String_With_Length_Parameter()
        {
            var len = 5432;
            Assert.AreEqual(len, RandomValue.String(len).Length);
        }


        #region Public Methods
        [TestMethod]
        public void If_Property_Has_Already_Value_Do_Not_Change_It()
        {
            var instance = RandomValue.Object<Class_With_Value_Set_In_Constructor>();
            Assert.AreEqual(56, instance.Property_int);
            Assert.AreEqual(78, instance.Property_int_nullable);
            Assert.IsTrue(instance.Property_int_nullable2 != null);

            Assert.IsTrue(instance.Labels != null);
            Assert.AreEqual("Aloha", instance.Labels.A);
            Assert.IsNull(instance.Labels.A2);
        }

        [TestMethod]
        public void Must_Support_Circular_Referenced_Types()
        {
            RandomValue.Object<A>();
            var a = RandomValue.Object<A>();

            Assert.IsTrue(a.AList.Count > 1);

            Assert.IsTrue(a.ReadOnlyList.Count > 1);
            Assert.IsTrue(a.A2.ReadOnlyList.Count > 1);

            Assert.IsTrue(a.IReadOnlyCollectionProperty.Count > 1);

            for (var i = 0; i < 10; i++)
            {
                RandomValue.Object<A>();
            }
            Assert.AreEqual(0, RandomValue._objectCreationStack.Count);
        }

        [TestMethod]
        public void Must_Support_Primitive_Types()
        {
            var instance = RandomValue.Object<MyClassHasPrimitiveTypes>();

            Assert.AreNotEqual(0, instance.Int32);
            Assert.AreNotEqual(0, instance.Int16);
            Assert.AreNotEqual(0, instance.UInt16);
            Assert.AreNotEqual(0, instance.Int64);
            Assert.AreNotEqual(0, instance.UInt64);
        }
        #endregion

        class MyClassHasPrimitiveTypes
        {
            #region Public Properties
            public short? Int16 { get; set; }
            public int? Int32 { get; set; }
            public long? Int64 { get; set; }
            public ushort? UInt16 { get; set; }
            public ulong? UInt64 { get; set; }
            #endregion
        }
    }

    public class Labels
    {
        #region Constructors
        public Labels()
        {
            A = "Aloha";
        }
        #endregion

        #region Public Properties
        public string A { get; set; }
        public string A2 { get; set; }
        #endregion
    }

    public class Class_With_Value_Set_In_Constructor
    {
        #region Constructors
        public Class_With_Value_Set_In_Constructor()
        {
            Property_int = 56;
            Property_int_nullable = 78;
            Labels = new Labels();
        }
        #endregion

        #region Public Properties
        public Labels Labels { get; set; }
        public int Property_int { get; set; }
        public int? Property_int_nullable { get; set; }
        public int? Property_int_nullable2 { get; set; }
        #endregion
    }
}