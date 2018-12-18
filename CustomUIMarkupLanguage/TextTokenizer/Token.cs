namespace CustomUIMarkupLanguage.TextTokenizer
{
    /// <summary>
    ///     The token
    /// </summary>
    public class Token
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        public Token(TokenType tokenType)
        {
            TokenType = tokenType;
            Value     = string.Empty;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        public Token(TokenType tokenType, string value)
        {
            TokenType = tokenType;
            Value     = value;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the type of the token.
        /// </summary>
        public TokenType TokenType { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public string Value { get; set; }
        #endregion
    }
}