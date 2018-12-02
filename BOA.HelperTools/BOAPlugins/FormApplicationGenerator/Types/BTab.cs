using System;
using System.Collections.Generic;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class TabControl
    {
        #region Public Properties
        public IReadOnlyList<TabPage> TabPages { get; set; }
        #endregion
    }

    [Serializable]
    public class TabPage
    {
        #region Constructors
        public TabPage(string title, IReadOnlyList<BCard> cards)
        {
            Cards = cards;
            Title = title;
        }

        public TabPage(string title, IReadOnlyList<BField> fields)
        {
            Title = title;

            Cards = new[]
            {
                new BCard
                {
                    Fields = fields
                }
            };
        }

        public TabPage(Enum title, IReadOnlyList<BField> fields)
        {
            Title = title.ToString();

            Cards = new[]
            {
                new BCard
                {
                    Fields = fields
                }
            };
        }
        #endregion

        #region Public Properties
        public IReadOnlyList<BCard> Cards { get; set; }
        public string               Title { get; set; }
        #endregion
    }
}