using System;
using System.Collections.Generic;

namespace CustomUIMarkupLanguage.Markup
{
    /// <summary>
    ///     The node
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets the UI.
        /// </summary>
        public string UI => Properties?["view"]?.ValueAsStringToUpperInEnglish;

        #region Public Properties
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the name to upper in english.
        /// </summary>
        public string NameToUpperInEnglish => Name.ToUpperEN();

        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        public NodeCollection Properties { get; set; }

        /// <summary>
        ///     Gets or sets the value as array.
        /// </summary>
        public NodeCollection ValueAsArray { get; set; }

        /// <summary>
        ///     Gets or sets the value as binding information.
        /// </summary>
        public BindingInfoContract ValueAsBindingInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [value as boolean].
        /// </summary>
        public bool ValueAsBoolean { get; set; }

        /// <summary>
        ///     Gets or sets the value as number.
        /// </summary>
        public decimal ValueAsNumber { get; set; }

        /// <summary>
        ///     Gets the value as number as double.
        /// </summary>
        public double ValueAsNumberAsDouble => Convert.ToDouble(ValueAsNumber);

        /// <summary>
        ///     Gets or sets the value as string.
        /// </summary>
        public string ValueAsString { get; set; }

        /// <summary>
        ///     Gets or sets the value as string to upper in english.
        /// </summary>
        public string ValueAsStringToUpperInEnglish => ValueAsString?.ToUpperEN();

        /// <summary>
        ///     Gets or sets a value indicating whether [value is array].
        /// </summary>
        public bool ValueIsArray { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [value is binding expression].
        /// </summary>
        public bool ValueIsBindingExpression { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [value is boolean].
        /// </summary>
        public bool ValueIsBoolean { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [value is number].
        /// </summary>
        public bool ValueIsNumber { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [value is string].
        /// </summary>
        public bool ValueIsString { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Adds the property.
        /// </summary>
        public void AddProperty(Node info)
        {
            if (Properties == null)
            {
                Properties = new NodeCollection();
            }

            Properties.Items.Add(info);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            const string Comma = ",";

            if (ValueIsString)
            {
                return $"{Name}:'{ValueAsString}'";
            }
            if (ValueAsBoolean )
            {
                return $"{Name}:{ValueAsBoolean}";
            }
            if ( ValueIsNumber )
            {
                return $"{Name}:{ValueAsNumber}";
            }
            if ( ValueIsBindingExpression)
            {
                return $"{Name}:{ValueAsBindingInfo}";
            }

            if (Properties?.Items?.Count > 0)
            {
                var list = new List<string>();
                foreach (var node in Properties.Items)
                {
                    list.Add(node.ToString());
                }

                var value = string.Join(" , ", list);

                return "{" + value + "}";
            }

            if (ValueIsArray)
            {
                var list = new List<string>();
                foreach (var node in ValueAsArray)
                {
                    list.Add(node.ToString());
                }

                var value = string.Join(Environment.NewLine + Comma, list);

                return $"{Name}:{value}";
            }

            throw new NotImplementedException(Name);
        }
        #endregion

        public bool HasProperty(string name)
        {

            return Properties?[name] != null;
        }
    }
}