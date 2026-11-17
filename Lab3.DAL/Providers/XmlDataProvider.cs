using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Lab3.DAL.Entities;

namespace Lab3.DAL.Providers;


public class XmlDataProvider<T> : DataProvider<T> where T : Entity, new()
{
    private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(T[]));

    public override SerializationType Type => SerializationType.Xml;
    public override string FileExtension => ".xml";

    protected override void Write(Stream stream, T[] items)
    {
        XmlWriterSettings settings = new XmlWriterSettings { Indent = true, Encoding = new UTF8Encoding(false) };
        using XmlWriter writer = XmlWriter.Create(stream, settings);
        Serializer.Serialize(writer, items);
    }

    protected override T[] Read(Stream stream) => (T[])Serializer.Deserialize(stream);
}
