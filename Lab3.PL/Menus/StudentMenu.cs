using Lab3.BLL.Models;
using Lab3.BLL.Services;
using Lab3.BLL.Validation;
using Lab3.PL.Views;

namespace Lab3.PL.Menus;

internal sealed class StudentMenu : EntityMenu<StudentView, Student>
{
    private readonly StudentService _studentService;

    public StudentMenu(StudentService service) : base(service, new StudentViewMapper())
    {
        _studentService = service;
    }

    protected override string Title => "СТУДЕНТИ";
    protected override string DefaultFileName => "students";

    protected override StudentView ReadView() => new StudentView
    {
        LastName = ConsoleIO.ReadValid("Прізвище", Validator.NamePattern, "літери, 2-30 символів"),
        FirstName = ConsoleIO.ReadValid("Ім'я", Validator.NamePattern, "літери, 2-30 символів"),
        Course = int.Parse(ConsoleIO.ReadValid("Курс", Validator.CoursePattern, "число від 1 до 6")),
        StudentId = ConsoleIO.ReadValid("Студентський квиток", Validator.StudentIdPattern, "2 великі латинські літери + 6 цифр, напр. KB123456"),
        ArrivalCity = ConsoleIO.ReadValid("Місто прибуття", Validator.CityPattern, "літери, 2-40 символів"),
        Passport = ConsoleIO.ReadValid("Паспорт (серія і номер)", Validator.PassportPattern, "2 великі літери + 6 цифр, напр. AB123456")
    };

    protected override StudentView[] SampleViews() => new[]
    {
        new StudentView { LastName = "Коваленко", FirstName = "Олена", Course = 1, StudentId = "KB123456", ArrivalCity = "Львів", Passport = "AB123456" },
        new StudentView { LastName = "Мельник", FirstName = "Андрій", Course = 1, StudentId = "KB123457", ArrivalCity = "Київ", Passport = "AB123457" },
        new StudentView { LastName = "Шевченко", FirstName = "Марія", Course = 1, StudentId = "KB123458", ArrivalCity = "Харків", Passport = "AB123458" },
        new StudentView { LastName = "Бондаренко", FirstName = "Ігор", Course = 1, StudentId = "KB123459", ArrivalCity = "Одеса", Passport = "AB123459" },
        new StudentView { LastName = "Ткаченко", FirstName = "Софія", Course = 2, StudentId = "KB223460", ArrivalCity = "Київ", Passport = "AB123460" },
        new StudentView { LastName = "Кравченко", FirstName = "Максим", Course = 3, StudentId = "KB323461", ArrivalCity = "Дніпро", Passport = "AB123461" }
    };

    protected override MenuItem[] ExtraItems() => new[]
    {
        new MenuItem("Завдання варіанта: відсоток студентів 1-го курсу з інших міст", RunVariantTask)
    };

    private void RunVariantTask()
    {
        string city = ConsoleIO.ReadLine("Місто університету (Enter = Київ): ");
        if (city.Length == 0) city = "Київ";

        FirstYearReport report = _studentService.GetFirstYearFromOtherCities(city);

        Console.WriteLine($"Студентів 1-го курсу: {report.FirstYearCount}");
        Console.WriteLine($"З інших міст (не з міста {report.UniversityCity}): {report.FromOtherCitiesCount}");
        Console.WriteLine($"Відсоток: {report.Percent:F1}%");
        foreach (Student student in report.Students)
            Console.WriteLine("  " + Mapper.ToView(student).Describe());
    }
}
