using Lab3.BLL;
using Lab3.BLL.Exceptions;
using Lab3.BLL.Models;
using Lab3.BLL.Services;
using Lab3.PL.Views;

namespace Lab3.PL.Menus;

internal sealed record MenuItem(string Title, Action Action);


internal abstract class EntityMenu<TView, TModel>
    where TView : IView
    where TModel : Person
{
    private readonly IViewMapper<TView, TModel> _mapper;

    protected IEntityService<TModel> Service { get; }
    protected IViewMapper<TView, TModel> Mapper => _mapper;

    protected EntityMenu(IEntityService<TModel> service, IViewMapper<TView, TModel> mapper)
    {
        Service = service;
        _mapper = mapper;
    }

    protected abstract string Title { get; }
    protected abstract string DefaultFileName { get; }
    protected abstract TView ReadView();
    protected abstract TView[] SampleViews();
    protected virtual MenuItem[] ExtraItems() => Array.Empty<MenuItem>();

    public void Run()
    {
        MenuItem[] items = BuildItems();
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"===== {Title} =====");
            Console.WriteLine(StatusLine());
            for (int i = 0; i < items.Length; i++)
                Console.WriteLine($"{i + 1}. {items[i].Title}");
            Console.WriteLine("0. Назад");

            string choice = ConsoleIO.ReadLine("Ваш вибір: ");
            if (choice == "0") return;
            if (!int.TryParse(choice, out int number) || number < 1 || number > items.Length)
            {
                Console.WriteLine("Невідома команда.");
                continue;
            }

            try
            {
                items[number - 1].Action();
            }
            catch (BllException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            catch (Exception ex) when (ex is not InputEndedException)
            {
                Console.WriteLine($"Неочікувана помилка: {ex.Message}");
            }
        }
    }

    private MenuItem[] BuildItems()
    {
        MenuItem[] common =
        {
            new MenuItem("Відкрити або створити файл БД (вибір формату та імені)", OpenStorage),
            new MenuItem("Показати всіх", ShowAll),
            new MenuItem("Додати", Add),
            new MenuItem("Видалити за ключем", Remove),
            new MenuItem("Знайти за прізвищем", FindByLastName),
            new MenuItem("Знайти за ключем", FindByKey),
            new MenuItem("Виконати дію: танцювати", Dance),
            new MenuItem("Зберегти в інший файл або формат", SaveAs),
            new MenuItem("Закрити файл БД", CloseStorage),
            new MenuItem("Видалити файл БД", DeleteStorage),
            new MenuItem("Додати тестові дані", AddSamples)
        };
        return common.Concat(ExtraItems()).ToArray();
    }

    private string StatusLine()
        => Service.IsOpen
            ? $"Файл: {Service.CurrentFile} ({FormatName(Service.CurrentFormat.Value)})"
            : "Файл БД не відкрито";

    

    private void OpenStorage()
    {
        SerializationFormat format = ChooseFormat();
        string path = ReadFileName();
        bool existed = Service.OpenStorage(path, format);
        Console.WriteLine(existed
            ? $"Файл відкрито: {Service.CurrentFile}"
            : $"Створено новий порожній файл: {Service.CurrentFile}");
    }

    private void ShowAll()
    {
        IReadOnlyList<TModel> all = Service.GetAll();
        if (all.Count == 0) { Console.WriteLine("База порожня."); return; }
        foreach (TModel model in all) Console.WriteLine(_mapper.ToView(model).Describe());
    }

    private void Add()
    {
        TView view = ReadView();
        Service.Add(_mapper.ToModel(view));
        Console.WriteLine("Додано.");
    }

    private void Remove()
    {
        Service.Remove(ConsoleIO.ReadLine("Ключ запису: "));
        Console.WriteLine("Видалено.");
    }

    private void FindByLastName()
    {
        IReadOnlyList<TModel> found = Service.FindByLastName(ConsoleIO.ReadLine("Прізвище: "));
        if (found.Count == 0) { Console.WriteLine("Нічого не знайдено."); return; }
        foreach (TModel model in found) Console.WriteLine(_mapper.ToView(model).Describe());
    }

    private void FindByKey()
        => Console.WriteLine(_mapper.ToView(Service.GetByKey(ConsoleIO.ReadLine("Ключ запису: "))).Describe());

    private void Dance()
    {
        string key = ConsoleIO.ReadLine("Ключ запису: ");
        Service.Dance(key);
        Console.WriteLine("Виконано. Новий стан:");
        Console.WriteLine(_mapper.ToView(Service.GetByKey(key)).Describe());
    }

    private void SaveAs()
    {
        SerializationFormat format = ChooseFormat();
        string path = ReadFileName();
        Service.SaveAs(path, format);
        Console.WriteLine("Дані записано. Поточний файл не змінено.");
    }

    private void CloseStorage()
    {
        Service.CloseStorage();
        Console.WriteLine("Файл закрито.");
    }

    private void DeleteStorage()
    {
        string confirm = ConsoleIO.ReadLine($"Видалити файл {Service.CurrentFile}? (так/ні): ");
        if (!string.Equals(confirm, "так", StringComparison.OrdinalIgnoreCase)) { Console.WriteLine("Скасовано."); return; }
        Service.DeleteStorage();
        Console.WriteLine("Файл видалено.");
    }

    private void AddSamples()
    {
        int added = 0, skipped = 0;
        foreach (TView view in SampleViews())
        {
            try { Service.Add(_mapper.ToModel(view)); added++; }
            catch (DuplicateEntityException) { skipped++; }
        }
        Console.WriteLine($"Додано: {added}, пропущено (вже є): {skipped}.");
    }

   

    private static SerializationFormat ChooseFormat()
    {
        int index = ConsoleIO.Choose("Формат серіалізації:", new[]
        {
            "Бінарна",
            "Користувацька (ISerializable)",
            "XML",
            "JSON"
        });
        return (SerializationFormat)index;
    }

    private string ReadFileName()
    {
        string name = ConsoleIO.ReadLine($"Ім'я файлу або шлях, Enter = {DefaultFileName} (розширення додається само): ");
        return name.Length == 0 ? DefaultFileName : name;
    }

    private static string FormatName(SerializationFormat format) => format switch
    {
        SerializationFormat.Binary => "бінарний",
        SerializationFormat.Custom => "користувацький",
        SerializationFormat.Xml => "XML",
        _ => "JSON"
    };
}
