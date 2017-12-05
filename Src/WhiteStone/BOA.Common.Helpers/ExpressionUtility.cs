using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BOA.Common.Helpers
{
    /// <summary>
    ///     Defines the expression utility.
    /// </summary>
    public static class ExpressionUtility
    {
        #region Constants
        /// <summary>
        ///     The dot
        /// </summary>
        const char Dot = '.';
        #endregion

        #region Public Methods
        /// <summary>
        ///     Returns accesses the path of given expression.
        /// </summary>
        public static string AccessPathOf<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
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

            path.Reverse();

            return string.Join(Dot.ToString(), path);
        }
        #endregion
    }
}