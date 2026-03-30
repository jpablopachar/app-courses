using Application.Instructors.GetInstructors;
using Application.Photos;
using Application.Prices.GetPrices;
using Application.Qualifications.GetQualifications;

namespace Application.Courses.GetCourse
{
    /// <summary>
    /// Representa la respuesta con la información de un curso.
    /// </summary>
    public record GetCourseResponse(
        /// <summary>
        /// Identificador único del curso.
        /// </summary>
        Guid Id,
        /// <summary>
        /// Título del curso.
        /// </summary>
        string Title,
        /// <summary>
        /// Descripción del curso.
        /// </summary>
        string Description,
        /// <summary>
        /// Fecha de publicación del curso. Puede ser nula si no ha sido publicado.
        /// </summary>
        DateTime? PublicationDate,
        /// <summary>
        /// Lista de instructores asociados al curso.
        /// </summary>
        List<InstructorResponse> Instructors,
        /// <summary>
        /// Lista de calificaciones del curso.
        /// </summary>
        List<QualificationResponse> Qualifications,
        /// <summary>
        /// Lista de precios del curso.
        /// </summary>
        List<PriceResponse> Prices,
        /// <summary>
        /// Lista de fotos del curso.
        /// </summary>
        List<PhotoResponse> Photos
    );
}