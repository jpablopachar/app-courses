using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Courses.CourseExcelReport
{
    /// <summary>
    /// Controlador de consulta para generar reportes de cursos en formato Excel/CSV.
    /// Implementa el patrón MediatR para manejar solicitudes de reporte de cursos.
    /// </summary>
    public class CourseExcelReportQueryHandler(AppCoursesDbContext context, IReportService<Course> reportService) : IRequestHandler<CourseExcelReportQuery, MemoryStream>
    {
        /// <summary>
        /// Contexto de base de datos para acceder a los cursos.
        /// </summary>
        private readonly AppCoursesDbContext _context = context;

        /// <summary>
        /// Servicio de reporte para generar reportes en formato CSV.
        /// </summary>
        private readonly IReportService<Course> _reportService = reportService;

        /// <summary>
        /// Maneja la solicitud de reporte de cursos, recupera los primeros 10 cursos de la base de datos
        /// y los convierte en un reporte CSV que se devuelve como un flujo de memoria.
        /// </summary>
        /// <param name="request">Solicitud de reporte de cursos (CourseExcelReportQuery).</param>
        /// <param name="cancellationToken">Token de cancelación para operaciones asincrónicas.</param>
        /// <returns>Un flujo de memoria (MemoryStream) que contiene el reporte en formato CSV.</returns>
        public async Task<MemoryStream> Handle(CourseExcelReportQuery request, CancellationToken cancellationToken)
        {
            var courses = await _context.Courses!.Take(10).Skip(0).ToListAsync(cancellationToken);

            return await _reportService.GetCsvReportAsync(courses);
        }
    }
}