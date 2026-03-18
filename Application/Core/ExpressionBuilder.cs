using System.Linq.Expressions;

namespace Application.Core
{
    /// <summary>
    /// Proporciona métodos utilitarios para construir y combinar expresiones LINQ.
    /// </summary>
    public static class ExpressionBuilder
    {
        /// <summary>
        /// Crea una nueva expresión que siempre evalúa a verdadero.
        /// </summary>
        /// <typeparam name="T">El tipo del parámetro en la expresión.</typeparam>
        /// <returns>Una expresión que siempre retorna verdadero.</returns>
        public static Expression<Func<T, bool>> New<T>()
        {
            return x => true;
        }

        /// <summary>
        /// Retorna la expresión proporcionada.
        /// </summary>
        /// <typeparam name="T">El tipo del parámetro en la expresión.</typeparam>
        /// <param name="expression">La expresión a retornar.</param>
        /// <returns>La expresión proporcionada.</returns>
        public static Expression<Func<T, bool>> New<T>(Expression<Func<T, bool>> expression)
        {
            return expression;
        }

        /// <summary>
        /// Combina dos expresiones con una operación lógica AND.
        /// </summary>
        /// <typeparam name="T">El tipo del parámetro en las expresiones.</typeparam>
        /// <param name="left">La primera expresión.</param>
        /// <param name="right">La segunda expresión.</param>
        /// <returns>Una expresión que representa el AND lógico de las dos expresiones.</returns>
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
        /// Combina dos expresiones con una operación lógica OR.
        /// </summary>
        /// <typeparam name="T">El tipo del parámetro en las expresiones.</typeparam>
        /// <param name="left">La primera expresión.</param>
        /// <param name="right">La segunda expresión.</param>
        /// <returns>Una expresión que representa el OR lógico de las dos expresiones.</returns>
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