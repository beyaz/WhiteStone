using System;
using System.Linq.Expressions;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Defines the expression utility.
    /// </summary>
    public static class ExpressionUtility
    {
        #region Constants
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
        static string NameofAllPath(MemberExpression memberExpression)
        {
            if (memberExpression == null)
            {
                return null;
            }

            var left = NameofAllPath(memberExpression.Expression as MemberExpression);
            if (left == null)
            {
                return memberExpression.Member.Name;
            }

            return left + Dot + memberExpression.Member.Name;
        }
        #endregion
    }
}