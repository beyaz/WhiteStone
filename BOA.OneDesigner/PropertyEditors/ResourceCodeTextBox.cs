using System.Collections.Generic;
using BOA.DatabaseAccess;
using BOA.OneDesigner.AppModel;
using WhiteStone.UI.Container;

namespace BOA.OneDesigner.PropertyEditors
{
    class ResourceCodeTextBox : LabeledComboBox
    {
        #region Static Fields
        public static readonly List<Aut_Resource> Items;
        #endregion

        #region Constructors
        static ResourceCodeTextBox()
        {
            using (var database = new DevelopmentDatabase())
            {
                Items = database.GetRecords<Aut_Resource>("SELECT Name,ResourceCode from AUT.Resource WITH(NOLOCK)");
            }
        }

        public ResourceCodeTextBox()
        {
            ItemsSource = Items;
        }
        #endregion
    }
}