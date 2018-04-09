using Lang.Data;
using Lang.Lexers;

namespace Lang.Matches
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