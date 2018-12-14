using BOA.Lang.Data;
using BOA.Lang.Lexers;

namespace BOA.Lang.Matches
{
    /// <summary>
    ///     The match white space
    /// </summary>
    class MatchWhiteSpace : MatcherBase
    {
        #region Methods
        /// <summary>
        ///     Determines whether [is match implementation] [the specified tokenizer].
        /// </summary>
        protected override Token IsMatchImpl(Tokenizer tokenizer)
        {
            var foundWhiteSpace = false;

            while (!tokenizer.End() && string.IsNullOrWhiteSpace(tokenizer.Current))
            {
                foundWhiteSpace = true;

                tokenizer.Consume();
            }

            if (foundWhiteSpace)
            {
                return new Token(TokenType.WhiteSpace);
            }

            return null;
        }
        #endregion
    }
}