using System.Collections.Generic;
using BOAPlugins.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAPlugins.RemoveUnusedMessagesInTypescriptCodes
{
    [TestClass]
    public class HandlerTest
    {
        #region Public Methods
        [TestMethod]
        public void PickupMessage()
        {
            var dictionary = new Dictionary<string, string>();
            var tsxCode = @"Messages.A
Message.AB,
Message.AB)

Message.AB}
 Message.AB},
{Message.ABC}
Message.ABCD.xxx
Message.ABCD,
Message.ABCD 
Message.ABCD
:Message.ABCD,
+Message.ABCD,
Message.ABCD,

";

            MessagesCleaner.PickupMessage(tsxCode, dictionary);

            Assert.AreEqual(4, dictionary.Count);
        }
        #endregion
    }
}