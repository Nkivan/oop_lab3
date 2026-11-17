using Lab3.DAL.Entities;

namespace Lab3.DAL.Providers;

public static class DataProviderFactory
{
    public static DataProvider<T> Create<T>(SerializationType type) where T : Entity, new()
    {
        return type switch
        {
            SerializationType.Binary => new BinaryDataProvider<T>(),
            SerializationType.Custom => new CustomDataProvider<T>(),
            SerializationType.Xml => new XmlDataProvider<T>(),
            SerializationType.Json => new JsonDataProvider<T>(),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Невідомий тип серіалізації.")
        };
    }

    public static string ExtensionFor(SerializationType type) => type switch
    {
        SerializationType.Binary => ".bin",
        SerializationType.Custom => ".txt",
        SerializationType.Xml => ".xml",
        SerializationType.Json => ".json",
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Невідомий тип серіалізації.")
    };
}
