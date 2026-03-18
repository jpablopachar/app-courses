using System.Linq.Expressions;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementa las operaciones de acceso a datos de calificaciones.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="QualificationRepository"/>.
    /// </remarks>
    /// <param name="context">El contexto de base de datos.</param>
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
