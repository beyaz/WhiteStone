using System.Text;
using Lang.Data;
using Lang.Lexers;

namespace Lang.Matches
{
    /// <summary>
    ///     The match string
    /// </summary>
    public class MatchString : MatcherBase
    {
        #region Constants
        /// <summary>
        ///     The quote
        /// </summary>
        public const string QUOTE = "\"";

        /// <summary>
        ///     The tic
        /// </summary>
        public const string TIC = "'";
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MatchString" /> class.
        /// </summary>
        public MatchString(string delim)
        {
            StringDelim = delim;
        }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the string delimiter.
        /// </summary>
        string StringDelim { get; set; }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is match implementation] [the specified tokenizer].
        /// </summary>
        protected override Token IsMatchImpl(Tokenizer tokenizer)
        {
            var str = new StringBuilder();

            if (tokenizer.Current == StringDelim)
            {
                tokenizer.Consume();

                while (!tokenizer.End() && tokenizer.Current != StringDelim)
                {
                    str.Append(tokenizer.Current);
                    tokenizer.Consume();
                }

                if (tokenizer.Current == StringDelim)
                {
                    tokenizer.Consume();
                }
            }

            if (str.Length > 0)
            {
                return new Token(TokenType.QuotedString, str.ToString());
            }

            return null;
        }
        #endregion
    }
}