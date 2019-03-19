using System.Collections.Generic;
using DotNetKit.Windows.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WhiteStone.UI.Container.Test
{
    public class UserInfo
    {
        #region Public Properties
        public string Id   { get; set; }
        public string Name { get; set; }
        #endregion

        #region Public Methods
        public override string ToString()
        {
            return Id + " - " + Name;
        }
        #endregion
    }

    [TestClass]
    public class AutoCompleteComboBoxTest
    {
        #region Fields
        List<UserInfo> Users;
        #endregion

        #region Public Methods
        [TestInitialize]
        public void Initialize()
        {
            Users = new List<UserInfo>
            {
                new UserInfo {Name = "Name0", Id = "0"},
                new UserInfo {Name = "Name1", Id = "1"},
                new UserInfo {Name = "Name2", Id = "2"}
            };
        }

        [TestMethod]
        public void Test1()
        {
            var autoCompleteComboBox = new AutoCompleteComboBox
            {
                DisplayMemberPath = nameof(UserInfo.Name),
                SelectedValuePath = nameof(UserInfo.Id),
                ItemsSource       = Users
            };

            autoCompleteComboBox.ApplyTemplate();

            autoCompleteComboBox.SelectedValue = "1";
            autoCompleteComboBox.SelectedItem.Should().BeSameAs(Users[1]);
            autoCompleteComboBox.Text.Should().BeSameAs("Name1");

            autoCompleteComboBox.EditableTextBox.Text.Should().BeSameAs("Name1");

            autoCompleteComboBox.SelectedValue = "2";
            autoCompleteComboBox.SelectedItem.Should().BeSameAs(Users[2]);
            autoCompleteComboBox.Text.Should().BeSameAs("Name2");
            autoCompleteComboBox.EditableTextBox.Text.Should().BeSameAs("Name2");
        }

        
        [TestMethod]
        public void LabeledComboBox()
        {
            App.InitializeBuilder();

            var autoCompleteComboBox = new LabeledComboBox
            {
                DisplayMemberPath = nameof(UserInfo.Name),
                SelectedValuePath = nameof(UserInfo.Id),
                ItemsSource       = Users
            };

            autoCompleteComboBox.ApplyTemplate();
            autoCompleteComboBox.PART_AutoCompleteComboBox.ApplyTemplate();


            autoCompleteComboBox.SelectedValue = "1";
            autoCompleteComboBox.PART_AutoCompleteComboBox. SelectedItem.Should().BeSameAs(Users[1]);
            autoCompleteComboBox.Text.Should().BeSameAs("Name1");

            autoCompleteComboBox.PART_AutoCompleteComboBox.EditableTextBox.Text.Should().BeSameAs("Name1");

            autoCompleteComboBox.SelectedValue = "2";
            autoCompleteComboBox.PART_AutoCompleteComboBox.SelectedItem.Should().BeSameAs(Users[2]);
            autoCompleteComboBox.Text.Should().BeSameAs("Name2");
            autoCompleteComboBox.PART_AutoCompleteComboBox.EditableTextBox.Text.Should().BeSameAs("Name2");
        }

        #endregion
    }
}