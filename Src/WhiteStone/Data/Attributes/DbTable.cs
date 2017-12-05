using System;

namespace BOA.Data.Attributes
{
    /// <summary>
    ///     Defines database table name.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DbTableAttribute : Attribute
    {
        readonly string _name;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbTableAttribute" /> class.
        /// </summary>
        /// <param name="name">Name of the table.</param>
        public DbTableAttribute(string name)
        {
            _name = name;
        }
    }
}