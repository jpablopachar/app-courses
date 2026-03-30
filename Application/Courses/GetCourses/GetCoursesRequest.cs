using Application.Core;

namespace Application.Courses.GetCourses
{
    /// <summary>
    /// Solicitud para obtener la lista de cursos con filtros opcionales.
    /// </summary>
    public class GetCoursesRequest : PagingParams
    {
        /// <summary>
        /// Título del curso para filtrar la búsqueda.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Descripción del curso para filtrar la búsqueda.
        /// </summary>
        public string? Description { get; set; }
    }
}