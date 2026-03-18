using Application.Core;

namespace Application.Instructors.GetInstructors
{
    /// <summary>
    /// Parámetros de solicitud para obtener una lista paginada de instructores, con filtrado opcional por nombre y apellido.
    /// </summary>
    public class GetInstructorsRequest : PagingParams
    {
        /// <summary>
        /// Filtro opcional para el nombre del instructor.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Filtro opcional para el apellido del instructor.
        /// </summary>
        public string? LastName { get; set; }
    }
}