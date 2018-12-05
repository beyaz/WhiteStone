using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The XML helper.
    /// </summary>
    public static class XmlHelper
    {
        #region Public Methods
        /// <summary>
        ///     Adds the attribute.
        /// </summary>
        public static void AddAttribute(XElement xElement, string search, string name, string value)
        {
            var tags = search.Split('/').Where(x => string.IsNullOrWhiteSpace(x) == false).ToList();

            var len = tags.Count;

            for (var i = 0; i < len; i++)
            {
                var isLast = i == len - 1;

                var tag               = tags[i];
                var indexOfAssignment = tag.IndexOf('=');

                string filterAttributeName  = null;
                string filterAttributeValue = null;

                if (indexOfAssignment > 0)
                {
                    var start = tag.IndexOf('[');
                    var end   = tag.IndexOf(']');

                    filterAttributeName  = tag.Substring(start + 1, end - start - 1).Trim();
                    filterAttributeValue = tag.Substring(indexOfAssignment + 1, tag.Length - indexOfAssignment - 1).Trim();
                    tag                  = tag.Substring(0, start).Trim();
                }

                var xName = xElement.Name.Namespace + tag;

                if (IsMatch(xElement, tag, filterAttributeName, filterAttributeValue))
                {
                    if (isLast)
                    {
                        xElement.SetAttributeValue(XName.Get(name), value);
                    }
                    continue;
                }

                var element = xElement.Descendants(xName).FirstOrDefault();
                if (element == null)
                {
                    element = new XElement(XName.Get(tag));
                    xElement.Add(element);
                    if (filterAttributeName != null)
                    {
                        element.SetAttributeValue(filterAttributeName, filterAttributeValue);
                    }
                }

                if (!isLast)
                {
                    xElement = element;
                    continue;
                }

                element.SetAttributeValue(XName.Get(name), value);
            }
        }

        /// <summary>
        ///     Clears the XML.
        /// </summary>
        public static string ClearXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true
                //Indent = false,
                //NewLineOnAttributes = false
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Fors the own and child nodes.
        /// </summary>
        public static void ForOwnAndChildNodes(this XmlNode node, Action<XmlNode> action)
        {
            if (!node.HasChildNodes)
            {
                action(node);
                return;
            }

            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                ForOwnAndChildNodes(xmlNode, action);
            }
        }

        /// <summary>
        ///     Gets all namespaces.
        /// </summary>
        public static IDictionary<string, string> GetAllNamespaces(XmlDocument xDoc)
        {
            var result = new XmlNamespaceManager(xDoc.NameTable);

            var xNav = xDoc.CreateNavigator();
            while (xNav.MoveToFollowing(XPathNodeType.Element))
            {
                var localNamespaces = xNav.GetNamespacesInScope(XmlNamespaceScope.Local);
                if (localNamespaces == null)
                {
                    continue;
                }

                foreach (var localNamespace in localNamespaces)
                {
                    var prefix = localNamespace.Key;
                    if (string.IsNullOrEmpty(prefix))
                    {
                        prefix = "DEFAULT";
                    }

                    result.AddNamespace(prefix, localNamespace.Value);
                }
            }

            return result.GetNamespacesInScope(XmlNamespaceScope.All);
        }

        /// <summary>
        ///     Gets the root node.
        /// </summary>
        public static XmlNode GetRootNode(string xml)
        {
            var document = new XmlDocument();
            document.LoadXml(xml);
            return document.FirstChild;
        }

        /// <summary>
        ///     Pretties the XML.
        /// </summary>
        public static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration  = true,
                Indent              = true,
                NewLineOnAttributes = true
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///     Removes from parent.
        /// </summary>
        public static void RemoveFromParent(this XmlNode node)
        {
            if (node.ParentNode == null)
            {
                throw new InvalidOperationException(nameof(node.ParentNode) + " is null.");
            }

            node.ParentNode.RemoveChild(node);
        }

        /// <summary>
        ///     To the list.
        /// </summary>
        public static List<XmlNode> ToList(this XmlNodeList nodeList)
        {
            var xmlNodes = new List<XmlNode>();

            foreach (XmlNode xmlNode in nodeList)
            {
                xmlNodes.Add(xmlNode);
            }

            return xmlNodes;
        }
        #endregion

        #region Methods
        static bool IsMatch(XElement element, string tag, string hasAttributeName, string hasAttributeValue)
        {
            if (element.Name.LocalName != tag)
            {
                return false;
            }

            if (hasAttributeName == null)
            {
                return true;
            }

            var attributeName = XName.Get(hasAttributeName);
            return element.Attribute(attributeName)?.Value == hasAttributeValue;
        }
        #endregion
    }
}