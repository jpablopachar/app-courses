using Microsoft.AspNetCore.Identity;

namespace Persistence.Models
{
    /// <summary>
    /// Represents an application user with additional profile information.
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// Gets or sets the occupation of the user.
        /// </summary>
        public string? Occupation { get; set; }
    }
}