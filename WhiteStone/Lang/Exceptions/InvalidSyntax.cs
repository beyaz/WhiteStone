using System;

namespace Lang.Exceptions
{
    /// <summary>
    ///     The invalid syntax
    /// </summary>
    public class InvalidSyntax : Exception
    {
        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the <see cref="InvalidSyntax" /> class.
        /// </summary>
        public InvalidSyntax(string format) : base(format)
        {
        }
        #endregion
    }
}