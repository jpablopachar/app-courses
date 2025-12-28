using System.Linq.Expressions;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implements qualification data access operations.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="QualificationRepository"/> class.
    /// </remarks>
    /// <param name="context">The database context.</param>
    public class QualificationRepository(AppCoursesDbContext context) : IQualificationRepository
    {
        private readonly AppCoursesDbContext _context = context;

        /// <inheritdoc/>
        public IQueryable<Qualification> GetQualifications(
            Expression<Func<Qualification, bool>>? predicate = null,
            Expression<Func<Qualification, object>>? orderBySelector = null,
            bool orderAscending = true
        )
        {
            IQueryable<Qualification> query = _context.Qualifications!
                .Include(q => q.Course);

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
