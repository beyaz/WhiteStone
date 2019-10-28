using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BOA.EntityGeneration.DbModel.SqlServerDataAccess
{
    [TestClass]
    public class NamingTest
    {
        [TestMethod]
        public void Support_BIGGER_AND_SEPARATED_NAMING()
        {
            "USER_NAME".ToContractName().Should().Be("UserName");
            "HATTORI_HANZO".ToContractName().Should().Be("HattoriHanzo");
            "UserName".ToContractName().Should().Be("UserName");
            "User_name".ToContractName().Should().Be("UserName");
        }
    }
}