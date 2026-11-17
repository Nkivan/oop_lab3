using Lab3.BLL.Models;

namespace Lab3.BLL.Services;

/// <summary>Контракт сервісу бізнес-логіки: рівень представлення залежить від нього, а не від реалізації.</summary>
public interface IEntityService<TModel> where TModel : Person
{
    bool IsOpen { get; }
    string CurrentFile { get; }
    SerializationFormat? CurrentFormat { get; }

    /// <summary>Відкриває або створює файл БД. Повертає true, якщо файл уже існував.</summary>
    bool OpenStorage(string path, SerializationFormat format);
    void SaveAs(string path, SerializationFormat format);
    void CloseStorage();
    void DeleteStorage();

    IReadOnlyList<TModel> GetAll();
    void Add(TModel model);
    void Remove(string key);
    TModel GetByKey(string key);
    IReadOnlyList<TModel> FindByLastName(string lastName);

    /// <summary>Виконує вміння «танцювати» та зберігає новий стан.</summary>
    void Dance(string key);
}
