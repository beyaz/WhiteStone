using System;
using System.Collections.Generic;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class BTab
    {
        #region Constructors
        public BTab(string title, IReadOnlyCollection<BCard> cards)
        {
            Cards = cards;
            Title = title;
        }

        public BTab(string title, IReadOnlyCollection<BField> fields)
        {
            Cards = new[]
            {
                new BCard("", fields)
            };
            Title = title;
        }

        public BTab(Enum title, IReadOnlyCollection<BField> fields) : this(title.ToString(), fields)
        {
        }
        #endregion

        #region Public Properties
        public IReadOnlyCollection<BCard> Cards { get; }
        public string                     Title { get; }
        #endregion
    }
}