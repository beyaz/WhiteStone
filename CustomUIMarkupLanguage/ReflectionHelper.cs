﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CustomUIMarkupLanguage
{
    /// <summary>
    ///     The reflection helper
    /// </summary>
    public static class ReflectionHelper
    {
        #region Public Methods
        /// <summary>
        ///     Gets the field.
        /// </summary>
        public static FieldInfo GetField(this Type type, string fieldName, bool? isPublic, bool? isStatic, bool ignoreCase, bool throwExceptionOnNotFound)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (fieldName == null)
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            FieldInfo fieldInfo = null;

            var fields = type.GetFields(GetFlag(isPublic, isStatic));

            if (ignoreCase)
            {
                fieldInfo = fields.FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                fieldInfo = fields.FirstOrDefault(p => p.Name == fieldName);
            }

            if (fieldInfo == null && throwExceptionOnNotFound)
            {
                throw CreateMissingMemberException(type, fieldName);
            }

            return fieldInfo;
        }

        /// <summary>
        ///     Gets the get method.
        /// </summary>
        public static MethodInfo GetMethod(this Type type, string methodName, bool? isPublic, bool? isStatic, bool ignoreCase, bool throwExceptionOnNotFound)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            MethodInfo methodInfo = null;

            var methods = type.GetMethods(GetFlag(isPublic, isStatic));

            if (ignoreCase)
            {
                methodInfo = methods.FirstOrDefault(p => p.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                methodInfo = methods.FirstOrDefault(p => p.Name == methodName);
            }

            if (methodInfo == null && throwExceptionOnNotFound)
            {
                throw CreateMissingMemberException(type, methodName);
            }

            return methodInfo;
        }

        /// <summary>
        ///     Gets the property.
        /// </summary>
        public static PropertyInfo GetProperty(this Type type, string propertyName, bool? isPublic, bool? isStatic, bool ignoreCase, bool throwExceptionOnNotFound)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyInfo propertyInfo = null;

            var fields = type.GetProperties(GetFlag(isPublic, isStatic));

            if (ignoreCase)
            {
                propertyInfo = fields.FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                propertyInfo = fields.FirstOrDefault(p => p.Name == propertyName);
            }

            if (propertyInfo == null && throwExceptionOnNotFound)
            {
                throw CreateMissingMemberException(type, propertyName);
            }

            return propertyInfo;
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Creates the missing member exception.
        /// </summary>
        static MissingMemberException CreateMissingMemberException(Type type, string memberName)
        {
            throw new MissingMemberException(type.FullName + Path.VolumeSeparatorChar + Path.VolumeSeparatorChar + memberName);
        }

        /// <summary>
        ///     Gets the flag.
        /// </summary>
        static BindingFlags GetFlag(bool? isPublic, bool? isStatic)
        {
            var flags = BindingFlags.Default;

            if (isPublic == null)
            {
                flags |= BindingFlags.Public | BindingFlags.NonPublic;
            }
            else if (isPublic == true)
            {
                flags |= BindingFlags.Public;
            }
            else
            {
                flags |= BindingFlags.NonPublic;
            }

            if (isStatic == null)
            {
                flags |= BindingFlags.Instance | BindingFlags.Static;
            }
            else if (isStatic == true)
            {
                flags |= BindingFlags.Static;
            }
            else
            {
                flags |= BindingFlags.Instance;
            }

            return flags;
        }
        #endregion
    }
}