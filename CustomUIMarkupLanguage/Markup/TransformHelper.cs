using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CustomUIMarkupLanguage.Markup
{
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

            return TransformViewName(TransformBindingInformation(Parse(jObject)));
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

        /// <summary>
        ///     Transforms the name of the view.
        /// </summary>
        public static Node TransformViewName(Node node)
        {
            if (node.Properties == null)
            {
                return node;
            }

            var ui = node.Properties["UI"];
            if (ui != null)
            {
                ui.NameToUpperInEnglish = "VIEW";
                ui.Name                 = "view";
            }

            foreach (var item in node.Properties)
            {
                if (item.ValueIsArray)
                {
                    foreach (var info in item.ValueAsArray)
                    {
                        TransformViewName(info);
                    }
                }
            }

            return node;
        }
        #endregion
    }
}