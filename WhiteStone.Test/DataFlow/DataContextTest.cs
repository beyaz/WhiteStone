using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.DataFlow
{
    class Container_0:ContextContainer
    {
        public string data_bracket_0_0 => Context.Get(DataContextTest.data_bracket_0_0);
    }

    class Container_1:Container_0
    {
        public string data_bracket_1_0 => Context.Get(DataContextTest.data_bracket_1_0);
        public string data_bracket_1_1 => Context.Get(DataContextTest.data_bracket_1_1);
    }

    [TestClass]
    public class DataContextTest
    {
        #region Public Methods

        public static readonly IProperty<string> data_bracket_0_0 = Property.Create<string>();

        public static readonly IProperty<string> data_bracket_1_0 = Property.Create<string>();
        public static readonly IProperty<string> data_bracket_1_1 = Property.Create<string>();

        public static readonly IProperty<string> data_bracket_2_0 = Property.Create<string>();
        public static readonly IProperty<string> data_bracket_2_1 = Property.Create<string>();
        public static readonly IProperty<string> data_bracket_2_2 = Property.Create<string>();

        public static readonly Event Started = new Event {Name = nameof(Started)};
        public static readonly Event Finished = new Event {Name = nameof(Finished)};

       
        [TestMethod]
        public void Should_transfer_context()
        {
            IContext context = new Context();

            data_bracket_0_0[context] = "A";

            context.OpenBracket();
            data_bracket_1_0[context] = "B";
            data_bracket_1_1[context] = "C";

            data_bracket_0_0[context].Should().Be("A");
            data_bracket_1_0[context].Should().Be("B");
            data_bracket_1_1[context].Should().Be("C");

            var container0 = new Container_0 {Context = context};
            container0.Create<Container_1>().data_bracket_0_0.Should().Be("A");
            container0.Create<Container_1>().data_bracket_1_0.Should().Be("B");
            container0.Create<Container_1>().data_bracket_1_1.Should().Be("C");

            

        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throw_Error_When_Brackets_Is_Not_Matched()
        {
            IContext context = new Context();
            context.OpenBracket();
            context.OpenBracket();
            context.OpenBracket();

            context.CloseBracket();
            context.CloseBracket();
            context.CloseBracket();

            context.CloseBracket();
            context.CloseBracket();
        }

        [TestMethod]
        public void Brackets_Should_Match()
        {
            IContext context = new Context();
            context.OpenBracket();
            context.OpenBracket();
            context.OpenBracket();

            context.CloseBracket();
            context.CloseBracket();
            context.CloseBracket();
            context.CloseBracket();
        }

        [TestMethod]
        public void Should_Remove_All_Elements_In_Bracket_When_Bracket_Is_Closed()
        {
            

            IContext context = new Context();

            data_bracket_0_0[context] = "A";

            context.OpenBracket();
            data_bracket_1_0[context] = "B";
            data_bracket_1_1[context] = "C";

            data_bracket_0_0[context].Should().Be("A");
            data_bracket_1_0[context].Should().Be("B");
            data_bracket_1_1[context].Should().Be("C");

            context.OpenBracket();
            data_bracket_2_0[context] = "2_0";
            data_bracket_2_1[context] = "2_1";
            data_bracket_2_2[context] = "2_2";

            data_bracket_0_0[context].Should().Be("A");
            data_bracket_1_0[context].Should().Be("B");
            data_bracket_1_1[context].Should().Be("C");

            data_bracket_2_0[context].Should().Be("2_0");
            data_bracket_2_1[context].Should().Be("2_1");
            data_bracket_2_2[context].Should().Be("2_2");

            context.CloseBracket();

            data_bracket_0_0[context].Should().Be("A");
            data_bracket_1_0[context].Should().Be("B");
            data_bracket_1_1[context].Should().Be("C");

            context.TryGet(data_bracket_2_0).Should().Be(null);
            context.TryGet(data_bracket_2_1).Should().Be(null);
            context.TryGet(data_bracket_2_2).Should().Be(null);

            context.CloseBracket();

            data_bracket_0_0[context].Should().Be("A");

            context.CloseBracket();

            context.TryGet(data_bracket_0_0).Should().Be(null);
        }
        #endregion
    }
}