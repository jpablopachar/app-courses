using System.Linq.Expressions;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courses.GetCourses
{
    /// <summary>
    /// Controlador de consultas para obtener cursos con filtrado, ordenamiento y paginación.
    /// </summary>
    /// <remarks>
    /// Maneja las solicitudes de obtención de cursos aplicando filtros por título y descripción,
    /// permitiendo ordenar los resultados de forma ascendente o descendente, y devolviendo
    /// resultados paginados.
    /// </remarks>
    public class GetCoursesQueryHandler(AppCoursesDbContext context, IMapper mapper) : IRequestHandler<GetCoursesQuery, Result<PagedList<CourseResponse>>>
    {
        private readonly AppCoursesDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Maneja la consulta para obtener una lista paginada de cursos.
        /// </summary>
        /// <param name="request">
        /// La solicitud que contiene los parámetros de filtrado, ordenamiento y paginación.
        /// </param>
        /// <param name="cancellationToken">
        /// Token de cancelación para interrumpir la operación asincrónica.
        /// </param>
        /// <returns>
        /// Una tarea que representa la operación asincrónica, devolviendo un resultado que contiene
        /// una lista paginada de respuestas de cursos o un mensaje de error.
        /// </returns>
        public async Task<Result<PagedList<CourseResponse>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Course> queryable = _context.Courses!
                            .Include(c => c.Instructors)
                            .Include(c => c.Qualifications)
                            .Include(c => c.Prices)
                            .Include(c => c.Photos);

            var predicate = ExpressionBuilder.New<Course>();

            if (!string.IsNullOrEmpty(request.GetCoursesRequest?.Title))
            {
                predicate = predicate.And(c => c.Title!.Contains(request.GetCoursesRequest.Title, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.GetCoursesRequest?.Description))
            {
                predicate = predicate.And(c => c.Description!.Contains(request.GetCoursesRequest.Description, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.GetCoursesRequest!.OrderBy))
            {
                Expression<Func<Course, object>>? orderBySelector = request.GetCoursesRequest.OrderBy!.ToLower() switch
                {
                    "title" => c => c.Title!,
                    "description" => c => c.Description!,
                    _ => c => c.Title!
                };

                bool orderBy = request.GetCoursesRequest.OrderAsc ?? true;

                queryable = orderBy
                    ? queryable.OrderBy(orderBySelector!)
                    : queryable.OrderByDescending(orderBySelector!);
            }

            queryable = queryable.Where(predicate);

            var coursesQuery = queryable.ProjectTo<CourseResponse>(_mapper.ConfigurationProvider).AsQueryable();

            var pagination = await PagedList<CourseResponse>.CreateAsync(coursesQuery, request.GetCoursesRequest.PageNumber, request.GetCoursesRequest.PageSize, cancellationToken);

            return Result<PagedList<CourseResponse>>.Success(pagination);
        }
    }
}