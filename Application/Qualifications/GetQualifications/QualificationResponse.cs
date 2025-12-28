namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Represents the response containing qualification details for a student.
    /// </summary>
    /// <param name="Student">The name of the student.</param>
    /// <param name="Store">The store identifier associated with the qualification.</param>
    /// <param name="Comment">Any comment related to the qualification.</param>
    /// <param name="CourseName">The name of the course.</param>
    public record QualificationResponse(string? Student, int? Store, string? Comment, string? CourseName)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QualificationResponse"/> record with all properties set to null.
        /// </summary>
        public QualificationResponse() : this(null, null, null, null) { }
    }
}