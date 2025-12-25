using Application.Core;
using MediatR;

namespace Application.Accounts.Register
{
    /// <summary>
    /// Represents a command request to register a new account.
    /// </summary>
    /// <param name="RegisterRequest">The registration details for the new account.</param>
    public record RegisterCommand(RegisterRequest RegisterRequest) : IRequest<Result<Profile>>, ICommandBase;
}