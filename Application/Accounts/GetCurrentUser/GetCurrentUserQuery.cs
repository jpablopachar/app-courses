using Application.Core;
using MediatR;

namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Query to retrieve the current user's profile based on the provided request.
    /// </summary>
    /// <param name="GetCurrentUserRequest">The request containing information to identify the current user.</param>
    public record GetCurrentUserQuery(GetCurrentUserRequest GetCurrentUserRequest) : IRequest<Result<Profile>>;
}