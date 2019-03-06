using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using BOA.Common.Helpers;
using BOA.DatabaseAccess;
using BOA.OneDesigner.AppModel;
using DotNetKit.Windows.Controls;
using FeserWard.Controls;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.WpfControls
{
    class ResourceCodeTextBox2:LabeledComboBox
    {
        public ResourceCodeTextBox2()
        {
            ItemsSource = new List<Aut_Resource>();
            PART_AutoCompleteComboBox.Setting = new Settings(this);
        }

        class Settings :AutoCompleteComboBoxSetting
        {
            ResourceCodeTextBox2 combo;
            public Settings(ResourceCodeTextBox2 combo)
            {
                this.combo = combo;
            }
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

            public override Func<object, bool> GetFilter(string query, Func<object, string> stringFromItem)
            {
                using (var database = new DevelopmentDatabase())
                {
                    var items = database.GetRecords<Aut_Resource>("SELECT TOP 5 Name,ResourceCode from AUT.Resource WITH(NOLOCK) WHERE Name LIKE '%" + query + "%' OR ResourceCode = '" + query + "'");

                    combo.ItemsSource = items;

                    return item=>items.Any(x =>IsMatch(x ,query));

                }

            }
        }
    }
    class ResourceCodeTextBox : IntellisenseTextBox
    {
        #region Constructors
        public ResourceCodeTextBox()
        {
            QueryProvider = new RequestPropertyIntellisenseProvider();
        }
        #endregion

        #region Public Properties
        public bool SearchByCurrentSelectedDataGridDataSourceContract { get; set; }

        public bool ShowOnlyBooleanProperties { get; set; }

        public bool ShowOnlyCollectionProperties   { get; set; }
        public bool ShowOnlyNotNullInt32Properties { get; set; }
        public bool ShowOnlyOrchestrationMethods   { get; set; }

        public bool ShowOnlyStringProperties { get; set; }
        #endregion

        class RequestPropertyIntellisenseProvider : IIntelliboxResultsProvider
        {
            #region Public Methods
            public IEnumerable DoSearch(string searchTerm, int maxResults, object tag)
            {
                using (var database = new DevelopmentDatabase())
                {
                    var items = database.GetRecords<Aut_Resource>("SELECT TOP 5 Name,ResourceCode from AUT.Resource WITH(NOLOCK) WHERE Name LIKE '%" + searchTerm + "%' OR ResourceCode = '" + searchTerm + "'");

                    return items.Where(x => IsMatch(x, searchTerm)).Select(t => t).Take(maxResults);
                }
            }
            #endregion

            #region Methods
            protected IEnumerable<string> Sort(IEnumerable<string> preResults)
            {
                return preResults.OrderByDescending(s => s.Length);
            }

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