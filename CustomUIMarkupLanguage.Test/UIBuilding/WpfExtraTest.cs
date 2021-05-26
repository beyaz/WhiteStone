using System.Windows.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomUIMarkupLanguage.UIBuilding
{
    [TestClass]
    public class WpfExtraTest
    {
        [TestMethod]
        public void Should_convert_to_group_box_StackPanel_with_title()
        {
            var groupBox = new GroupBox();

            groupBox.LoadJson("{ui:'StackPanel',Title:'Aloha'}");

            groupBox.Header.Should().Be("Aloha");
        }

        [TestMethod]
        public void Should_convert_to_group_box_StackPanel_with_title_2()
        {
            var groupBox = new GroupBox();

            groupBox.LoadJson("{ui:'GrouBox',Content:{ ui:'StackPanel',Title:'Aloha'  }}");

            ((GroupBox)groupBox.Content).Header.Should().Be("Aloha");
            ((GroupBox)groupBox.Content).Content.Should().BeAssignableTo(typeof(StackPanel));
        }
    }
}
