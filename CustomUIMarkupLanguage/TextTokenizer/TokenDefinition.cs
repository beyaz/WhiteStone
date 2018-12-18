﻿using System.Collections.Generic;
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
        #endregion

        #region Public Methods
        /// <summary>
        ///     Finds the matches.
        /// </summary>
        public IEnumerable<TokenMatch> FindMatches(string inputString)
        {
            var matches = _regex.Matches(inputString);
            var len     = matches.Count;
            for (var i = 0; i < len; i++)
            {
                var match = matches[i];

                yield return new TokenMatch
                {
                    StartIndex = match.Index,
                    EndIndex   = match.Index + match.Length,
                    TokenType  = _tokenType,
                    Value      = match.Value,
                    Precedence = _precedence
                };
            }
        }
        #endregion
    }
}