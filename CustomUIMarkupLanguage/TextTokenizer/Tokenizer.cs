using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomUIMarkupLanguage.TextTokenizer
{
    /// <summary>
    ///     The tokenizer
    /// </summary>
    public static class Tokenizer
    {
        #region Public Methods
        /// <summary>
        ///     Tokenizes the specified token definitions.
        /// </summary>
        public static IReadOnlyList<Token> Tokenize(IReadOnlyList<TokenDefinition> TokenDefinitions, string data)
        {
            var tokenDefinitions = TokenDefinitions;

            if (tokenDefinitions == null)
            {
                throw new ArgumentException(nameof(TokenDefinitions));
            }

            var tokenMatches = new List<TokenMatch>();

            foreach (var tokenDefinition in tokenDefinitions)
            {
                tokenMatches.AddRange(tokenDefinition.FindMatches(data));
            }

            var items = new List<Token>();

            var groupedByIndex = tokenMatches.GroupBy(x => x.StartIndex)
                                             .OrderBy(x => x.Key)
                                             .ToList();

            TokenMatch lastMatch = null;

            var len = groupedByIndex.Count;
            for (var i = 0; i < len; i++)
            {
                var bestMatch = groupedByIndex[i].OrderBy(x => x.Precedence).First();
                if (lastMatch != null && bestMatch.StartIndex < lastMatch.EndIndex)
                {
                    continue;
                }

                items.Add(new Token(bestMatch.TokenType, bestMatch.Value));

                lastMatch = bestMatch;
            }

            return items;
        }
        #endregion
    }
}