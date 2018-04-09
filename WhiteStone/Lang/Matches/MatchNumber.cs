using System.Text.RegularExpressions;
using Lang.Data;
using Lang.Lexers;

namespace Lang.Matches
{
    /// <summary>
    ///     The match number
    /// </summary>
    public class MatchNumber : MatcherBase
    {
        #region Methods
        /// <summary>
        ///     Determines whether [is match implementation] [the specified tokenizer].
        /// </summary>
        protected override Token IsMatchImpl(Tokenizer tokenizer)
        {
            var leftOperand = GetIntegers(tokenizer);

            if (leftOperand != null)
            {
                if (tokenizer.Current == ".")
                {
                    tokenizer.Consume();

                    var rightOperand = GetIntegers(tokenizer);

                    // found a float
                    if (rightOperand != null)
                    {
                        return new Token(TokenType.Float, leftOperand + "." + rightOperand);
                    }
                }

                return new Token(TokenType.Int, leftOperand);
            }

            return null;
        }

        /// <summary>
        ///     Gets the integers.
        /// </summary>
        string GetIntegers(Tokenizer tokenizer)
        {
            var regex = new Regex("[0-9]");

            string num = null;

            while (tokenizer.Current != null && regex.IsMatch(tokenizer.Current))
            {
                num += tokenizer.Current;
                tokenizer.Consume();
            }

            if (num != null)
            {
                return num;
            }

            return null;
        }
        #endregion
    }
}