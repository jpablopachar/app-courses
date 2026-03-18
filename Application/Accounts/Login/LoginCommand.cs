using Application.Core;
using MediatR;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Registro de comando para gestionar solicitudes de inicio de sesión de usuario.
    /// </summary>
    /// <param name="LoginRequest">La solicitud de inicio de sesión que contiene las credenciales del usuario.</param>
    public record LoginCommand(LoginRequest LoginRequest) : IRequest<Result<Profile>>, ICommandBase;
}