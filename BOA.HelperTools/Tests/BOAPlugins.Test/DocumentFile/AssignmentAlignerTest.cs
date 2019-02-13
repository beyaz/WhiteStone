using BOA.Common.Helpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BOAPlugins.DocumentFile
{
   

    [TestClass]
    public class AssignmentAlignerTest
    {
        // [TestMethod]
        public void Should_align_by_assignment()
        {
            const string code = @"

dataSource.GridList = dppKeyInfoList;
request.State.IsSaveActionVisible = false;
request.StatusMessage = string.Format('{0} Adet Kayıt Getitildi', dppKeyInfoList.Count);

";


                        const string expected = @"

dataSource.GridList               = dppKeyInfoList;
request.State.IsSaveActionVisible = false;
request.StatusMessage             = string.Format('{0} Adet Kayıt Getitildi', dppKeyInfoList.Count);

";

            var data = new AssignmentAlignerData
            {
                Lines = code.SplitAndClear(Environment.NewLine).ToList()
            };

            AssignmentAligner.Align(data);

            data.OutputLines[0].Should().Be("dataSource.GridList               = dppKeyInfoList;");


        }
    }
}