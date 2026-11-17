// SYSLIB0050: у .NET 8 допоміжні типи ISerializable-інфраструктури (FormatterConverter, StreamingContextStates)
// позначені як застарілі разом із BinaryFormatter. Сам інтерфейс ISerializable не вилучено, і він залишається
// класичним механізмом користувацької серіалізації, який тут і досліджується. Тому попередження вимкнено
// лише в цьому файлі.
#pragma warning disable SYSLIB0050

using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Lab3.DAL.Entities;

namespace Lab3.DAL.Providers;

/// <summary>
/// Користувацька серіалізація через ISerializable. Об'єкт сам віддає свої дані у SerializationInfo
/// (GetObjectData), а провайдер записує їх у власний текстовий формат:
///   [StudentEntity]
///   FirstName=Олена
///   ...
/// Під час читання дані збираються назад у SerializationInfo, і викликається
/// захищений конструктор (SerializationInfo, StreamingContext) сутності.
/// </summary>
public class CustomDataProvider<T> : DataProvider<T> where T : Entity, new()
{
    public override SerializationType Type => SerializationType.Custom;
    public override string FileExtension => ".txt";

    protected override void Write(Stream stream, T[] items)
    {
        using StreamWriter writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true);
        foreach (T item in items)
        {
            SerializationInfo info = NewInfo();
            item.GetObjectData(info, new StreamingContext(StreamingContextStates.File));

            writer.WriteLine($"[{typeof(T).Name}]");
            foreach (SerializationEntry entry in info)
                writer.WriteLine($"{entry.Name}={Convert.ToString(entry.Value, CultureInfo.InvariantCulture)}");
            writer.WriteLine();
        }
    }

    protected override T[] Read(Stream stream)
    {
        List<T> result = new List<T>();
        SerializationInfo info = null;

        using StreamReader reader = new StreamReader(stream, new UTF8Encoding(false), true, leaveOpen: true);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                if (info != null) result.Add(Create(info));
                if (line != $"[{typeof(T).Name}]")
                    throw new FormatException($"Очікувався розділ [{typeof(T).Name}], а знайдено {line}.");
                info = NewInfo();
                continue;
            }

            int separator = line.IndexOf('=');
            if (info == null || separator <= 0)
                throw new FormatException($"Невірний рядок: '{line}'.");
            info.AddValue(line[..separator], line[(separator + 1)..]);
        }

        if (info != null) result.Add(Create(info));
        return result.ToArray();
    }

    private static SerializationInfo NewInfo() => new SerializationInfo(typeof(T), new FormatterConverter());

    private static T Create(SerializationInfo info)
        => (T)Activator.CreateInstance(
            typeof(T),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new object[] { info, new StreamingContext(StreamingContextStates.File) },
            null);
}
