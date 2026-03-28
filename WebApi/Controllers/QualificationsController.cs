using System.Net;
using Application.Core;
using Application.Qualifications.GetQualifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con las calificaciones.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class QualificationsController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        /// <summary>
        /// Obtiene una lista paginada de calificaciones.
        /// </summary>
        /// <param name="request">Los parámetros de consulta para obtener las calificaciones paginadas.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Una lista paginada de calificaciones si la operación es exitosa; de lo contrario, un resultado NotFound.</returns>
        /// <response code="200">Calificaciones obtenidas exitosamente.</response>
        /// <response code="404">No se encontraron calificaciones.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedList<QualificationResponse>>> PaginationQualification([FromQuery] GetQualificationsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetQualificationsQuery { QualificationsRequest = request };
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }
    }
}