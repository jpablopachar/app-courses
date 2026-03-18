using System.Linq.Expressions;
using Application.Interfaces;
using Domain;
using Persistence;

namespace Infrastructure.Repositories
{
    /// <summary>
    /// Implementa las operaciones de acceso a datos de instructores.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de la clase <see cref="InstructorRepository"/>.
    /// </remarks>
    /// <param name="context">El contexto de base de datos.</param>
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
