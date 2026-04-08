using Application.Core;
using MediatR;

namespace Application.Courses.CourseUpdate
{
    /// <summary>
    /// Comando para actualizar un curso existente.
    /// </summary>
    /// <param name="CourseUpdateRequest">Los datos actualizados del curso.</param>
    /// <param name="CourseId">El identificador único del curso a actualizar.</param>
    public record CourseUpdateCommand(CourseUpdateRequest CourseUpdateRequest, Guid? CourseId): IRequest<Result<Guid>>, ICommandBase;
}