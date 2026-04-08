using Application.Core;
using MediatR;

namespace Application.Courses.CourseDelete
{
    /// <summary>
    /// Comando para eliminar un curso.
    /// </summary>
    /// <param name="CourseId">Identificador único del curso a eliminar.</param>
    public record CourseDeleteCommand(Guid? CourseId) : IRequest<Result<Unit>>, ICommandBase;
}