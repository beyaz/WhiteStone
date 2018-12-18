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
        ///     Gets or sets the children.
        /// </summary>
        public NodeCollection Children { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the properties.
        /// </summary>
        public NodeCollection Properties { get; set; }

        /// <summary>
        ///     Gets or sets the value as array.
        /// </summary>
        public List<Node> ValueAsArray { get; set; }

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
        ///     Gets or sets the value as string.
        /// </summary>
        public string ValueAsString { get; set; }

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
        ///     Adds the child.
        /// </summary>
        public void AddChild(Node info)
        {
            if (Children == null)
            {
                Children = new NodeCollection();
            }

            Children.Items.Add(info);
        }

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
        public static void Transform(Node node, JObject jObject)
        {
            foreach (var jProperty in jObject.Properties())
            {
                var attribute = new Node
                {
                    Name = jProperty.Name
                };

                if (jProperty.Value.Type == JTokenType.String)
                {
                    attribute.ValueAsString = (string) ((JValue) jProperty.Value).Value;

                    attribute.ValueIsString = true;

                    node.AddProperty(attribute);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Boolean)
                {
                    attribute.ValueAsBoolean = (bool) ((JValue) jProperty.Value).Value;

                    attribute.ValueIsBoolean = true;

                    node.AddProperty(attribute);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Float)
                {
                    attribute.ValueAsNumber = (decimal) (float) ((JValue) jProperty.Value).Value;

                    attribute.ValueIsNumber = true;

                    node.AddProperty(attribute);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Integer)
                {
                    attribute.ValueAsNumber = (int) ((JValue) jProperty.Value).Value;

                    attribute.ValueIsNumber = true;

                    node.AddProperty(attribute);
                    continue;
                }

                if (jProperty.Value.Type == JTokenType.Array)
                {
                    var items = new List<Node>();

                    foreach (var jToken in jProperty.Value.ToArray())
                    {
                        var jObj = jToken as JObject;
                        if (jObj == null)
                        {
                            throw new ArgumentException(jToken.ToString());
                        }

                        var attributeInfo = new Node();

                        Transform(attributeInfo, jObj);

                        items.Add(attributeInfo);
                    }

                    attribute.ValueAsArray = items;

                    attribute.ValueIsArray = true;

                    continue;
                }

                throw new ArgumentException();
            }
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

            var node = new Node();

            Transform(node, jObject);

            return node;
        }

        /// <summary>
        ///     Transforms the binding information.
        /// </summary>
        public static void TransformBindingInformation(Node node)
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
        }
        #endregion
    }
}