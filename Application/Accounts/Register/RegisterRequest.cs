namespace Application.Accounts.Register
{
    /// <summary>
    /// Represents a request to register a new account.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Gets or sets the degree of the user.
        /// </summary>
        public string? Degree { get; set; }
    }
}