using System;

namespace CustomUIMarkupLanguage.Jaml
{
    /// <summary>
    ///     Defines the i type finder.
    /// </summary>
    public interface ITypeFinder
    {
        /// <summary>
        ///     Finds the specified type full name.
        /// </summary>
        Type Find(string typeFullName);
    }
}