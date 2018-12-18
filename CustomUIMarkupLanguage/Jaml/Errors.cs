using System;

namespace BOA.Jaml
{
    /// <summary>
    ///     Error codes in this type
    /// </summary>
    public static class Errors
    {
        /// <summary>
        ///     Types the not found.
        /// </summary>
        public static Exception TypeNotFound(string typeName)
        {
            return new ArgumentException(typeName + " not found ");
        }

        /// <summary>
        ///     Properties the not found.
        /// </summary>
        public static Exception PropertyNotFound(string propertyName, Type type)
        {
            return new ArgumentException(propertyName + " not found in " + type.FullName);
        }

        /// <summary>
        ///     Dependencies the property not found.
        /// </summary>
        public static Exception DependencyPropertyNotFound(string propertyName)
        {
            return new ArgumentException("DependencyPropertyNotFound:" + propertyName);
        }

        /// <summary>
        ///     Parsings the error.
        /// </summary>
        public static Exception ParsingError(string message)
        {
            return new ArgumentException("ParsingError:" + message);
        }

        /// <summary>
        ///     Methods the not found.
        /// </summary>
        public static Exception MethodNotFound(string methodName, Type type)
        {
            return new ArgumentException(methodName + " not found in " + type.FullName);
        }
    }
}