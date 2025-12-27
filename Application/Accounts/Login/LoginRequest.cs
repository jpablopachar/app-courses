namespace Application.Accounts.Login
{
    /// <summary>
    /// Represents a request for user login containing email and password.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Gets or sets the email address of the user attempting to log in.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user attempting to log in.
        /// </summary>
        public string? Password { get; set; }
    }
}