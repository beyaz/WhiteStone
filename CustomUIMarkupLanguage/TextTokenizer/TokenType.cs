namespace CustomUIMarkupLanguage.TextTokenizer
{
    /// <summary>
    ///     The token type
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        ///     The binding
        /// </summary>
        Binding,

        /// <summary>
        ///     The mode
        /// </summary>
        Mode,

        /// <summary>
        ///     The converter
        /// </summary>
        Converter,

        /// <summary>
        ///     The two way
        /// </summary>
        TwoWay,

        /// <summary>
        ///     The left bracket
        /// </summary>
        LeftBracket,

        /// <summary>
        ///     The right bracket
        /// </summary>
        RightBracket,

        /// <summary>
        ///     The open parenthesis
        /// </summary>
        OpenParenthesis,

        /// <summary>
        ///     The close parenthesis
        /// </summary>
        CloseParenthesis,

        /// <summary>
        ///     The identifier
        /// </summary>
        Identifier,

        /// <summary>
        ///     The comma
        /// </summary>
        Comma,

        /// <summary>
        ///     The dot
        /// </summary>
        Dot,

        /// <summary>
        ///     The equals
        /// </summary>
        Equals,

        /// <summary>
        ///     The this
        /// </summary>
        This,

        /// <summary>
        ///     The not equals
        /// </summary>
        NotEquals,

        /// <summary>
        ///     The string value
        /// </summary>
        StringValue,

        /// <summary>
        ///     The sequence terminator
        /// </summary>
        SequenceTerminator,

        /// <summary>
        ///     The number value
        /// </summary>
        NumberValue
    }
}