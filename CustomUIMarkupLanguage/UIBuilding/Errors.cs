using System;
using System.Windows;
using CustomUIMarkupLanguage.Markup;

namespace CustomUIMarkupLanguage.UIBuilding
{
    /// <summary>
    ///     Error codes in this type
    /// </summary>
    static class Errors
    {
        #region Public Methods
        /// <summary>
        ///     Attributes the value is invalid.
        /// </summary>
        public static Exception AttributeValueIsInvalid(Node node)
        {
            return new ArgumentException(nameof(AttributeValueIsInvalid) + node);
        }

        /// <summary>
        ///     Dependencies the property not found.
        /// </summary>
        public static Exception DependencyPropertyNotFound(string propertyName)
        {
            return new ArgumentException("DependencyPropertyNotFound:" + propertyName);
        }

        /// <summary>
        ///     Elements the creation failed exception.
        /// </summary>
        public static Exception ElementCreationFailedException(Node node)
        {
            return new ArgumentException(node.ToString());
        }

        /// <summary>
        ///     Elements the must be inherit from i add child.
        /// </summary>
        public static Exception ElementMustBeInheritFromIAddChild(UIElement element)
        {
            return new ArgumentException(nameof(ElementMustBeInheritFromIAddChild) + element.GetType().FullName);
        }

        /// <summary>
        ///     Jsons the parsing error.
        /// </summary>
        public static Exception JsonParsingError(Exception exception)
        {
            return new ArgumentException("json parse error.", exception);
        }

        /// <summary>
        ///     Methods the not found.
        /// </summary>
        public static Exception MethodNotFound(string methodName, Type type)
        {
            return new ArgumentException(methodName + " not found in " + type.FullName);
        }

        /// <summary>
        ///     Properties the not found.
        /// </summary>
        public static Exception PropertyNotFound(string propertyName, Type type)
        {
            return new ArgumentException(propertyName + " not found in " + type.FullName);
        }

        /// <summary>
        ///     Types the not found.
        /// </summary>
        public static Exception TypeNotFound(string typeName)
        {
            return new ArgumentException(typeName + " not found ");
        }
        #endregion
    }
}