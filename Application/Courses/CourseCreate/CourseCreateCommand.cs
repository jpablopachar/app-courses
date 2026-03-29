using Application.Core;
using MediatR;

namespace Application.Courses.CourseCreate
{
    /// <summary>
    /// Comando para crear un nuevo curso.
    /// </summary>
    /// <param name="CourseCreateRequest">La solicitud que contiene los datos necesarios para crear el curso.</param>
    public record CourseCreateCommand(CourseCreateRequest CourseCreateRequest) : IRequest<Result<Guid>>, ICommandBase;
}