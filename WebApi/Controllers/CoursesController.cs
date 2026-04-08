using System.Net;
using Application.Core;
using Application.Courses;
using Application.Courses.CourseCreate;
using Application.Courses.CourseDelete;
using Application.Courses.CourseExcelReport;
using Application.Courses.CourseUpdate;
using Application.Courses.GetCourse;
using Application.Courses.GetCourses;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los cursos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        /// <summary>
        /// Obtiene una lista paginada de cursos.
        /// </summary>
        /// <param name="request">Los parámetros de consulta para filtrar y paginar los cursos.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Una lista paginada de cursos si la operación es exitosa; de lo contrario, un resultado NotFound.</returns>
        /// <response code="200">Cursos obtenidos exitosamente.</response>
        /// <response code="404">No se encontraron cursos.</response>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<PagedList<CourseResponse>>> PaginationCourses([FromQuery] GetCoursesRequest request, CancellationToken cancellationToken)
        {
            var query = new GetCoursesQuery { GetCoursesRequest = request };
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        /// <summary>
        /// Obtiene un curso por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del curso.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>El curso si se encuentra; de lo contrario, un resultado NotFound.</returns>
        /// <response code="200">Curso obtenido exitosamente.</response>
        /// <response code="404">No se encontró el curso.</response>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<CourseResponse>> GetCourse(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCourseQuery { Id = id };
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : NotFound();
        }

        /// <summary>
        /// Crea un nuevo curso.
        /// </summary>
        /// <param name="request">Los datos necesarios para crear el curso.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>El identificador del curso creado si la operación es exitosa; de lo contrario, un resultado BadRequest.</returns>
        /// <response code="200">Curso creado exitosamente.</response>
        /// <response code="400">Datos de solicitud inválidos.</response>
        [Authorize(Policy = PolicyMaster.COURSE_WRITE)]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Guid>> CreateCourse([FromForm] CourseCreateRequest request, CancellationToken cancellationToken)
        {
            var command = new CourseCreateCommand(request);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        /// <summary>
        /// Actualiza un curso existente.
        /// </summary>
        /// <param name="id">El identificador único del curso a actualizar.</param>
        /// <param name="request">Los datos actualizados del curso.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>El identificador del curso actualizado si la operación es exitosa; de lo contrario, un resultado BadRequest.</returns>
        /// <response code="200">Curso actualizado exitosamente.</response>
        /// <response code="400">Datos de solicitud inválidos.</response>
        [Authorize(Policy = PolicyMaster.COURSE_UPDATE)]
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Guid>> UpdateCourse(Guid id, [FromBody] CourseUpdateRequest request, CancellationToken cancellationToken)
        {
            var command = new CourseUpdateCommand(request, id);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        /// <summary>
        /// Elimina un curso por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del curso a eliminar.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>OK si la operación es exitosa; de lo contrario, un resultado BadRequest.</returns>
        /// <response code="200">Curso eliminado exitosamente.</response>
        /// <response code="400">No se pudo eliminar el curso.</response>
        [Authorize(Policy = PolicyMaster.COURSE_DELETE)]
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteCourse(Guid id, CancellationToken cancellationToken)
        {
            var command = new CourseDeleteCommand(id);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        /// <summary>
        /// Genera y descarga un reporte de cursos en formato CSV.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Un archivo CSV con el reporte de cursos.</returns>
        /// <response code="200">Reporte generado exitosamente.</response>
        [Authorize(Policy = PolicyMaster.COURSE_READ)]
        [HttpGet("report")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ExcelReport(CancellationToken cancellationToken)
        {
            var query = new CourseExcelReportQuery();
            var result = await _sender.Send(query, cancellationToken);

            byte[] excelBytes = result.ToArray();

            return File(excelBytes, "text/csv", "courses-report.csv");
        }
    }
}
