using Lab3.BLL.Exceptions;
using Lab3.BLL.Mapping;
using Lab3.BLL.Models;
using Lab3.DAL;
using Lab3.DAL.Entities;
using Lab3.DAL.Providers;

namespace Lab3.BLL.Services;


public class EntityService<TModel, TEntity> : IEntityService<TModel>
    where TModel : Person
    where TEntity : PersonEntity, new()
{
    private readonly EntityContext<TEntity> _context;
    private readonly IEntityMapper<TModel, TEntity> _mapper;

    public EntityService(EntityContext<TEntity> context, IEntityMapper<TModel, TEntity> mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public bool IsOpen => _context.IsOpen;
    public string CurrentFile => _context.FilePath;

    public SerializationFormat? CurrentFormat
        => _context.CurrentType.HasValue ? ToFormat(_context.CurrentType.Value) : null;

    

    public bool OpenStorage(string path, SerializationFormat format)
    {
        SerializationType type = ToType(format);
        string fullPath = WithExtension(path, type);
        return Execute(() => _context.Open(fullPath, type));
    }

    public void SaveAs(string path, SerializationFormat format)
    {
        EnsureOpen();
        SerializationType type = ToType(format);
        string fullPath = WithExtension(path, type);
        Execute(() => _context.SaveAs(fullPath, type));
    }

    public void CloseStorage()
    {
        EnsureOpen();
        _context.Close();
    }

    public void DeleteStorage()
    {
        EnsureOpen();
        Execute(() => _context.Delete());
    }

    

    public IReadOnlyList<TModel> GetAll()
    {
        EnsureOpen();
        return _context.Items.Select(_mapper.ToModel).ToList();
    }

    public void Add(TModel model)
    {
        EnsureOpen();
        if (FindEntity(model.Key) != null) throw new DuplicateEntityException(model.Key);
        Execute(() =>
        {
            _context.Add(_mapper.ToEntity(model));
            _context.SaveChanges();
        });
    }

    public void Remove(string key)
    {
        EnsureOpen();
        TEntity entity = FindEntity(key) ?? throw new EntityNotFoundException(key);
        Execute(() =>
        {
            _context.Remove(entity);
            _context.SaveChanges();
        });
    }

    public TModel GetByKey(string key)
    {
        EnsureOpen();
        TEntity entity = FindEntity(key) ?? throw new EntityNotFoundException(key);
        return _mapper.ToModel(entity);
    }

    public IReadOnlyList<TModel> FindByLastName(string lastName)
    {
        EnsureOpen();
        return _context.Items
            .Where(e => string.Equals(e.LastName, lastName, StringComparison.OrdinalIgnoreCase))
            .Select(_mapper.ToModel)
            .ToList();
    }

    public void Dance(string key)
    {
        EnsureOpen();
        TEntity entity = FindEntity(key) ?? throw new EntityNotFoundException(key);
        TModel model = _mapper.ToModel(entity);
        model.Dance();
        Execute(() =>
        {
            _context.Update(_mapper.ToEntity(model));
            _context.SaveChanges();
        });
    }

    

    protected void EnsureOpen()
    {
        if (!_context.IsOpen)
            throw new StorageException("Файл бази даних не відкрито. Спершу відкрийте або створіть файл.");
    }

    
    protected void ReloadFromFile() => Execute(() => _context.Refresh());

    protected IReadOnlyList<TModel> AllModels() => _context.Items.Select(_mapper.ToModel).ToList();

    private TEntity FindEntity(string key)
        => _context.Items.FirstOrDefault(e => string.Equals(e.GetKey(), key, StringComparison.OrdinalIgnoreCase));

    
    private static T Execute<T>(Func<T> action)
    {
        try { return action(); }
        catch (DataAccessException ex) { throw new StorageException(ex.Message, ex); }
    }

    private static void Execute(Action action) => Execute(() => { action(); return 0; });

    private static string WithExtension(string path, SerializationType type)
        => Path.HasExtension(path) ? path : path + DataProviderFactory.ExtensionFor(type);

    private static SerializationType ToType(SerializationFormat format) => format switch
    {
        SerializationFormat.Binary => SerializationType.Binary,
        SerializationFormat.Custom => SerializationType.Custom,
        SerializationFormat.Xml => SerializationType.Xml,
        SerializationFormat.Json => SerializationType.Json,
        _ => throw new ArgumentOutOfRangeException(nameof(format), format, "Невідомий формат.")
    };

    private static SerializationFormat ToFormat(SerializationType type) => type switch
    {
        SerializationType.Binary => SerializationFormat.Binary,
        SerializationType.Custom => SerializationFormat.Custom,
        SerializationType.Xml => SerializationFormat.Xml,
        SerializationType.Json => SerializationFormat.Json,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Невідомий тип.")
    };
}
