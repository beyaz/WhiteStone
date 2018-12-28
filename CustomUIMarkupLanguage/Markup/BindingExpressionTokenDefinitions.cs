using System.Collections.Generic;
using System.Text.RegularExpressions;
using CustomUIMarkupLanguage.TextTokenizer;

namespace CustomUIMarkupLanguage.Markup
{
    /// <summary>
    ///     The binding expression token definitions
    /// </summary>
    class BindingExpressionTokenDefinitions
    {
        #region Static Fields
        /// <summary>
        ///     Gets the value.
        /// </summary>
        public static IReadOnlyList<TokenDefinition> Value = new List<TokenDefinition>
        {
            //new TokenDefinition(TokenType.Binding, "binding", 1),
            //new TokenDefinition(TokenType.Mode, "mode", 1),
            //new TokenDefinition(TokenType.Converter, "converter", 1),
            //new TokenDefinition(TokenType.TwoWay, "twoway", 1),
            new TokenDefinition(TokenType.LeftBracket, "\\{", 1),
            new TokenDefinition(TokenType.RightBracket, "\\}", 1),
            new TokenDefinition(TokenType.OpenParenthesis, "\\(", 1),
            new TokenDefinition(TokenType.CloseParenthesis, "\\)", 1),

            new TokenDefinition(TokenType.Equals, "=", 1),
            new TokenDefinition(TokenType.This, "this", 1),
            new TokenDefinition(TokenType.NotEquals, "!=", 1),
            // new TokenDefinition(TokenType.Identifier, "[a-zA-Z_$][a-zA-Z0-9_$]*", 1),
            new TokenDefinition(TokenType.Identifier, new Regex("[a-zA-Z_$][a-zA-Z0-9_$]*"), 1),

            // new TokenDefinition(TokenType.StringValue, "'([^']*)'", 1),
            new TokenDefinition(TokenType.NumberValue, "\\d+", 1),

            new TokenDefinition(TokenType.Comma, ",", 1),
            new TokenDefinition(TokenType.Dot, ".", 1)
        };
        #endregion
    }
}