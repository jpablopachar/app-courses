using System.Linq.Expressions;

namespace Application.Core
{
    /// <summary>
    /// Provides utility methods for building and combining LINQ expressions.
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// Creates a new expression that always evaluates to true.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expression.</typeparam>
        /// <returns>An expression that always returns true.</returns>
        public static Expression<Func<T, bool>> New<T>()
        {
            return x => true;
        }

        /// <summary>
        /// Returns the provided expression.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expression.</typeparam>
        /// <param name="expression">The expression to return.</param>
        /// <returns>The provided expression.</returns>
        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> expression)
        {
            return expression;
        }

        /// <summary>
        /// Combines two expressions with a logical AND operation.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
        /// <param name="left">The first expression.</param>
        /// <param name="right">The second expression.</param>
        /// <returns>An expression representing the logical AND of the two expressions.</returns>
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(
                    left.Body,
                    Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
        }

        /// <summary>
        /// Combines two expressions with a logical OR operation.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
        /// <param name="left">The first expression.</param>
        /// <param name="right">The second expression.</param>
        /// <returns>An expression representing the logical OR of the two expressions.</returns>
        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right)
        {
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(
                    left.Body,
                    Expression.Invoke(right, left.Parameters[0])), left.Parameters[0]);
        }
    }
}