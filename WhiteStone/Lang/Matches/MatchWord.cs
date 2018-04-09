using System.Collections.Generic;
using System.Linq;
using Lang.Data;
using Lang.Exceptions;
using Lang.Lexers;

namespace Lang.Matches
{
    /// <summary>
    ///     The match word
    /// </summary>
    public class MatchWord : MatcherBase
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MatchWord" /> class.
        /// </summary>
        public MatchWord(IEnumerable<IMatcher> keywordMatchers)
        {
            SpecialCharacters = keywordMatchers.Select(i => i as MatchKeyword).Where(i => i != null).ToList();
        }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the special characters.
        /// </summary>
        List<MatchKeyword> SpecialCharacters { get; set; }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is match implementation] [the specified tokenizer].
        /// </summary>
        protected override Token IsMatchImpl(Tokenizer tokenizer)
        {
            string current = null;

            while (!tokenizer.End() && !string.IsNullOrWhiteSpace(tokenizer.Current) && SpecialCharacters.All(m => m.Match != tokenizer.Current))
            {
                current += tokenizer.Current;
                tokenizer.Consume();
            }

            if (current == null)
            {
                return null;
            }

            // can't start a word with a special character
            if (SpecialCharacters.Any(c => current.StartsWith(c.Match)))
            {
                throw new InvalidSyntax(string.Format("Cannot start a word with a special character {0}", current));
            }

            return new Token(TokenType.Word, current);
        }
        #endregion
    }
}