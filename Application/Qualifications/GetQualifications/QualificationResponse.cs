namespace Application.Qualifications.GetQualifications
{
    /// <summary>
    /// Representa la respuesta que contiene los detalles de la calificación de un estudiante.
    /// </summary>
    /// <param name="Student">El nombre del estudiante.</param>
    /// <param name="Store">El identificador de tienda asociado a la calificación.</param>
    /// <param name="Comment">Cualquier comentario relacionado con la calificación.</param>
    /// <param name="CourseName">El nombre del curso.</param>
    public record QualificationResponse(string? Student, int? Store, string? Comment, string? CourseName)
    {
        /// <summary>
        /// Inicializa una nueva instancia del registro <see cref="QualificationResponse"/> con todas las propiedades en null.
        /// </summary>
        public QualificationResponse() : this(null, null, null, null) { }
    }
}