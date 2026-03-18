using System.Linq.Expressions;
using Domain;

namespace Application.Interfaces
{
    /// <summary>
    /// Define el contrato para las operaciones de acceso a datos de instructores.
    /// </summary>
    public interface IInstructorRepository
    {
        /// <summary>
        /// Obtiene una colección consultable de instructores con filtrado y ordenamiento opcional.
        /// </summary>
        /// <param name="predicate">Predicado de filtro opcional para aplicar a los instructores.</param>
        /// <param name="orderBySelector">Selector de propiedad opcional para el ordenamiento.</param>
        /// <param name="orderAscending">Determina la dirección del orden. True para ascendente, false para descendente.</param>
        /// <returns>Una colección consultable de instructores.</returns>
        IQueryable<Instructor> GetInstructors(
            Expression<Func<Instructor, bool>>? predicate = null,
            Expression<Func<Instructor, object>>? orderBySelector = null,
            bool orderAscending = true
        );
    }
}
