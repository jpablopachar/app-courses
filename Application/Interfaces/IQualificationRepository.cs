using System.Linq.Expressions;
using Domain;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines the contract for qualification data access operations.
    /// </summary>
    public interface IQualificationRepository
    {
        /// <summary>
        /// Gets a queryable collection of qualifications with optional filtering and ordering.
        /// </summary>
        /// <param name="predicate">Optional filter predicate to apply to the qualifications.</param>
        /// <param name="orderBySelector">Optional property selector for ordering.</param>
        /// <param name="orderAscending">Determines the sort direction. True for ascending, false for descending.</param>
        /// <returns>A queryable collection of qualifications.</returns>
        IQueryable<Qualification> GetQualifications(
            Expression<Func<Qualification, bool>>? predicate = null,
            Expression<Func<Qualification, object>>? orderBySelector = null,
            bool orderAscending = true
        );
    }
}
