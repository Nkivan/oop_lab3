using System.Reflection;
using System.Text;
using Lab3.DAL.Entities;

namespace Lab3.DAL.Providers;


public class BinaryDataProvider<T> : DataProvider<T> where T : Entity, new()
{
    private const string Signature = "LAB3-BIN";

    private static readonly PropertyInfo[] Properties = typeof(T)
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.CanRead && p.CanWrite)
        .OrderBy(p => p.Name, StringComparer.Ordinal)
        .ToArray();

    public override SerializationType Type => SerializationType.Binary;
    public override string FileExtension => ".bin";

    protected override void Write(Stream stream, T[] items)
    {
        using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(Signature);
        writer.Write(items.Length);
        foreach (T item in items)
            foreach (PropertyInfo property in Properties)
                WriteValue(writer, property.PropertyType, property.GetValue(item));
    }

    protected override T[] Read(Stream stream)
    {
        using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
        if (reader.ReadString() != Signature)
            throw new FormatException("Файл не є двійковою базою цієї програми.");

        int count = reader.ReadInt32();
        if (count < 0) throw new FormatException("Пошкоджений заголовок файлу.");

        T[] result = new T[count];
        for (int i = 0; i < count; i++)
        {
            T item = new T();
            foreach (PropertyInfo property in Properties)
                property.SetValue(item, ReadValue(reader, property.PropertyType));
            result[i] = item;
        }
        return result;
    }

    private static void WriteValue(BinaryWriter writer, Type type, object value)
    {
        if (type == typeof(string))
        {
            string text = (string)value;
            writer.Write(text != null);
            if (text != null) writer.Write(text);
        }
        else if (type == typeof(int)) writer.Write((int)value);
        else if (type == typeof(bool)) writer.Write((bool)value);
        else if (type == typeof(decimal)) writer.Write((decimal)value);
        else if (type == typeof(double)) writer.Write((double)value);
        else throw new NotSupportedException($"Тип {type.Name} не підтримується двійковим провайдером.");
    }

    private static object ReadValue(BinaryReader reader, Type type)
    {
        if (type == typeof(string)) return reader.ReadBoolean() ? reader.ReadString() : null;
        if (type == typeof(int)) return reader.ReadInt32();
        if (type == typeof(bool)) return reader.ReadBoolean();
        if (type == typeof(decimal)) return reader.ReadDecimal();
        if (type == typeof(double)) return reader.ReadDouble();
        throw new NotSupportedException($"Тип {type.Name} не підтримується двійковим провайдером.");
    }
}
