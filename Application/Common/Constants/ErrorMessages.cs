namespace Application.Common.Constants
{
    /// <summary>
    /// Provides constant error messages used throughout the application.
    /// </summary>
    public static class ErrorMessages
    {
        /// <summary>
        /// Error message when a user is not found.
        /// </summary>
        public const string UserNotFound = "User not found";

        /// <summary>
        /// Error message for invalid email or password credentials.
        /// </summary>
        public const string InvalidCredentials = "Invalid email or password";

        /// <summary>
        /// Error message when the email is already taken.
        /// </summary>
        public const string EmailTaken = "Email is already taken";

        /// <summary>
        /// Error message when the username is already taken.
        /// </summary>
        public const string UsernameTaken = "Username is already taken";

        /// <summary>
        /// Error message when user registration fails.
        /// </summary>
        public const string RegistrationFailed = "Failed to register user";

        /// <summary>
        /// Error message when validation fails.
        /// </summary>
        public const string ValidationFailed = "One or more validation errors occurred.";
    }
}