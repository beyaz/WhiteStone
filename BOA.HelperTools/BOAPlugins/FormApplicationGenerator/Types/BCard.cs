using System;
using System.Collections.Generic;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class BCardSection
    {
        #region Public Properties
        public IReadOnlyList<BCard> Cards { get; set; }
        #endregion
    }

    [Serializable]
    public class BCard
    {
        #region Constructors
        public BCard()
        {
        }

        public BCard(string title, IReadOnlyList<BField> fields)
        {
            Fields = fields;
            Title  = title;
        }

        public BCard(Enum title, IReadOnlyList<BField> fields) : this(title.ToString(), fields)
        {
        }
        #endregion

        #region Public Properties
        public IReadOnlyList<BField> Fields { get; set; }
        public string                Title  { get; set; }
        #endregion
    }
}