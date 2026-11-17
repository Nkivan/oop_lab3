using Lab3.BLL.Validation;

namespace Lab3.BLL.Models;

public class Student : Person
{
    private int _course;
    private string _studentId = "";
    private string _arrivalCity = "";
    private string _passport = "";

    public int Course
    {
        get => _course;
        set { Validator.Require(Validator.CoursePattern, value.ToString(), "Курс"); _course = value; }
    }

    public string StudentId
    {
        get => _studentId;
        set { Validator.Require(Validator.StudentIdPattern, value, "Студентський квиток"); _studentId = value; }
    }

    public string ArrivalCity
    {
        get => _arrivalCity;
        set { Validator.Require(Validator.CityPattern, value, "Місто прибуття"); _arrivalCity = value; }
    }

    public string Passport
    {
        get => _passport;
        set { Validator.Require(Validator.PassportPattern, value, "Паспорт"); _passport = value; }
    }

    public override string Key => StudentId;

    public Student(string lastName, string firstName, int course, string studentId,
                   string arrivalCity, string passport, int dances = 0)
        : base(lastName, firstName, dances)
    {
        Course = course;
        StudentId = studentId;
        ArrivalCity = arrivalCity;
        Passport = passport;
    }
}
