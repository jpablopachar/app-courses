using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courses.GetCourse
{
    /// <summary>
    /// Manejador de consultas para obtener los detalles de un curso específico.
    /// </summary>
    /// <remarks>
    /// Implementa el patrón CQRS (Command Query Responsibility Segregation) y utiliza MediatR
    /// para procesar la solicitud de obtención de un curso.
    /// </remarks>
    public class GetCourseQueryHandler(AppCoursesDbContext context, IMapper mapper) : IRequestHandler<GetCourseQuery, Result<CourseResponse>>
    {
        private readonly AppCoursesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Maneja la solicitud para obtener un curso específico.
        /// </summary>
        /// <param name="request">La consulta que contiene el identificador del curso a obtener.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Un resultado que contiene la información detallada del curso si existe, o un error en caso contrario.</returns>
        public async Task<Result<CourseResponse>> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _context.Courses!.Where(x => x.Id == request.Id)
                            .Include(x => x.Instructors)
                            .Include(x => x.Prices)
                            .Include(x => x.Qualifications)
                            .Include(x => x.Photos)
                            .ProjectTo<CourseResponse>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return Result<CourseResponse>.Success(course!);
        }
    }
}