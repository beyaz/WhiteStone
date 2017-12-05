using System;

namespace BOA.Data.Attributes
{
    /// <summary>
    ///     Indicates property is related with database table column.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbColumnAttribute : Attribute
    {

    }
}