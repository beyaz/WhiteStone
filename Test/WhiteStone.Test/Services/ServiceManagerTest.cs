using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteStone.Services;

namespace WhiteStone.Test.Services
{
    public interface ITestInterface
    {
        string GetA();
    }

    public class ITestInterfaceImplementation : ITestInterface
    {
        public string GetA()
        {
            return "A";
        }
    }

    [TestClass]
    public class ServiceManagerTest
    {
        ServiceManager Api => new ServiceManager();

        [TestMethod]
        public void TryToSetServiceFromName_MustBeHandleByNameForWhiteStoneServices()
        {
            Assert.IsNotNull(Api.GetService<IDataTableStringifier>());
        }

        [TestMethod]
        public void LoadFromString()
        {
            // ARRANGE
            var json = @"

    [
        {
            InterfaceName: 'WhiteStone.Test.Services.ITestInterface',
            ImplementationTypeName: 'WhiteStone.Test.Services.ITestInterfaceImplementation',
            AssemblyName:'WhiteStone.Test'
        }
    ]


";

            var sm = new ServiceManager();

            // ACT
            sm.LoadFromJsonString(json);

            // ASSERT
            var service = sm.GetService<ITestInterface>();
            Assert.IsNotNull(service);
            Assert.AreEqual("A", service.GetA());
        }
    }
}