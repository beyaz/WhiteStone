using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteStone.UI.Container.Test
{
    [TestClass]
    public class WindowBaseTest
    {
        [TestMethod]
        public void ApplyMahAppMetroStyle()
        {
            WindowBase.ApplyMahAppMetroStyle(new ResourceDictionary(), true);
        }
    }
}
