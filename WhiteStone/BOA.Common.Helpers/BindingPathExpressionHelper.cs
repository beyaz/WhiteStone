using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     The binding path expression helper
    /// </summary>
    public static class BindingPathExpressionHelper
    {
        #region Public Methods
        /// <summary>
        ///     Gets the binding path.
        /// </summary>
        public static string GetBindingPath<T>(this Expression<Func<T>> propertyAccessor)
        {
            var memberExpression = propertyAccessor.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new ArgumentException(propertyAccessor.ToString());
            }

            return NameofAllPath(memberExpression);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Nameofs all path.
        /// </summary>
        static string NameofAllPath(MemberExpression memberExpression)
        {
            var path = new List<string>();

            while (memberExpression != null)
            {
                path.Add(memberExpression.Member.Name);

                memberExpression = memberExpression.Expression as MemberExpression;
            }

            if (path.Count == 0)
            {
                return null;
            }

            path.RemoveAt(path.Count - 1);

            path.Reverse();

            const string Separator = ".";

            return string.Join(Separator, path);
        }
        #endregion
    }
}