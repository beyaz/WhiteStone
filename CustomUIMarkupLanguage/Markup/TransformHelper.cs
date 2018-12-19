using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomUIMarkupLanguage.Markup
{
    /// <summary>
    ///     The transform helper
    /// </summary>
    public static class TransformHelper
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
                    Name = jProperty.Name
                };

                if (jProperty.Value.Type == JTokenType.String)
                {
                    propertyNode.ValueAsString = (string) ((JValue) jProperty.Value).Value;

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

            var rootNode = Parse(jObject);

            TransformBindingInformation(rootNode);

            return rootNode;
        }

        /// <summary>
        ///     Transforms the binding information.
        /// </summary>
        public static void TransformBindingInformation(Node root)
        {
            root.Visit(node =>
            {
                if (node.ValueIsString)
                {
                    var bindingInfoContract = BindingExpressionParser.TryParse(node.ValueAsString);
                    if (bindingInfoContract == null)
                    {
                        return;
                    }

                    node.ValueIsString = false;
                    node.ValueAsString = null;

                    node.ValueIsBindingExpression = true;
                    node.ValueAsBindingInfo       = bindingInfoContract;
                }
            });
        }

        public static void Visit(this Node root, Action<Node> onVisit)
        {
            if (root.Properties != null)
            {
                foreach (var item in root.Properties)
                {
                    Visit(item, onVisit);
                }
            }

            if (root.ValueIsArray)
            {
                foreach (var item in root.ValueAsArray)
                {
                    Visit(item, onVisit);
                }
            }

            onVisit(root);
        }
        #endregion
    }
}