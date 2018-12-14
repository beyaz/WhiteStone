namespace BOA.Lang.Data
{
    /// <summary>
    ///     The token type
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        ///     The infer
        /// </summary>
        Infer,

        /// <summary>
        ///     The void
        /// </summary>
        Void,

        /// <summary>
        ///     The white space
        /// </summary>
        WhiteSpace,

        /// <summary>
        ///     The l bracket
        /// </summary>
        LBracket,

        /// <summary>
        ///     The r bracket
        /// </summary>
        RBracket,

        /// <summary>
        ///     The plus
        /// </summary>
        Plus,

        /// <summary>
        ///     The minus
        /// </summary>
        Minus,

        /// <summary>
        ///     The equals
        /// </summary>
        Equals,

        /// <summary>
        ///     The hash tag
        /// </summary>
        HashTag,

        /// <summary>
        ///     The quoted string
        /// </summary>
        QuotedString,

        /// <summary>
        ///     The word
        /// </summary>
        Word,

        /// <summary>
        ///     The comma
        /// </summary>
        Comma,

        /// <summary>
        ///     The open parenth
        /// </summary>
        OpenParenth,

        /// <summary>
        ///     The close parenth
        /// </summary>
        CloseParenth,

        /// <summary>
        ///     The asterix
        /// </summary>
        Asterix,

        /// <summary>
        ///     The slash
        /// </summary>
        Slash,

        /// <summary>
        ///     The carat
        /// </summary>
        Carat,

        /// <summary>
        ///     The de reference
        /// </summary>
        DeRef,

        /// <summary>
        ///     The ampersand
        /// </summary>
        Ampersand,

        /// <summary>
        ///     The fun
        /// </summary>
        Fun,

        /// <summary>
        ///     The greater than
        /// </summary>
        GreaterThan,

        /// <summary>
        ///     The less than
        /// </summary>
        LessThan,

        /// <summary>
        ///     The semi colon
        /// </summary>
        SemiColon,

        /// <summary>
        ///     If
        /// </summary>
        If,

        /// <summary>
        ///     The return
        /// </summary>
        Return,

        /// <summary>
        ///     The while
        /// </summary>
        While,

        /// <summary>
        ///     The else
        /// </summary>
        Else,

        /// <summary>
        ///     The scope start
        /// </summary>
        ScopeStart,

        /// <summary>
        ///     The EOF
        /// </summary>
        EOF,

        /// <summary>
        ///     For
        /// </summary>
        For,

        /// <summary>
        ///     The float
        /// </summary>
        Float,

        /// <summary>
        ///     The print
        /// </summary>
        Print,

        /// <summary>
        ///     The dot
        /// </summary>
        Dot,

        /// <summary>
        ///     The true
        /// </summary>
        True,

        /// <summary>
        ///     The false
        /// </summary>
        False,

        /// <summary>
        ///     The boolean
        /// </summary>
        Boolean,

        /// <summary>
        ///     The or
        /// </summary>
        Or,

        /// <summary>
        ///     The int
        /// </summary>
        Int,

        /// <summary>
        ///     The double
        /// </summary>
        Double,

        /// <summary>
        ///     The string
        /// </summary>
        String,

        /// <summary>
        ///     The method
        /// </summary>
        Method,

        /// <summary>
        ///     The class
        /// </summary>
        Class,

        /// <summary>
        ///     The new
        /// </summary>
        New,

        /// <summary>
        ///     The compare
        /// </summary>
        Compare,

        /// <summary>
        ///     The nil
        /// </summary>
        Nil,

        /// <summary>
        ///     The not compare
        /// </summary>
        NotCompare,

        /// <summary>
        ///     The try
        /// </summary>
        Try,

        /// <summary>
        ///     The catch
        /// </summary>
        Catch,

        /// <summary>
        ///     The l square bracket
        /// </summary>
        LSquareBracket,

        /// <summary>
        ///     The r square bracket
        /// </summary>
        RSquareBracket
    }
}