using Domain;

namespace Application.Interfaces
{
    /// <summary>
    /// Interfaz para la generación de reportes en formato CSV.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que hereda de BaseEntity.</typeparam>
    public interface IReportService<T> where T : BaseEntity
    {
        /// <summary>
        /// Genera un reporte en formato CSV de forma asincrónica.
        /// </summary>
        /// <param name="records">Lista de registros a incluir en el reporte.</param>
        /// <returns>Un flujo de memoria que contiene el contenido del reporte CSV.</returns>
        Task<MemoryStream> GetCsvReportAsync(List<T> records);
    }
}