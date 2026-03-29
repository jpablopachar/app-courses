using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courses.CourseCreate
{
    /// <summary>
    /// Manejador para la creación de cursos. Se encarga de procesar la solicitud de creación,
    /// asociar foto, instructor y precio si se proporcionan, y guardar el curso en la base de datos.
    /// </summary>
    /// <remarks>
    /// Utiliza inyección de dependencias para acceder al contexto de base de datos y al servicio de fotos.
    /// </remarks>
    public class CourseCreateCommandHandler(AppCoursesDbContext context, IPhotoService photoService) : IRequestHandler<CourseCreateCommand, Result<Guid>>
    {
        private readonly AppCoursesDbContext _context = context;
        private readonly IPhotoService _photoService = photoService;

        /// <summary>
        /// Procesa la solicitud de creación de un curso, incluyendo la asociación de foto, instructor y precio si se especifican.
        /// </summary>
        /// <param name="request">Comando con los datos para crear el curso.</param>
        /// <param name="cancellationToken">Token para cancelar la operación asincrónica.</param>
        /// <returns>Resultado con el identificador del curso creado o mensaje de error.</returns>
        public async Task<Result<Guid>> Handle(CourseCreateCommand request, CancellationToken cancellationToken)
        {
            var courseId = Guid.NewGuid();

            var course = new Course
            {
                Id = courseId,
                Title = request.CourseCreateRequest.Title,
                Description = request.CourseCreateRequest.Description,
                PublicationDate = request.CourseCreateRequest.PublicationDate,
            };

            if (request.CourseCreateRequest.Photo is not null)
            {
                var photoUploadResult = await _photoService.AddPhotoAsync(request.CourseCreateRequest.Photo);

                var photo = new Photo
                {
                    Id = Guid.NewGuid(),
                    Url = photoUploadResult.Url,
                    PublicId = photoUploadResult.PublicId,
                    CourseId = courseId
                };

                course.Photos = [photo];
            }

            if (request.CourseCreateRequest.InstructorId is not null)
            {
                var instructor = await _context.Instructors!.FirstOrDefaultAsync(i => i.Id == request.CourseCreateRequest.InstructorId, cancellationToken: cancellationToken);

                if (instructor is null) return Result<Guid>.Failure("Instructor not found");

                course.Instructors = [instructor];
            }

            if (request.CourseCreateRequest.PriceId is not null)
            {
                var price = await _context.Prices!.FirstOrDefaultAsync(p => p.Id == request.CourseCreateRequest.PriceId, cancellationToken: cancellationToken);

                if (price is null) return Result<Guid>.Failure("Price not found");

                course.Prices = [price];
            }

            _context.Add(course);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return result ? Result<Guid>.Success(courseId) : Result<Guid>.Failure("Failed to create course");
        }
    }
}