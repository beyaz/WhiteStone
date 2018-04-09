using Lang.Data;
using Lang.Lexers;

namespace Lang.Matches
{
    /// <summary>
    ///     The matcher base
    /// </summary>
    public abstract class MatcherBase : IMatcher
    {
        #region Public Methods
        /// <summary>
        ///     Determines whether the specified tokenizer is match.
        /// </summary>
        public Token IsMatch(Tokenizer tokenizer)
        {
            if (tokenizer.End())
            {
                return new Token(TokenType.EOF);
            }

            tokenizer.TakeSnapshot();

            var match = IsMatchImpl(tokenizer);

            if (match == null)
            {
                tokenizer.RollbackSnapshot();
            }
            else
            {
                tokenizer.CommitSnapshot();
            }

            return match;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is match implementation] [the specified tokenizer].
        /// </summary>
        protected abstract Token IsMatchImpl(Tokenizer tokenizer);
        #endregion
    }
}