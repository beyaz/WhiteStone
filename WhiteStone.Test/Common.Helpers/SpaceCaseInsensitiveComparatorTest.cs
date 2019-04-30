using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.Common.Helpers
{
    [TestClass]
    public class SpaceCaseInsensitiveComparatorTest
    {
        #region Public Methods
        [TestMethod]
        public void Test()
        {
            const string oldContent = @"onCommitmentCampaignListRowSelectionChanged() {
        this.executeWindowRequest(""GetCommitmentCampaignDetail"");
    }";

            const string newContent = @"onCommitmentCampaignListRowSelectionChanged()
    {
        this.executeWindowRequest(""GetCommitmentCampaignDetail"");
    }";

            StringHelper.IsEqualAsData(oldContent, newContent).Should().BeTrue();
        }
        #endregion
    }
}