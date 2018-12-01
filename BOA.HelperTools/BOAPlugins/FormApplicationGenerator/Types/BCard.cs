using System;
using System.Collections.Generic;

namespace BOAPlugins.FormApplicationGenerator.Types
{
    [Serializable]
    public class BCard
    {
        #region Constructors
        public BCard(string title, IReadOnlyCollection<BField> fields)
        {
            Fields = fields;
            Title  = title;
        }

        public BCard(Enum title, IReadOnlyCollection<BField> fields) : this(title.ToString(), fields)
        {
        }
        #endregion

        #region Public Properties
        public IReadOnlyCollection<BField> Fields { get; }
        public string                      Title  { get; }
        #endregion
    }
}