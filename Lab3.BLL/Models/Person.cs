using Lab3.BLL.Exceptions;
using Lab3.BLL.Validation;

namespace Lab3.BLL.Models;

/// <summary>Власна модель рівня BLL (не залежить від класів DAL).</summary>
public abstract class Person : IDancer
{
    private string _firstName = "";
    private string _lastName = "";

    public string FirstName
    {
        get => _firstName;
        set { Validator.Require(Validator.NamePattern, value, "Ім'я"); _firstName = value; }
    }

    public string LastName
    {
        get => _lastName;
        set { Validator.Require(Validator.NamePattern, value, "Прізвище"); _lastName = value; }
    }

    public int Dances { get; private set; }

    /// <summary>Унікальний ключ конкретного типу особи.</summary>
    public abstract string Key { get; }

    protected Person(string lastName, string firstName, int dances)
    {
        LastName = lastName;
        FirstName = firstName;
        if (dances < 0) throw new InvalidEntityDataException("Кількість танців не може бути від'ємною.");
        Dances = dances;
    }

    /// <summary>Змінює стан об'єкта.</summary>
    public void Dance() => Dances++;
}
