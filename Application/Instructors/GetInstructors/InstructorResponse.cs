namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Represents the response data for an instructor.
    /// </summary>
    /// <param name="Id">The unique identifier of the instructor.</param>
    /// <param name="Name">The first name of the instructor.</param>
    /// <param name="LastName">The last name of the instructor.</param>
    /// <param name="Degree">The degree or academic title of the instructor.</param>
    public record InstructorResponse(Guid? Id, string? Name, string? LastName, string? Degree)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstructorResponse"/> record with default values.
        /// </summary>
        public InstructorResponse() : this(null, null, null, null) { }
    };
}