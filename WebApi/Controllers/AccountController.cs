using System.Net;
using Application.Accounts;
using Application.Accounts.GetCurrentUser;
using Application.Accounts.Login;
using Application.Accounts.Register;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar las operaciones de cuenta de usuario.
    /// Proporciona endpoints para autenticación, registro y obtención de información del usuario actual.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(ISender sender, IUserAccessor userAccessor) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly IUserAccessor _userAccessor = userAccessor;

        /// <summary>
        /// Autentica un usuario y devuelve su perfil si las credenciales son válidas.
        /// </summary>
        /// <param name="request">Solicitud de inicio de sesión con credenciales del usuario.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Perfil del usuario autenticado o respuesta No autorizado.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : Unauthorized();
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="request">Solicitud de registro con información del nuevo usuario.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Perfil del usuario registrado o respuesta No autorizado.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> Register([FromForm] RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(request);
            var result = await _sender.Send(command, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : Unauthorized();
        }

        /// <summary>
        /// Obtiene la información del usuario autenticado actualmente.
        /// Requiere que el usuario esté autenticado.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Perfil del usuario actual o respuesta No autorizado.</returns>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<Profile>> Me(CancellationToken cancellationToken)
        {
            var email = _userAccessor.GetEmail();
            var request = new GetCurrentUserRequest { Email = email };
            var query = new GetCurrentUserQuery(request);
            var result = await _sender.Send(query, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : Unauthorized();
        }
    }
}