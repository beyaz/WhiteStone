using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CustomUIMarkupLanguage.TextTokenizer
{
    /// <summary>
    ///     The token definition
    /// </summary>
    public class TokenDefinition
    {
        #region Fields
        /// <summary>
        ///     The precedence
        /// </summary>
        readonly int _precedence;

        /// <summary>
        ///     The regex
        /// </summary>
        readonly Regex _regex;

        /// <summary>
        ///     The token type
        /// </summary>
        readonly TokenType _tokenType;
        #endregion

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="TokenDefinition" /> class.
        /// </summary>
        public TokenDefinition(TokenType tokenType, string regexPattern, int precedence)
        {
            _regex      = new Regex(regexPattern, RegexOptions.IgnoreCase /*| RegexOptions.Compiled*/);
            _tokenType  = tokenType;
            _precedence = precedence;
        }

        public TokenDefinition(TokenType tokenType, Regex regex, int precedence)
        {
            _regex      = regex;
            _tokenType  = tokenType;
            _precedence = precedence;
        }
        #endregion

        #region Public Properties
        public TokenType TokenType => _tokenType;
        #endregion

        #region Public Methods
        /// <summary>
        ///     Finds the matches.
        /// </summary>
        public IReadOnlyList<TokenMatch> FindMatches(string inputString)
        {
            var matches     = _regex.Matches(inputString);
            var len         = matches.Count;
            var findMatches = new List<TokenMatch>();
            for (var i = 0; i < len; i++)
            {
                var match = matches[i];

                findMatches.Add(new TokenMatch
                {
                    StartIndex = match.Index,
                    EndIndex   = match.Index + match.Length,
                    TokenType  = _tokenType,
                    Value      = match.Value,
                    Precedence = _precedence
                });
            }

            return findMatches;
        }
        #endregion
    }
}