namespace Application.Accounts
{
        /// <summary>
        /// Represents a user profile with basic account information.
        /// </summary>
        public class Profile
        {
            /// <summary>
            /// Gets or sets the full name of the user.
            /// </summary>
            public string? FullName { get; set; }

            /// <summary>
            /// Gets or sets the email address of the user.
            /// </summary>
            public string? Email { get; set; }

            /// <summary>
            /// Gets or sets the authentication token for the user.
            /// </summary>
            public string? Token { get; set; }

            /// <summary>
            /// Gets or sets the username of the user.
            /// </summary>
            public string? Username { get; set; }
        }
}