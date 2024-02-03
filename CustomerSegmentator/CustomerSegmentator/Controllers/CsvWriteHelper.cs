using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

public static class CsvWriteHelper
{
    public const string ContentType = "text/csv";

    public static async Task<byte[]> WriteCsv(Func<CsvWriter, Task> writeRecords)
    {
        await using var memoryStream = new MemoryStream();
        await using var writer = new StreamWriter(memoryStream);
        await using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));
        await writeRecords(csv);
        await csv.FlushAsync();
        await writer.FlushAsync();
        return memoryStream.ToArray();
    }

    public static async Task WriteToCsvAsync<T>(this CsvWriter csvWriter, IReadOnlyCollection<T> records, CancellationToken cancellationToken)
    {
        csvWriter.WriteHeader(typeof(T));
        await csvWriter.NextRecordAsync();
        await csvWriter.WriteRecordsAsync(records, cancellationToken);
        await csvWriter.NextRecordAsync();
    }
}
