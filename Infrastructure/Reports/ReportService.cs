using System.Globalization;
using Application.Interfaces;
using Domain;

namespace Infrastructure.Reports
{
    /// <summary>
    /// Servicio genérico para generar reportes en formato CSV.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad que debe heredar de BaseEntity.</typeparam>
    public class ReportService<T> : IReportService<T> where T : BaseEntity
    {
        /// <summary>
        /// Genera un reporte en formato CSV a partir de una lista de registros.
        /// </summary>
        /// <param name="records">Lista de registros a exportar en el reporte CSV.</param>
        /// <returns>Un flujo de memoria que contiene los datos del reporte en formato CSV.</returns>
        public Task<MemoryStream> GetCsvReportAsync(List<T> records)
        {
            using var memoryStream = new MemoryStream();
            using var textWriter = new StreamWriter(memoryStream);
            using var csvWriter = new CsvHelper.CsvWriter(textWriter, CultureInfo.InvariantCulture);

            csvWriter.WriteRecords(records);
            textWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);

            return Task.FromResult(memoryStream);
        }
    }
}