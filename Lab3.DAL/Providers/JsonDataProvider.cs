using System.Text.Encodings.Web;
using System.Text.Json;
using Lab3.DAL.Entities;

namespace Lab3.DAL.Providers;

/// <summary>JSON-серіалізація (System.Text.Json).</summary>
public class JsonDataProvider<T> : DataProvider<T> where T : Entity, new()
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping   // кирилиця читабельна, без \uXXXX
    };

    public override SerializationType Type => SerializationType.Json;
    public override string FileExtension => ".json";

    protected override void Write(Stream stream, T[] items) => JsonSerializer.Serialize(stream, items, Options);

    protected override T[] Read(Stream stream) => JsonSerializer.Deserialize<T[]>(stream, Options) ?? Array.Empty<T>();
}
