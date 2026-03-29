using Microsoft.AspNetCore.Http;

namespace Application.Courses.CourseCreate
{
    /// <summary>
    /// Representa la solicitud para crear un nuevo curso.
    /// </summary>
    public class CourseCreateRequest
    {
        /// <summary>
        /// Obtiene o establece el título del curso.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Obtiene o establece la descripción del curso.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de publicación del curso.
        /// </summary>
        public DateTime? PublicationDate { get; set; }

        /// <summary>
        /// Obtiene o establece la foto o imagen principal del curso.
        /// </summary>
        public IFormFile? Photo { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador único del instructor responsable del curso.
        /// </summary>
        public Guid? InstructorId { get; set; }

        /// <summary>
        /// Obtiene o establece el identificador único del precio del curso.
        /// </summary>
        public Guid? PriceId { get; set; }
    }
}