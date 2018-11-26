using System;
using System.Collections.Generic;
using System.Data;
using BOA.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Helpers;

namespace WhiteStone.Test
{
    [TestClass]
    public class ReflectionTests
    {
        [TestMethod]
        public void ToListToList()
        {
            // ARRANGE
            var dt = new DataTable();
            dt.Columns.Add("A", typeof(string));
            dt.Columns.Add("B", typeof(string));

            dt.Rows.Add("00", "01");
            dt.Rows.Add("10", "11");
            dt.Rows.Add("20", "21");

            // ACT
            var list = dt.Columns[1].ToList<string>();

            // ASSERT
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("01", list[0]);
            Assert.AreEqual("11", list[1]);
            Assert.AreEqual("21", list[2]);
        }




        string ExportObjectToCSharpCode(object obj)
        {
            var str = ReflectionHelper.ExportObjectToCSharpCode(obj);
            return str.Replace(Environment.NewLine, "").Replace(" ", "");
        }

        [TestMethod]
        public void ExportObjectToCSharpCode_Test()
        {
            

            Assert.AreEqual("null", ExportObjectToCSharpCode(null));

            Assert.AreEqual("\"Aloha\"", ExportObjectToCSharpCode("Aloha"));

            Assert.AreEqual("\"Aloha\"", ExportObjectToCSharpCode("Aloha"));

            Assert.AreEqual("@\"Aloha\"", ExportObjectToCSharpCode(@"A
loha"));

            var instance = new ExportObjectToCSharpCode_Test_Class_1();

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1()", ExportObjectToCSharpCode(instance));

            instance.A1 = 6;

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6}", ExportObjectToCSharpCode(instance));

            instance.A2 = 79;

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79}", ExportObjectToCSharpCode(instance));

            instance.A78 = 654.87;

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87}", ExportObjectToCSharpCode(instance));

            instance.A4 = new ExportObjectToCSharpCode_Test_Class_1();

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87,A4=newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1()}", ExportObjectToCSharpCode(instance));


