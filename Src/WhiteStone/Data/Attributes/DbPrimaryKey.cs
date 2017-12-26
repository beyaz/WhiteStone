using System;

namespace BOA.Data.Attributes
{
    /// <summary>
    /// Indicate column is primary key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbPrimaryKeyAttribute : Attribute
    {
    }

    /// <summary>
    /// Indicate column is primary key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DbIdentityColumnAttribute : Attribute
    {
    }
}