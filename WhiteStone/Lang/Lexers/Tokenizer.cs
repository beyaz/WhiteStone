using System.Globalization;
using System.Linq;

namespace Lang.Lexers
{
    /// <summary>
    ///     The tokenizer
    /// </summary>
    public class Tokenizer : TokenizableStreamBase<string>
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Tokenizer" /> class.
        /// </summary>
        public Tokenizer(string source)
            : base(() => source.ToCharArray().Select(i => i.ToString(CultureInfo.InvariantCulture)).ToList())
        {
        }
        #endregion
    }
}