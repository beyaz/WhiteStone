﻿using System.Linq;
using BOA.OneDesigner.WpfControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.OneDesigner
{
    [TestClass]
    public class CecilHelperTest
    {
        #region Public Methods
        [TestMethod]
        public void GetAllBindProperties()
        {
            var bindProperties = CecilHelper.GetAllBindProperties(@"d:\boa\server\bin\BOA.Types.CardGeneral.DebitCard.dll", "BOA.Types.CardGeneral.DebitCard.ForeignDebitClearingRequest");

            Assert.IsTrue(bindProperties.Contains("request.FileId"));

            Assert.IsTrue(bindProperties.Contains("request.Account.IBAN"));
        }
        #endregion
    }
}