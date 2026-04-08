using MediatR;

namespace Application.Courses.CourseExcelReport
{
    /// <summary>
    /// Query para generar un reporte de cursos en formato Excel.
    /// </summary>
    public record CourseExcelReportQuery : IRequest<MemoryStream>;
}