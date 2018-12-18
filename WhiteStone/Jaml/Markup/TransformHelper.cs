using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BOA.Jaml.Markup
{
    /// <summary>
    ///     The node
    /// </summary>
    public class Node
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        public string NameToUpperInEnglish { get; set; }

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

        public double ValueAsNumberAsDouble => ValueAsNumber.ToDouble();

        /// <summary>
        ///     Gets or sets the value as string.
        /// </summary>
        public string ValueAsString { get; set; }

        public string ValueAsStringToUpperInEnglish { get; set; }

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
                return $"{{{Name}:'{ValueAsString}'}}";
            }

            if (ValueAsBoolean || ValueIsNumber || ValueIsBindingExpression)
            {
                return $"{{{Name}:{ValueAsBoolean}}}";
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

                return $"{{{Name}:{value}}}";
            }

            throw new NotImplementedException(Name);
        }
        #endregion
    }

    /// <summary>
    ///     The node collection
    /// </summary>
    public class NodeCollection : IEnumerable<Node>
    {
        #region Public Properties
        /// <summary>
        ///     Gets or sets the items.
        /// </summary>
        public List<Node> Items { get; set; } = new List<Node>();
        #endregion

        #region Public Indexers
        /// <summary>
        ///     Gets the <see cref="Node" /> with the specified name.
        /// </summary>
        public Node this[string name]
        {
            get { return Items.FirstOrDefault(x => x.Name == name); }
        }

        /// <summary>
        ///     Gets the <see cref="Node" /> at the specified index.
        /// </summary>
        public Node this[int index]
        {
            get { return Items[index]; }
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<Node> GetEnumerator()
        {
            return Items.GetEnumerator();
        }
        #endregion

        #region Explicit Interface Methods
        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

    /// <summary>
    ///     The transform helper
    /// </summary>
    public class TransformHelper
    {
        #region Public Methods
        /// <summary>
        ///     Transforms the specified node.
        /// </summary>
        public static Node Parse(JObject jObject)
        {
            var node = new Node();

            foreach (var jProperty in jObject.Properties())
            {
                var propertyNode = new Node
                {
                    Name                 = jProperty.Name,
                    NameToUpperInEnglish = jProperty.Name.ToUpperEN()
                };

                if (jProperty.Value.Type == JTokenType.String)
                {
                    propertyNode.ValueAsString = (string) ((JValue) jProperty.Value).Value;

                    propertyNode.ValueAsStringToUpperInEnglish = propertyNode.ValueAsString.ToUpperEN();

                    propertyNode.ValueIsString = true;

                    node.AddProperty(propertyNode);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Boolean)
                {
                    propertyNode.ValueAsBoolean = (bool) ((JValue) jProperty.Value).Value;

                    propertyNode.ValueIsBoolean = true;

                    node.AddProperty(propertyNode);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Float)
                {
                    propertyNode.ValueAsNumber = (decimal) (double) ((JValue) jProperty.Value).Value;

                    propertyNode.ValueIsNumber = true;

                    node.AddProperty(propertyNode);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Integer)
                {
                    propertyNode.ValueAsNumber = Convert.ToDecimal(((JValue) jProperty.Value).Value);

                    propertyNode.ValueIsNumber = true;

                    node.AddProperty(propertyNode);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Array)
                {
                    var nodeCollection = new NodeCollection();

                    foreach (var jToken in jProperty.Value.ToArray())
                    {
                        var jObj = jToken as JObject;
                        if (jObj == null)
                        {
                            throw new ArgumentException(jToken.ToString());
                        }

                        nodeCollection.Items.Add(Parse(jObj));
                    }

                    propertyNode.ValueAsArray = nodeCollection;

                    propertyNode.ValueIsArray = true;

                    node.AddProperty(propertyNode);

                    continue;
                }

                throw new ArgumentException();
            }

            return node;
        }

        /// <summary>
        ///     Transforms the specified json.
        /// </summary>
        public static Node Transform(string json)
        {
            JObject jObject;
            try
            {
                jObject = (JObject) JsonConvert.DeserializeObject(json);
            }
            catch (Exception)
            {
                throw new ArgumentException("json parse error.");
            }

            return TransformBindingInformation(Parse(jObject));
        }

        /// <summary>
        ///     Transforms the binding information.
        /// </summary>
        public static Node TransformBindingInformation(Node node)
        {
            foreach (var item in node.Properties.Items)
            {
                if (item.ValueIsString)
                {
                    var bindingInfoContract = BindingExpressionParser.TryParse(item.ValueAsString);
                    if (bindingInfoContract == null)
                    {
                        continue;
                    }

                    item.ValueIsString = false;
                    item.ValueAsString = null;

                    item.ValueIsBindingExpression = true;
                    item.ValueAsBindingInfo       = bindingInfoContract;
                    continue;
                }

                if (item.ValueIsArray)
                {
                    foreach (var info in item.ValueAsArray)
                    {
                        TransformBindingInformation(info);
                    }
                }
            }

            return node;
        }
        #endregion
    }
}