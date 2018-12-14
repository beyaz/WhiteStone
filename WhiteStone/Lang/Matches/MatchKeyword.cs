using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BOA.Lang.Data;
using BOA.Lang.Lexers;

namespace BOA.Lang.Matches
{
    /// <summary>
    ///     The match keyword
    /// </summary>
    public class MatchKeyword : MatcherBase
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="MatchKeyword" /> class.
        /// </summary>
        public MatchKeyword(TokenType type, string match)
        {
            Match            = match;
            TokenType        = type;
            AllowAsSubString = true;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     If true then matching on { in a string like "{test" will match the first cahracter
        ///     because it is not space delimited. If false it must be space or special character delimited
        /// </summary>
        public bool AllowAsSubString { get; set; }

        /// <summary>
        ///     Gets or sets the match.
        /// </summary>
        public string Match { get; set; }

        /// <summary>
        ///     Gets or sets the special characters.
        /// </summary>
        public List<MatchKeyword> SpecialCharacters { get; set; }
        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the type of the token.
        /// </summary>
        TokenType TokenType { get; set; }
        #endregion

        #region Methods
        /// <summary>
        ///     Determines whether [is match implementation] [the specified tokenizer].
        /// </summary>
        protected override Token IsMatchImpl(Tokenizer tokenizer)
        {
            foreach (var character in Match)
            {
                if (tokenizer.Current == character.ToString(CultureInfo.InvariantCulture))
                {
                    tokenizer.Consume();
                }
                else
                {
                    return null;
                }
            }

            bool found;

            if (!AllowAsSubString)
            {
                var next = tokenizer.Current;

                found = string.IsNullOrWhiteSpace(next) || SpecialCharacters.Any(character => character.Match == next);
            }
            else
            {
                found = true;
            }

            if (found)
            {
                return new Token(TokenType, Match);
            }

            return null;
        }
        #endregion
    }
}