            instance.A4.A1 = 65;

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87,A4=newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=65}}", ExportObjectToCSharpCode(instance));


            instance.A6 = new List<int?>();

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87,A4=newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=65},A6=newSystem.Collections.Generic.List<System.Nullable<System.Int32>>{}}", ExportObjectToCSharpCode(instance));

            instance.A6.Add(34);
            instance.A6.Add(38);
            instance.A6.Add(9);

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87,A4=newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=65},A6=newSystem.Collections.Generic.List<System.Nullable<System.Int32>>{34,38,9}}", ExportObjectToCSharpCode(instance));

            instance.A5 = new List<ExportObjectToCSharpCode_Test_Class_1>();

            var value = ExportObjectToCSharpCode(instance);

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87,A4=newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=65},A5=newSystem.Collections.Generic.List<WhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1>{},A6=newSystem.Collections.Generic.List<System.Nullable<System.Int32>>{34,38,9}}", value);


            instance.A5.Add(new ExportObjectToCSharpCode_Test_Class_1
            {
                A1 = 5
            });

            value = ExportObjectToCSharpCode(instance);

            var expected = "newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=6,A2=79,A78=654.87,A4=newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=65},A5=newSystem.Collections.Generic.List<WhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1>{newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A1=5}},A6=newSystem.Collections.Generic.List<System.Nullable<System.Int32>>{34,38,9}}";

            Assert.AreEqual(expected, value);

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A7=newSystem.DateTime(2015,1,1,1,1,1,1)}", ExportObjectToCSharpCode(new ExportObjectToCSharpCode_Test_Class_1
            {
                A7 = new DateTime(2015, 1, 1, 1, 1, 1, 1)
            }));


            var instance2 = new ExportObjectToCSharpCode_Test_Class_1
            {
                A10 = ExportObjectToCSharpCode_Test_EnumType.B
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{A10=WhiteStone.Test.ExportObjectToCSharpCode_Test_EnumType.B}", ExportObjectToCSharpCode(instance2));


            var guid = Guid.NewGuid();

            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                GuidProperty = guid
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{GuidProperty=newSystem.Guid(\"" + guid + "\")}", ExportObjectToCSharpCode(instance));

            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                D1Dictionary = new Dictionary<string, object>
                {
                    {"a", "t"}
                }
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{D1Dictionary=newSystem.Collections.Generic.Dictionary<System.String,System.Object>{{\"a\",\"t\"}}}", ExportObjectToCSharpCode(instance));

            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                D1Dictionary = new Dictionary<string, object>
                {
                    {"a", new KeyValuePair<int, short>(4, 7)}
                }
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{D1Dictionary=newSystem.Collections.Generic.Dictionary<System.String,System.Object>{{\"a\",System.Collections.Generic.KeyValuePair<System.Int32,System.Int16>(4,7)}}}", ExportObjectToCSharpCode(instance));


            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                ArrayProperty_String = new[] {"A", "B"}
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{ArrayProperty_String=newSystem.String[]{\"A\",\"B\"}}", ExportObjectToCSharpCode(instance));

            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                ReadOnlyList1 = new List<string> {"A", "B"}
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{ReadOnlyList1=newSystem.Collections.Generic.List<System.String>{\"A\",\"B\"}}", ExportObjectToCSharpCode(instance));


            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                ReadOnlyList2 = new List<string> {"A", "B"}
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{ReadOnlyList2=newSystem.Collections.Generic.List<System.String>{\"A\",\"B\"}}", ExportObjectToCSharpCode(instance));


            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                IList = new[] {"A", "B"}
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{IList=newSystem.String[]{\"A\",\"B\"}}", ExportObjectToCSharpCode(instance));


            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                DerivedEnum = new DerivedEnum("A", 6)
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{DerivedEnum=newWhiteStone.Test.DerivedEnum(\"A\",6)}", ExportObjectToCSharpCode(instance));

            instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                intArray = new[] {5, 7}
            };

            Assert.AreEqual("newWhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1{intArray=newSystem.Int32[]{5,7}}", ExportObjectToCSharpCode(instance));



            

           
        }

        [TestMethod]
        public void TestCircularObjects()
        {
            var instance = new ExportObjectToCSharpCode_Test_Class_1
            {
                Child1 = new ExportObjectToCSharpCode_Test_Class_1
                {
                    ReadOnlyList2 = new List<string>
                    {
                        "A",
                        "B"
                    }
                }
            };

            instance.Child1.Child1 = instance;

            var Exporter         = new ReflectionHelper.ObjectToCSharpCodeExporter();
            var objectASharpCode = Exporter.Export(instance);

            const string expected = @"new WhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1
            {
                Child1 = new WhiteStone.Test.ExportObjectToCSharpCode_Test_Class_1
                {
                    ReadOnlyList2 = new System.Collections.Generic.List<System.String>
                    {
                        ""A"",
                        ""B""
                    }
                }
            }
            ";

            Assert.IsTrue(StringHelper.IsEqualAsData(expected, objectASharpCode));

            Assert.AreEqual(0, Exporter._objectCreationStack.Count);

            Exporter = new ReflectionHelper.ObjectToCSharpCodeExporter();
            Exporter.UsingList.Add(typeof(ExportObjectToCSharpCode_Test_Class_1).Namespace);
            Exporter.UsingList.Add(typeof(List<>).Namespace);


            objectASharpCode = Exporter.Export(instance);

            const string expectedWithUsing = @"new ExportObjectToCSharpCode_Test_Class_1
            {
                Child1 = new ExportObjectToCSharpCode_Test_Class_1
                {
                    ReadOnlyList2 = new List<System.String>
                    {
                        ""A"",
                        ""B""
                    }
                }
            }
            ";

            Assert.IsTrue(StringHelper.IsEqualAsData(expectedWithUsing, objectASharpCode));

            Assert.AreEqual(0, Exporter._objectCreationStack.Count);


        }
    }

    public class DerivedEnum
    {
        public string Name { get; private set; }
        public int Value{ get; private set; }
        public DerivedEnum(string A,int B)
        {
            Name = A;
            Value = B;
        }
    }
    public class ExportObjectToCSharpCode_Test_Class_1
    {
        public int A1 { get; set; }
        public int? A2 { get; set; }

        public double? A78 { get; set; }

        public string A3 { get; set; }

        public ExportObjectToCSharpCode_Test_Class_1 A4 { get; set; }

        public List<ExportObjectToCSharpCode_Test_Class_1> A5 { get; set; }

        public List<int?> A6 { get; set; }

        public DateTime A7 { get; set; }

        public ExportObjectToCSharpCode_Test_EnumType A10 { get; set; }

        public string A35 { get; private set; }

        public Guid GuidProperty { get; set; }

        public Dictionary<string, object> D1Dictionary { get; set; }

        public string[] ArrayProperty_String { get; set; }

        public IReadOnlyList<string> ReadOnlyList1 { get; set; }
        public IReadOnlyCollection<string> ReadOnlyList2 { get; set; }
        public IList<string> IList { get; set; }

        public DerivedEnum DerivedEnum { get; set; }

        public int[] intArray { get; set; }

        public ExportObjectToCSharpCode_Test_Class_1 Child1 { get; set; }
            
    }

    public enum ExportObjectToCSharpCode_Test_EnumType
    {
        A,
        B,
        C
    }

 
}