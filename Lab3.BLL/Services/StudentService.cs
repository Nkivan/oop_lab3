using Lab3.BLL.Mapping;
using Lab3.BLL.Models;
using Lab3.BLL.Validation;
using Lab3.DAL;
using Lab3.DAL.Entities;

namespace Lab3.BLL.Services;

/// <summary>Результат завдання варіанта 2.</summary>
public class FirstYearReport
{
    public string UniversityCity { get; init; } = "";
    public int FirstYearCount { get; init; }
    public int FromOtherCitiesCount { get; init; }
    public double Percent { get; init; }
    public IReadOnlyList<Student> Students { get; init; } = Array.Empty<Student>();
}

public class StudentService : EntityService<Student, StudentEntity>
{
    public StudentService(EntityContext<StudentEntity> context) : base(context, new StudentMapper()) { }

    /// <summary>
    /// Обчислює відсоток студентів 1-го курсу, які приїхали з інших міст (не з міста університету),
    /// і повертає їхні дані. Дані спершу перечитуються з файлу.
    /// </summary>
    public FirstYearReport GetFirstYearFromOtherCities(string universityCity)
    {
        EnsureOpen();
        Validator.Require(Validator.CityPattern, universityCity, "Місто університету");
        ReloadFromFile();

        List<Student> firstYear = AllModels().Where(s => s.Course == 1).ToList();
        List<Student> fromOther = firstYear
            .Where(s => !string.Equals(s.ArrivalCity, universityCity, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return new FirstYearReport
        {
            UniversityCity = universityCity,
            FirstYearCount = firstYear.Count,
            FromOtherCitiesCount = fromOther.Count,
            Percent = firstYear.Count == 0 ? 0 : fromOther.Count * 100.0 / firstYear.Count,
            Students = fromOther
        };
    }
}
