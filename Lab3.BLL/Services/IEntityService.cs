using Lab3.BLL.Models;

namespace Lab3.BLL.Services;


public interface IEntityService<TModel> where TModel : Person
{
    bool IsOpen { get; }
    string CurrentFile { get; }
    SerializationFormat? CurrentFormat { get; }

    
    bool OpenStorage(string path, SerializationFormat format);
    void SaveAs(string path, SerializationFormat format);
    void CloseStorage();
    void DeleteStorage();

    IReadOnlyList<TModel> GetAll();
    void Add(TModel model);
    void Remove(string key);
    TModel GetByKey(string key);
    IReadOnlyList<TModel> FindByLastName(string lastName);

    
    void Dance(string key);
}
