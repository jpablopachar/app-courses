using Application.Common.Constants;
using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Models;

namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Controlador que maneja las consultas para obtener el perfil del usuario actual.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa el patrón Command Query Responsibility Segregation (CQRS) 
    /// utilizando MediatR para gestionar solicitudes de obtención del perfil del usuario autenticado.
    /// </remarks>
    public class GetCurrentUserQueryHandler(
        UserManager<AppUser> userManager,
        IProfileBuilderService profileBuilderService)
        : IRequestHandler<GetCurrentUserQuery, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IProfileBuilderService _profileBuilderService = profileBuilderService;

        /// <summary>
        /// Maneja la solicitud para obtener el perfil del usuario actual.
        /// </summary>
        /// <param name="request">La solicitud que contiene los datos del usuario a buscar.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Un resultado que contiene el perfil del usuario si se encuentra, o un error si no existe.</returns>
        public async Task<Result<Profile>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.GetCurrentUserRequest.Email,
                cancellationToken);

            if (user is null) return Result<Profile>.Failure(ErrorMessages.UserNotFound);

            var profile = await _profileBuilderService.BuildProfileAsync(user);

            return Result<Profile>.Success(profile);
        }
    }
}