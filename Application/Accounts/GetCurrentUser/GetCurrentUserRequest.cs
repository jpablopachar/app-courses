namespace Application.Accounts.GetCurrentUser
{
    /// <summary>
    /// Represents a request to get the current user's information.
    /// </summary>
    public class GetCurrentUserRequest
    {
        /// <summary>
        /// Gets or sets the email address of the current user.
        /// </summary>
        public string? Email { get; set; }
    }
}