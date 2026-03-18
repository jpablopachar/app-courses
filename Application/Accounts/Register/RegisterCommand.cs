using Application.Core;
using MediatR;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Representa una solicitud de comando para registrar una nueva cuenta.
    /// </summary>
    /// <param name="RegisterRequest">Los detalles de registro para la nueva cuenta.</param>
    public record RegisterCommand(RegisterRequest RegisterRequest) : IRequest<Result<Profile>>, ICommandBase;
}