using System;
using BOA.OneDesigner.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner.Helper
{
    [TestClass]
    public class TsxCodeBeautifierTest
    {
        #region Public Methods
        [TestMethod]
        public void Should_reduce_empty_two_line_to_one_line()
        {
            TsxCodeBeautifier.Beautify("A" + Environment.NewLine + Environment.NewLine + Environment.NewLine + "B")
                             .Should().Be("A" + Environment.NewLine + Environment.NewLine + "B");

            TsxCodeBeautifier.Beautify(@"A0
A1
A2





A3
A4
").Should().Be(@"A0
A1
A2

A3
A4");

            TsxCodeBeautifier.Beautify(@"A0
A1
A2


}


A3
A4
").Should().Be(@"A0
A1
A2
}

A3
A4");
        }
        #endregion
    }
}