using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CustomUIMarkupLanguage.Markup
{
    /// <summary>
    ///     The node collection
    /// </summary>
    public class NodeCollection : IEnumerable<Node>
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        public List<Node> Items { get; set; } = new List<Node>();
        #endregion

        #region Public Indexers
        /// <summary>
        ///     Gets the <see cref="Node" /> with the specified name.
        /// </summary>
        public Node this[string name]
        {
            get
            {
                name = name.ToUpperEN();
                return Items.FirstOrDefault(x => x.NameToUpperInEnglish == name);
            }
        }

        /// <summary>
        ///     Gets the <see cref="Node" /> at the specified index.
        /// </summary>
        public Node this[int index]
        {
            get { return Items[index]; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<Node> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
        #endregion

        #region Explicit Interface Methods
        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}