using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.OneDesigner.AppModel;
using DotNetKit.Windows.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class ResourceCodeTextBox : LabeledComboBox
    {
        public static readonly List<Aut_Resource> Items;
        static ResourceCodeTextBox()
        {
            using (var database = new DevelopmentDatabase())
            {
                Items = database.GetRecords<Aut_Resource>("SELECT Name,ResourceCode from AUT.Resource WITH(NOLOCK)");
            }
        }

        #region Constructors
        public ResourceCodeTextBox()
        {
            ItemsSource = Items;


            // PART_AutoCompleteComboBox.Setting = new Settings(this);

            // AddHandler(TextBoxBase.TextChangedEvent, new TextChangedEventHandler(OnTextChanged));

        }

        //private void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    MessageBox.Show("t");
        //}

        #endregion

        class Settings : AutoCompleteComboBoxSetting
        {
            #region Fields
            readonly ResourceCodeTextBox combo;
            #endregion

            #region Constructors
            public Settings(ResourceCodeTextBox combo)
            {
                this.combo = combo;
            }
            #endregion

            #region Public Methods
            public override Func<object, bool> GetFilter(string query, Func<object, string> stringFromItem)
            {
                using (var database = new DevelopmentDatabase())
                {
                    // var items = database.GetRecords<Aut_Resource>("SELECT TOP 5 Name,ResourceCode from AUT.Resource WITH(NOLOCK) WHERE Name LIKE '%" + query + "%' OR ResourceCode = '" + query + "'");

                    var items = Items.Where(x => IsMatch(x, query)).ToList();

                    combo.ItemsSource = items;

                    return item => items.Any(x => IsMatch(x, query));
                }
            }
            #endregion

            #region Methods
            static bool IsMatch(Aut_Resource x, string searchTerm)
            {
                if (x == null)
                {
                    return false;
                }

                if (x.Name == null)
                {
                    return false;
                }

                if (x.ResourceCode == null)
                {
                    return false;
                }

                return x.Name.ToUpperEN().Contains(searchTerm.ToUpperEN()) || x.ResourceCode.ToUpperEN().Contains(searchTerm.ToUpperEN());
            }
            #endregion
        }
    }
}