using System.Linq.Expressions;
using Domain;

namespace Application.Interfaces
{
    /// <summary>
    /// Define el contrato para operaciones de acceso a datos de calificaciones.
    /// </summary>
    public interface IQualificationRepository
    {
        /// <summary>
        /// Obtiene una colección consultable de calificaciones con filtrado y ordenamiento opcional.
        /// </summary>
        /// <param name="predicate">Predicado de filtro opcional para aplicar a las calificaciones.</param>
        /// <param name="orderBySelector">Selector de propiedad opcional para el ordenamiento.</param>
        /// <param name="orderAscending">Determina la dirección del orden. True para ascendente, false para descendente.</param>
        /// <returns>Una colección consultable de calificaciones.</returns>
        IQueryable<Qualification> GetQualifications(
            Expression<Func<Qualification, bool>>? predicate = null,
            Expression<Func<Qualification, object>>? orderBySelector = null,
            bool orderAscending = true
        );
    }
}
