using Lab3.BLL.Models;
using Lab3.DAL;
using Lab3.DAL.Entities;

namespace Lab3.BLL.Services;

/// <summary>
/// Складає сервіси разом із їхніми контекстами. Завдяки цьому рівень представлення
/// не посилається на DAL і не знає про його класи.
/// </summary>
public static class ServiceFactory
{
    public static StudentService CreateStudentService()
        => new StudentService(new EntityContext<StudentEntity>());

    public static IEntityService<TaxiDriver> CreateTaxiDriverService()
        => new TaxiDriverService(new EntityContext<TaxiDriverEntity>());

    public static IEntityService<Acrobat> CreateAcrobatService()
        => new AcrobatService(new EntityContext<AcrobatEntity>());
}
