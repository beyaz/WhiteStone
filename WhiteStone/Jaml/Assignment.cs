
namespace BOA.Jaml
{
    /// <summary>
    ///     Defines the assignment.
    /// </summary>
    public class Assignment
    {
        /// <summary>
        ///     Gets the builder.
        /// </summary>
        public Builder Builder { get; internal set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///     Gets the value as string.
        /// </summary>
        public string ValueAsString => Value as string;

        /// <summary>
        /// Gets the value as boolean.
        /// </summary>
        public bool? ValueAsBoolean => Value as bool?;

        /// <summary>
        /// Gets the value as string to upper in english.
        /// </summary>
        public string ValueAsStringToUpperInEnglish => ValueAsString?.ToUpperEN();

        /// <summary>
        ///     Gets the value to double.
        /// </summary>
        public double ValueToDouble
        {
            get { return Value.ToDouble(); }
        }

        /// <summary>
        ///     Gets the property name to upper in english.
        /// </summary>
        public string NameToUpperInEnglish
        {
            get { return Name.ToUpperEN(); }
        }
    }
}