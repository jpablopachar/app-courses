using Application.Core;
using MediatR;

namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Consulta para obtener el perfil del usuario actual basado en la solicitud proporcionada.
    /// </summary>
    /// <param name="GetCurrentUserRequest">La solicitud que contiene información para identificar al usuario actual.</param>
    public record GetCurrentUserQuery(GetCurrentUserRequest GetCurrentUserRequest) : IRequest<Result<Profile>>;
}