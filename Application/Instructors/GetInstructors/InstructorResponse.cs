namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Representa los datos de respuesta para un instructor.
    /// </summary>
    /// <param name="Id">El identificador único del instructor.</param>
    /// <param name="Name">El nombre del instructor.</param>
    /// <param name="LastName">El apellido del instructor.</param>
    /// <param name="Degree">El grado o título académico del instructor.</param>
    public record InstructorResponse(Guid? Id, string? Name, string? LastName, string? Degree)
    {
        /// <summary>
        /// Inicializa una nueva instancia del registro <see cref="InstructorResponse"/> con valores predeterminados.
        /// </summary>
        public InstructorResponse() : this(null, null, null, null) { }
    };
}