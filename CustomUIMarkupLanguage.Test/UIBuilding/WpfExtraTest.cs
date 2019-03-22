using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomUIMarkupLanguage.UIBuilding
{
    [TestClass]
    public class WpfExtraTest
    {
        [TestMethod]
        public void A()
        {
            var groupBox = new GroupBox();
            groupBox.LoadJson("{ui:'StackPanel',Title:'Aloha'}");

            groupBox.Header.Should().Be("Aloha");
        }
    }
}
