using Application.Core;
using MediatR;

namespace Application.Accounts.Login
{
    /// <summary>
    /// Command record for handling user login requests.
    /// </summary>
    /// <param name="LoginRequest">The login request containing user credentials.</param>
    public record LoginCommand(LoginRequest LoginRequest) : IRequest<Result<Profile>>, ICommandBase;
}