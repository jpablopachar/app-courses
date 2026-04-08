using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courses.CourseUpdate
{
    /// <summary>
    /// Handler para procesar comandos de actualización de cursos.
    /// Implementa la interfaz IRequestHandler del patrón CQRS.
    /// </summary>
    public class CourseUpdateCommandHandler(AppCoursesDbContext context) : IRequestHandler<CourseUpdateCommand, Result<Guid>>
    {
        private readonly AppCoursesDbContext _context = context;

        /// <summary>
        /// Maneja la solicitud de actualización de un curso.
        /// </summary>
        /// <param name="request">Comando con los datos del curso a actualizar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Resultado con el ID del curso actualizado o un mensaje de error.</returns>
        public async Task<Result<Guid>> Handle(CourseUpdateCommand request, CancellationToken cancellationToken)
        {
            var courseId = request.CourseId;

            var course = await _context.Courses!.FirstOrDefaultAsync(c => c.Id == courseId, cancellationToken);

            if (course is null)
            {
                return Result<Guid>.Failure("Course not found.");
            }

            course.Title = request.CourseUpdateRequest.Title;
            course.Description = request.CourseUpdateRequest.Description;
            course.PublicationDate = request.CourseUpdateRequest.PublicationDate;

            _context.Entry(course).State = EntityState.Modified;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result
                ? Result<Guid>.Success(course.Id)
                : Result<Guid>.Failure("Failed to update the course.");
        }
    }
}