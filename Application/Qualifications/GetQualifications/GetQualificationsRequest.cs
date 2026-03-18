using Application.Core;

namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Parámetros de solicitud para obtener calificaciones con filtrado y paginación opcional.
    /// </summary>
    public class GetQualificationsRequest : PagingParams
    {
        /// <summary>
        /// Obtiene o establece el identificador o nombre del estudiante para filtrar calificaciones.
        /// </summary>
        public string? Student { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador del curso para filtrar calificaciones.
        /// </summary>
        public Guid? CourseId { get; set; }
    }
}