using Lab3.DAL.Entities;
using Lab3.DAL.Providers;

namespace Lab3.DAL;

/// <summary>
/// Контекст даних (DAL): узагальнений набір операцій над файлом бази даних -
/// створення/відкриття, оновлення (збереження), закриття та видалення файлу.
/// Тип серіалізації й ім'я файлу приходять згори (з рівня представлення).
/// </summary>
public class EntityContext<T> where T : Entity, new()
{
    private readonly List<T> _items = new List<T>();
    private DataProvider<T> _provider;

    public string FilePath { get; private set; }
    public bool IsOpen => _provider != null;
    public SerializationType? CurrentType => _provider?.Type;
    public IReadOnlyList<T> Items => _items;

    /// <summary>Відкриває файл БД; якщо його немає - створює порожній. Повертає true, якщо файл уже існував.</summary>
    public bool Open(string path, SerializationType type)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new DataAccessException("Не вказано ім'я файлу.");

        DataProvider<T> provider = DataProviderFactory.Create<T>(type);
        bool exists = File.Exists(path);
        T[] loaded = Array.Empty<T>();

        if (exists) loaded = provider.Load(path);
        else provider.Save(path, loaded);

        _items.Clear();
        _items.AddRange(loaded);
        _provider = provider;
        FilePath = path;
        return exists;
    }

    public void Add(T item)
    {
        EnsureOpen();
        _items.Add(item);
    }

    public bool Remove(T item)
    {
        EnsureOpen();
        return _items.Remove(item);
    }

    /// <summary>Замінює в пам'яті сутність із тим самим ключем.</summary>
    public void Update(T item)
    {
        EnsureOpen();
        int index = _items.FindIndex(e => string.Equals(e.GetKey(), item.GetKey(), StringComparison.OrdinalIgnoreCase));
        if (index < 0) throw new DataAccessException($"Сутність '{item.GetKey()}' не знайдено в контексті.");
        _items[index] = item;
    }

    /// <summary>Записує поточний стан у відкритий файл.</summary>
    public void SaveChanges()
    {
        EnsureOpen();
        _provider.Save(FilePath, _items);
    }

    /// <summary>Записує поточні дані в інший файл (і, за потреби, іншого типу) без зміни поточного.</summary>
    public void SaveAs(string path, SerializationType type)
    {
        EnsureOpen();
        if (string.IsNullOrWhiteSpace(path)) throw new DataAccessException("Не вказано ім'я файлу.");
        DataProviderFactory.Create<T>(type).Save(path, _items);
    }

    /// <summary>Заново читає дані з файлу (відкидає незбережені зміни в пам'яті).</summary>
    public void Refresh()
    {
        EnsureOpen();
        T[] loaded = _provider.Load(FilePath);
        _items.Clear();
        _items.AddRange(loaded);
    }

    public void Close()
    {
        _items.Clear();
        _provider = null;
        FilePath = null;
    }

    /// <summary>Закриває і фізично видаляє файл БД.</summary>
    public void Delete()
    {
        EnsureOpen();
        string path = FilePath;
        Close();
        try { File.Delete(path); }
        catch (Exception ex) { throw new DataAccessException($"Не вдалося видалити файл '{path}': {ex.Message}", ex); }
    }

    private void EnsureOpen()
    {
        if (!IsOpen) throw new DataAccessException("Файл бази даних не відкрито.");
    }
}
