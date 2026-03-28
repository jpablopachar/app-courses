using System.Net;
using Application.Core;
using Application.Instructors.GetInstructors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los instructores.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        /// <summary>
        /// Obtiene una lista paginada de instructores.
        /// </summary>
        /// <param name="request">Los parámetros de consulta para obtener instructores paginados.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Una lista paginada de instructores si la operación es exitosa; de lo contrario, un resultado NotFound.</returns>
        /// <response code="200">Instructores obtenidos exitosamente.</response>
        /// <response code="404">No se encontraron instructores.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedList<InstructorResponse>>> PaginationInstructors([FromQuery] GetInstructorsRequest request, CancellationToken cancellationToken)
        {
            var query = new GetInstructorsQuery { InstructorRequest = request };
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }
    }
}