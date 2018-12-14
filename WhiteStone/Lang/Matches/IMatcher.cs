using BOA.Lang.Data;
using BOA.Lang.Lexers;

namespace BOA.Lang.Matches
{
    /// <summary>
    ///     The matcher
    /// </summary>
    public interface IMatcher
    {
        #region Public Methods
        /// <summary>
        ///     Determines whether the specified tokenizer is match.
        /// </summary>
        Token IsMatch(Tokenizer tokenizer);
        #endregion
    }
}