using Application.Core;
using MediatR;

namespace Application.Courses.GetCourse
{
    /// <summary>
    /// Query para obtener un curso por su identificador único.
    /// </summary>
    public record GetCourseQuery : IRequest<Result<CourseResponse>>
    {
        /// <summary>
        /// Identificador único del curso a recuperar.
        /// </summary>
        public Guid Id { get; set; }
    }
}