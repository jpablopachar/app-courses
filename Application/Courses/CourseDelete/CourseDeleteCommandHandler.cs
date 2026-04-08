using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courses.CourseDelete
{
    /// <summary>
    /// Manejador de comandos para eliminar un curso.
    /// Implementa la interfaz IRequestHandler de MediatR para procesar comandos de eliminación de cursos.
    /// </summary>
    public class CourseDeleteCommandHandler(AppCoursesDbContext context) : IRequestHandler<CourseDeleteCommand, Result<Unit>>
    {
        private readonly AppCoursesDbContext _context = context;

        /// <summary>
        /// Maneja la eliminación de un curso existente.
        /// Busca el curso por su ID, incluye todas sus relaciones (instructores, precios, calificaciones y fotos),
        /// y procede a eliminarlo de la base de datos.
        /// </summary>
        /// <param name="request">Comando que contiene el ID del curso a eliminar.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>
        /// Un Result&lt;Unit&gt; que indica el éxito o fracaso de la operación.
        /// Retorna éxito si el curso fue eliminado correctamente, o un fallo si el curso no existe o la eliminación falló.
        /// </returns>
        public async Task<Result<Unit>> Handle(CourseDeleteCommand request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses!
            .Include(c => c.Instructors)
            .Include(c => c.Prices)
            .Include(c => c.Qualifications)
            .Include(c => c.Photos)
            .FirstOrDefaultAsync(c => c.Id == request.CourseId, cancellationToken);

            if (course is null) return Result<Unit>.Failure("Course not found.");

            _context.Courses!.Remove(course);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Failed to delete course.");
        }
    }
}