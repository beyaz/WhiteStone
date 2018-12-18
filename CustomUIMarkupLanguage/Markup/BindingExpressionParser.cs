using System;
using System.Collections.Generic;
using System.Windows.Data;
using BOA.TextTokenizer;
using CustomUIMarkupLanguage;

namespace BOA.Jaml.Markup
{
    /// <summary>
    ///     The binding expression parser
    /// </summary>
    class BindingExpressionParser
    {
        #region Public Methods
        /// <summary>
        ///     Tries the parse.
        /// </summary>
        public static BindingInfoContract TryParse(string value)
        {
            if (value == null)
            {
                return null;
            }

            value = value.Trim();

            if (value.StartsWith("{") == false)
            {
                return null;
            }

            if (value.EndsWith("}") == false)
            {
                return null;
            }

            string sourcePath            = null;
            var    bindingMode           = BindingMode.TwoWay;
            string converterTypeFullName = null;
            string converterParameter    = null;

            var tokens = Tokenizer.Tokenize(BindingExpressionTokenDefinitions.Value, value);
            var len    = tokens.Count;
            for (var i = 0; i < len; i++)
            {
                var token = tokens[i];

                if (token.Value.ToUpperEN() == "BINDING" || token.Value == " ")
                {
                    continue;
                }

                if (sourcePath == null && token.TokenType == TokenType.Identifier)
                {
                    sourcePath = ReadPath(tokens, ref i);

                    continue;
                }

                if (token.Value.ToUpperEN() == "MODE")
                {
                    i++; // skip mode

                    SkipAssignmentAndSpace(tokens, ref i);

                    Enum.TryParse(tokens[i].Value, out bindingMode);
                    continue;
                }

                if (token.Value.ToUpperEN() == "CONVERTERPARAMETER")
                {
                    i++; // skip converterparameter
                    SkipAssignmentAndSpace(tokens, ref i);

                    converterParameter = ReadConverterParameter(tokens, ref i);
                    converterParameter = converterParameter.Trim();

                    if (converterParameter.StartsWith("'") &&
                        converterParameter.EndsWith("'"))
                    {
                        converterParameter = converterParameter.RemoveFromStart("'").RemoveFromEnd("'");
                    }

                    continue;
                }

                if (token.Value.ToUpperEN() == "CONVERTER")
                {
                    i++; // skip converter

                    SkipAssignmentAndSpace(tokens, ref i);

                    converterTypeFullName = ReadPath(tokens, ref i);
                }
            }

            return new BindingInfoContract
            {
                SourcePath            = sourcePath,
                BindingMode           = bindingMode,
                ConverterTypeFullName = converterTypeFullName,
                ConverterParameter    = converterParameter
            };
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Reads the converter parameter.
        /// </summary>
        static string ReadConverterParameter(IReadOnlyList<Token> tokens, ref int i)
        {
            var len = tokens.Count;

            var path = "";
            while (i < len)
            {
                var token = tokens[i];

                if (token.TokenType == TokenType.Comma ||
                    token.TokenType == TokenType.RightBracket)
                {
                    i--;
                    break;
                }

                path += token.Value;
                i++;
            }

            return path;
        }

        /// <summary>
        ///     Reads the path.
        /// </summary>
        static string ReadPath(IReadOnlyList<Token> tokens, ref int i)
        {
            var path = "";
            while (true)
            {
                var token = tokens[i];

                if (token.Value == " ")
                {
                    i++;
                    continue;
                }

                if (token.TokenType == TokenType.Identifier ||
                    token.TokenType == TokenType.Dot)
                {
                    path += token.Value;
                    i++;
                }
                else
                {
                    i--;
                    break;
                }
            }

            return path;
        }

        /// <summary>
        ///     Skips the assignment and space.
        /// </summary>
        static void SkipAssignmentAndSpace(IReadOnlyList<Token> tokens, ref int i)
        {
            var len = tokens.Count;

            while (i < len)
            {
                var token = tokens[i];

                if (token.Value == "=" ||
                    token.Value == " ")
                {
                    i++;
                    continue;
                }

                return;
            }
        }
        #endregion
    }
}