using Application.Photos;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    /// <summary>
    /// Interfaz para la gestión de fotos en la aplicación.
    /// </summary>
    public interface IPhotoService
    {
        /// <summary>
        /// Añade una nueva foto de forma asíncrona.
        /// </summary>
        /// <param name="file">Archivo de foto a cargar.</param>
        /// <returns>Resultado de la carga de la foto.</returns>
        Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);

        /// <summary>
        /// Elimina una foto de forma asíncrona.
        /// </summary>
        /// <param name="publicId">Identificador público de la foto a eliminar.</param>
        /// <returns>Mensaje con el resultado de la operación de eliminación.</returns>
        Task<string> DeletePhotoAsync(string publicId);
    }
}