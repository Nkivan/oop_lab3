using Lab3.DAL.Entities;

namespace Lab3.DAL.Providers;

/// <summary>
/// Абстракція провайдера даних: відповідає за читання й запис набору сутностей у файл певного типу.
/// Загальна частина (робота з FileStream, обробка помилок) - тут, а конкретний спосіб
/// серіалізації - у нащадках (Write/Read). Так код не повторюється.
/// </summary>
public abstract class DataProvider<T> where T : Entity, new()
{
    public abstract SerializationType Type { get; }
    public abstract string FileExtension { get; }

    public void Save(string path, IReadOnlyCollection<T> items)
    {
        try
        {
            string directory = Path.GetDirectoryName(Path.GetFullPath(path));
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);

            T[] array = items.ToArray();
            using FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            Write(stream, array);
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Не вдалося записати файл '{path}' ({Type}): {ex.Message}", ex);
        }
    }

    public T[] Load(string path)
    {
        try
        {
            using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            return Read(stream);
        }
        catch (Exception ex)
        {
            throw new DataAccessException(
                $"Не вдалося прочитати файл '{path}' як {Type} (можливо, це файл іншого формату): {ex.Message}", ex);
        }
    }

    protected abstract void Write(Stream stream, T[] items);
    protected abstract T[] Read(Stream stream);
}
