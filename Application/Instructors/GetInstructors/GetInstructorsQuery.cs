using Application.Core;
using MediatR;

namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Consulta para obtener una lista paginada de instructores basada en los parámetros de solicitud especificados.
    /// </summary>
    /// <remarks>
    /// Este registro se utiliza para encapsular los parámetros para obtener instructores, soportando paginación y filtrado.
    /// </remarks>
    public record GetInstructorsQuery : IRequest<Result<PagedList<InstructorResponse>>>
    {
        /// <summary>
        /// Los parámetros de solicitud para obtener instructores, incluyendo opciones de paginación y filtrado.
        /// </summary>
        public GetInstructorsRequest? InstructorRequest { get; init; }
    }
}