namespace BOA.TextTokenizer
{
    /// <summary>
    ///     The token match
    /// </summary>
    public class TokenMatch
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the end index.
        /// </summary>
        public int EndIndex { get; set; }

        /// <summary>
        ///     Gets or sets the precedence.
        /// </summary>
        public int Precedence { get; set; }

        /// <summary>
        ///     Gets or sets the start index.
        /// </summary>
        public int StartIndex { get; set; }

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