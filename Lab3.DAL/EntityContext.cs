using Lab3.DAL.Entities;
using Lab3.DAL.Providers;

namespace Lab3.DAL;


public class EntityContext<T> where T : Entity, new()
{
    private readonly List<T> _items = new List<T>();
    private DataProvider<T> _provider;

    public string FilePath { get; private set; }
    public bool IsOpen => _provider != null;
    public SerializationType? CurrentType => _provider?.Type;
    public IReadOnlyList<T> Items => _items;

    
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

    
    public void Update(T item)
    {
        EnsureOpen();
        int index = _items.FindIndex(e => string.Equals(e.GetKey(), item.GetKey(), StringComparison.OrdinalIgnoreCase));
        if (index < 0) throw new DataAccessException($"Сутність '{item.GetKey()}' не знайдено в контексті.");
        _items[index] = item;
    }

    
    public void SaveChanges()
    {
        EnsureOpen();
        _provider.Save(FilePath, _items);
    }

    
    public void SaveAs(string path, SerializationType type)
    {
        EnsureOpen();
        if (string.IsNullOrWhiteSpace(path)) throw new DataAccessException("Не вказано ім'я файлу.");
        DataProviderFactory.Create<T>(type).Save(path, _items);
    }

   
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
