using System.Linq.Expressions;
using Application.Interfaces;
using Domain;
using Persistence;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implements instructor data access operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="InstructorRepository"/> class.
    /// </remarks>
    /// <param name="context">The database context.</param>
    public class InstructorRepository(AppCoursesDbContext context) : IInstructorRepository
    {
        private readonly AppCoursesDbContext _context = context;

        /// <inheritdoc/>
        public IQueryable<Instructor> GetInstructors(
            Expression<Func<Instructor, bool>>? predicate = null,
            Expression<Func<Instructor, object>>? orderBySelector = null,
            bool orderAscending = true
        )
        {
            IQueryable<Instructor> query = _context.Instructors!;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBySelector != null)
            {
                query = orderAscending
                    ? query.OrderBy(orderBySelector)
                    : query.OrderByDescending(orderBySelector);
            }

            return query;
        }
    }
}
