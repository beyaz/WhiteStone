using System;

namespace BOA.Data.Attributes
{
    /// <summary>
    ///     Indicate column is primary key.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbPrimaryKeyAttribute : Attribute
    {
    }
}