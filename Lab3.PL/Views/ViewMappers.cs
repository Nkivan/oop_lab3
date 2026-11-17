using Lab3.BLL.Models;

namespace Lab3.PL.Views;


internal interface IViewMapper<TView, TModel>
{
    TView ToView(TModel model);
    TModel ToModel(TView view);
}

internal class StudentViewMapper : IViewMapper<StudentView, Student>
{
    public StudentView ToView(Student m) => new StudentView
    {
        LastName = m.LastName, FirstName = m.FirstName, Course = m.Course, StudentId = m.StudentId,
        ArrivalCity = m.ArrivalCity, Passport = m.Passport, Dances = m.Dances
    };

    public Student ToModel(StudentView v)
        => new Student(v.LastName, v.FirstName, v.Course, v.StudentId, v.ArrivalCity, v.Passport, v.Dances);
}

internal class TaxiDriverViewMapper : IViewMapper<TaxiDriverView, TaxiDriver>
{
    public TaxiDriverView ToView(TaxiDriver m) => new TaxiDriverView
    {
        LastName = m.LastName, FirstName = m.FirstName, DriverLicense = m.DriverLicense, Dances = m.Dances
    };

    public TaxiDriver ToModel(TaxiDriverView v)
        => new TaxiDriver(v.LastName, v.FirstName, v.DriverLicense, v.Dances);
}

internal class AcrobatViewMapper : IViewMapper<AcrobatView, Acrobat>
{
    public AcrobatView ToView(Acrobat m) => new AcrobatView
    {
        LastName = m.LastName, FirstName = m.FirstName, CertificateId = m.CertificateId,
        ExperienceYears = m.ExperienceYears, Dances = m.Dances
    };

    public Acrobat ToModel(AcrobatView v)
        => new Acrobat(v.LastName, v.FirstName, v.CertificateId, v.ExperienceYears, v.Dances);
}
