using System;
using System.Collections;
using System.Linq;
using System.Windows;
using WpfControls;
using static BOA.EntityGeneration.UI.Container.App;
using static BOA.EntityGeneration.UI.Container.Data;

namespace BOA.EntityGeneration.UI.Container.Components
{
    public class SchemaNameComponent : AutoCompleteTextBox
    {
        #region Constructors
        public SchemaNameComponent()
        {
            
            Provider = new SuggestionProvider();
        }
        #endregion

        class SuggestionProvider : ISuggestionProvider
        {
            #region Public Methods
            public IEnumerable GetSuggestions(string filter)
            {
                if (string.IsNullOrEmpty(filter))
                {
                    return null;
                }

                return SchemaNames[Context].Where(name => name.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            #endregion
        }
    }
}