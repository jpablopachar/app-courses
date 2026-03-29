using Application.Interfaces;
using Application.Photos;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Infrastructure.Photos
{
    /// <summary>
    /// Servicio para gestionar la carga y eliminación de fotografías usando Cloudinary.
    /// </summary>
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="PhotoService"/>.
        /// </summary>
        /// <param name="config">Configuración de Cloudinary con credenciales de API.</param>
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        /// <summary>
        /// Carga una fotografía de forma asincrónica a Cloudinary.
        /// </summary>
        /// <param name="file">El archivo de imagen a cargar.</param>
        /// <returns>Un objeto <see cref="PhotoUploadResult"/> con los detalles de la carga realizada.</returns>
        /// <exception cref="Exception">Se lanza si ocurre un error durante la carga.</exception>
        public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
        {
            if (file.Length <= 0) return null!;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation()
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error is not null) throw new Exception(result.Error.Message);

            return new PhotoUploadResult
            {
                PublicId = result.PublicId,
                Url = result.SecureUrl.AbsoluteUri
            };
        }

        /// <summary>
        /// Elimina una fotografía de Cloudinary de forma asincrónica.
        /// </summary>
        /// <param name="publicId">El identificador público de la fotografía a eliminar.</param>
        /// <returns>Una cadena que indica "ok" si la eliminación fue exitosa, de lo contrario retorna null.</returns>
        public async Task<string> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok" ? result.Result : null!;
        }
    }
}