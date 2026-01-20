using System.Linq.Expressions;
using Domain;

namespace Application.Interfaces
{
    /// <summary>
    /// Defines the contract for instructor data access operations.
    /// </summary>
    public interface IInstructorRepository
    {
        /// <summary>
        /// Gets a queryable collection of instructors with optional filtering and ordering.
        /// </summary>
        /// <param name="predicate">Optional filter predicate to apply to the instructors.</param>
        /// <param name="orderBySelector">Optional property selector for ordering.</param>
        /// <param name="orderAscending">Determines the sort direction. True for ascending, false for descending.</param>
        /// <returns>A queryable collection of instructors.</returns>
        IQueryable<Instructor> GetInstructors(
            Expression<Func<Instructor, bool>>? predicate = null,
            Expression<Func<Instructor, object>>? orderBySelector = null,
            bool orderAscending = true
        );
    }
}
