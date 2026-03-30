using Application.Core;
using MediatR;

namespace Application.Courses.GetCourses
{
    /// <summary>
    /// Query para obtener una lista paginada de cursos.
    /// </summary>
    public record GetCoursesQuery : IRequest<Result<PagedList<CourseResponse>>>
    {
        /// <summary>
        /// Parámetros de solicitud para filtrar y paginar los cursos.
        /// </summary>
        public GetCoursesRequest? GetCoursesRequest { get; set; }
    }
}