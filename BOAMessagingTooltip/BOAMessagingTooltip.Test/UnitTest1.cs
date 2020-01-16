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
            void assert(MessagingAccessInfo info)
            {
                info.GroupCode.Should().Be("CallCenter");

                info.PropertyName.Should().Be("SendMailFailure");

                MessagingDataAccess.FillTR(info);

                info.TurkishText.Should().Be("E-posta gönderilemedi.");
            }


            assert(Parser.Parse(@"ErrorCode = BOA.Messaging.MessagingHelper.GetMessage(""CallCenter"", ""SendMailFailure""),Severity = Severity.Error });"));
            assert(Parser.Parse(@"ErrorCode = BOA.Messaging.MessagingHelper.GetMessage(""CallCenter"", ""SendMailFailure""    ),Severity = Severity.Error });"));
            assert(Parser.Parse(@"ErrorCode = BOA.Messaging.MessagingHelper.GetMessage(""CallCenter"", ""SendMailFailure"",Severity = Severity.Error });"));
            assert(Parser.Parse(@"ErrorCode = BOA.Messaging.MessagingHelper.GetMessage(""CallCenter"", ""SendMailFailure""   ,Severity = Severity.Error });"));
        }
        #endregion
    }
}