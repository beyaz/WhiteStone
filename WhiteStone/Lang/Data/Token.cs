namespace Lang.Data
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
        public Token(TokenType tokenType, string token)
        {
            TokenType  = tokenType;
            TokenValue = token;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Token" /> class.
        /// </summary>
        public Token(TokenType tokenType)
        {
            TokenValue = null;
            TokenType  = tokenType;
        }
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets the type of the token.
        /// </summary>
        public TokenType TokenType { get; private set; }

        /// <summary>
        ///     Gets the token value.
        /// </summary>
        public string TokenValue { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return TokenType + ": " + TokenValue;
        }
        #endregion
    }
}