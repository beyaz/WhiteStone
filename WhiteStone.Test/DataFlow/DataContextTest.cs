using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.DataFlow
{
    [TestClass]
    public class DataContextTest
    {
        #region Public Methods

        static readonly IDataConstant<string> data_bracket_0_0 = DataConstant.Create<string>();

        static readonly IDataConstant<string> data_bracket_1_0 = DataConstant.Create<string>();
        static readonly IDataConstant<string> data_bracket_1_1 = DataConstant.Create<string>();

        static readonly IDataConstant<string> data_bracket_2_0 = DataConstant.Create<string>();
        static readonly IDataConstant<string> data_bracket_2_1 = DataConstant.Create<string>();
        static readonly IDataConstant<string> data_bracket_2_2 = DataConstant.Create<string>();

        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Throw_Error_When_Brackets_Is_Not_Matched()
        {
            IDataContext context = new DataContext();
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
            IDataContext context = new DataContext();
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
            

            IDataContext context = new DataContext();

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