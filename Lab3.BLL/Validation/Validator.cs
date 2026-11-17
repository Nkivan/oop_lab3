using System.Text.RegularExpressions;
using Lab3.BLL.Exceptions;

namespace Lab3.BLL.Validation;

/// <summary>Правила предметної області (регулярні вирази). Використовуються і моделями, і рівнем представлення.</summary>
public static class Validator
{
    public const string NamePattern = @"^[A-Za-zА-Яа-яІіЇїЄєҐґ'’\-]{2,30}$";
    public const string CityPattern = @"^[A-Za-zА-Яа-яІіЇїЄєҐґ'’\- ]{2,40}$";
    public const string CoursePattern = @"^[1-6]$";
    public const string StudentIdPattern = @"^[A-Z]{2}\d{6}$";
    public const string PassportPattern = @"^[A-ZА-ЯІЇЄҐ]{2}\d{6}$";
    public const string DriverLicensePattern = @"^[A-Z]{3}\d{6}$";
    public const string CertificatePattern = @"^AC\d{5}$";
    public const string ExperiencePattern = @"^\d{1,2}$";

    public static bool IsValid(string pattern, string value)
        => value != null && Regex.IsMatch(value, pattern);

    public static void Require(string pattern, string value, string field)
    {
        if (!IsValid(pattern, value))
            throw new InvalidEntityDataException($"Недопустиме значення '{value}' для поля '{field}'.");
    }
}
