
#pragma warning disable SYSLIB0050

using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace Lab3.Part1;


public interface IBookSerializer
{
    string Name { get; }
    string Extension { get; }
    void Serialize<TCollection>(string path, TCollection books) where TCollection : IEnumerable<Book>;
    TCollection Deserialize<TCollection>(string path) where TCollection : IEnumerable<Book>;
}

internal static class CollectionFactory
{
    public static TCollection Create<TCollection>(Book[] items) where TCollection : IEnumerable<Book>
    {
        object result;
        if (typeof(TCollection) == typeof(Book[])) result = items;
        else if (typeof(TCollection) == typeof(List<Book>)) result = new List<Book>(items);
        else throw new NotSupportedException($"Тип колекції {typeof(TCollection).Name} не підтримується.");
        return (TCollection)result;
    }
}

public class BinaryBookSerializer : IBookSerializer
{
    public string Name => "Двійкова (BinaryWriter/BinaryReader)";
    public string Extension => ".bin";

    public void Serialize<TCollection>(string path, TCollection books) where TCollection : IEnumerable<Book>
    {
        Book[] array = books.ToArray();
        using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);
        writer.Write(array.Length);
        foreach (Book book in array)
        {
            writer.Write(book.SerialNumber);
            writer.Write(book.Title);
            writer.Write(book.PublicationYear);
            writer.Write(book.PricePerCopy);
            writer.Write(book.Copies);
        }
    }

    public TCollection Deserialize<TCollection>(string path) where TCollection : IEnumerable<Book>
    {
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
        int count = reader.ReadInt32();
        Book[] result = new Book[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = new Book(
                reader.ReadInt32(), reader.ReadString(), reader.ReadInt32(),
                reader.ReadDecimal(), reader.ReadInt32());
        }
        return CollectionFactory.Create<TCollection>(result);
    }
}

public class CustomBookSerializer : IBookSerializer
{
    public string Name => "Користувацька (ISerializable)";
    public string Extension => ".txt";

    public void Serialize<TCollection>(string path, TCollection books) where TCollection : IEnumerable<Book>
    {
        using StreamWriter writer = new StreamWriter(path, false, new UTF8Encoding(false));
        foreach (Book book in books)
        {
            SerializationInfo info = new SerializationInfo(typeof(Book), new FormatterConverter());
            book.GetObjectData(info, new StreamingContext(StreamingContextStates.File));

            writer.WriteLine("[Book]");
            foreach (SerializationEntry entry in info)
                writer.WriteLine($"{entry.Name}={Convert.ToString(entry.Value, CultureInfo.InvariantCulture)}");
            writer.WriteLine();
        }
    }

    public TCollection Deserialize<TCollection>(string path) where TCollection : IEnumerable<Book>
    {
        Book[] result = new Book[0];
        SerializationInfo info = null;

        foreach (string line in File.ReadLines(path, new UTF8Encoding(false)))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line == "[Book]")
            {
                if (info != null) result = Append(result, Create(info));
                info = new SerializationInfo(typeof(Book), new FormatterConverter());
                continue;
            }

            int separator = line.IndexOf('=');
            if (info == null || separator <= 0) throw new FormatException($"Невірний рядок: '{line}'.");
            info.AddValue(line[..separator], line[(separator + 1)..]);
        }
        if (info != null) result = Append(result, Create(info));

        return CollectionFactory.Create<TCollection>(result);
    }

    private static Book Create(SerializationInfo info)
        => (Book)Activator.CreateInstance(
            typeof(Book),
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public,
            null,
            new object[] { info, new StreamingContext(StreamingContextStates.File) },
            null);

    private static Book[] Append(Book[] array, Book item)
    {
        Book[] bigger = new Book[array.Length + 1];
        Array.Copy(array, bigger, array.Length);
        bigger[array.Length] = item;
        return bigger;
    }
}

public class XmlBookSerializer : IBookSerializer
{
    public string Name => "XML (XmlSerializer)";
    public string Extension => ".xml";

    public void Serialize<TCollection>(string path, TCollection books) where TCollection : IEnumerable<Book>
    {
        using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        using XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) });
        new XmlSerializer(typeof(TCollection)).Serialize(writer, books);
    }

    public TCollection Deserialize<TCollection>(string path) where TCollection : IEnumerable<Book>
    {
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return (TCollection)new XmlSerializer(typeof(TCollection)).Deserialize(stream);
    }
}

public class JsonBookSerializer : IBookSerializer
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public string Name => "JSON (System.Text.Json)";
    public string Extension => ".json";

    public void Serialize<TCollection>(string path, TCollection books) where TCollection : IEnumerable<Book>
    {
        using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        JsonSerializer.Serialize(stream, books, Options);
    }

    public TCollection Deserialize<TCollection>(string path) where TCollection : IEnumerable<Book>
    {
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return JsonSerializer.Deserialize<TCollection>(stream, Options);
    }
}
