using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOAMessagingTooltip.Test
{
    [TestClass]
    public class LineParserTest
    {
        #region Public Methods
        [TestMethod]
        public void ShouldExtractMessagingCodeAndPropertyIfLineContainsMessagingAccessLine()
        {
            
            const string line = @"ErrorCode = BOA.Messaging.MessagingHelper.GetMessage(""CallCenter"", ""SendMailFailure""),";

            var          info = Parser.Parse(line);

            info.GroupCode.Should().Be("CallCenter");
            info.PropertyName.Should().Be("SendMailFailure");

            MessagingDataAccess.FillTR(info);

            info.TurkishText.Should().Be("E-posta gönderilemedi.");
        }
        #endregion
    }
}