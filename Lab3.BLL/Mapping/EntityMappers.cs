using Lab3.BLL.Models;
using Lab3.DAL.Entities;

namespace Lab3.BLL.Mapping;

/// <summary>Перетворення між моделлю BLL та сутністю DAL.</summary>
public interface IEntityMapper<TModel, TEntity>
{
    TModel ToModel(TEntity entity);
    TEntity ToEntity(TModel model);
}

public class StudentMapper : IEntityMapper<Student, StudentEntity>
{
    public Student ToModel(StudentEntity e)
        => new Student(e.LastName, e.FirstName, e.Course, e.StudentId, e.ArrivalCity, e.Passport, e.Dances);

    public StudentEntity ToEntity(Student m) => new StudentEntity
    {
        LastName = m.LastName, FirstName = m.FirstName, Course = m.Course, StudentId = m.StudentId,
        ArrivalCity = m.ArrivalCity, Passport = m.Passport, Dances = m.Dances
    };
}

public class TaxiDriverMapper : IEntityMapper<TaxiDriver, TaxiDriverEntity>
{
    public TaxiDriver ToModel(TaxiDriverEntity e)
        => new TaxiDriver(e.LastName, e.FirstName, e.DriverLicense, e.Dances);

    public TaxiDriverEntity ToEntity(TaxiDriver m) => new TaxiDriverEntity
    {
        LastName = m.LastName, FirstName = m.FirstName, DriverLicense = m.DriverLicense, Dances = m.Dances
    };
}

public class AcrobatMapper : IEntityMapper<Acrobat, AcrobatEntity>
{
    public Acrobat ToModel(AcrobatEntity e)
        => new Acrobat(e.LastName, e.FirstName, e.CertificateId, e.ExperienceYears, e.Dances);

    public AcrobatEntity ToEntity(Acrobat m) => new AcrobatEntity
    {
        LastName = m.LastName, FirstName = m.FirstName, CertificateId = m.CertificateId,
        ExperienceYears = m.ExperienceYears, Dances = m.Dances
    };
}
