using Application.Core;
using MediatR;

namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Consulta para obtener una lista paginada de calificaciones según los parámetros de solicitud especificados.
    /// </summary>
    /// <remarks>
    /// Esta consulta se utiliza para solicitar calificaciones con opciones de filtrado y paginación opcionales.
    /// </remarks>
    public record GetQualificationsQuery
        : IRequest<Result<PagedList<QualificationResponse>>>
    {
        /// <summary>
        /// Los parámetros de solicitud para filtrar y paginar calificaciones.
        /// </summary>
        public GetQualificationsRequest? QualificationsRequest { get; set; }
    }
}