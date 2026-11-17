using Lab3.BLL.Models;
using Lab3.BLL.Services;
using Lab3.BLL.Validation;
using Lab3.PL.Views;

namespace Lab3.PL.Menus;

internal sealed class AcrobatMenu : EntityMenu<AcrobatView, Acrobat>
{
    public AcrobatMenu(IEntityService<Acrobat> service) : base(service, new AcrobatViewMapper()) { }

    protected override string Title => "АКРОБАТИ";
    protected override string DefaultFileName => "acrobats";

    protected override AcrobatView ReadView() => new AcrobatView
    {
        LastName = ConsoleIO.ReadValid("Прізвище", Validator.NamePattern, "літери, 2-30 символів"),
        FirstName = ConsoleIO.ReadValid("Ім'я", Validator.NamePattern, "літери, 2-30 символів"),
        CertificateId = ConsoleIO.ReadValid("Сертифікат акробата", Validator.CertificatePattern, "AC + 5 цифр, напр. AC12345"),
        ExperienceYears = int.Parse(ConsoleIO.ReadValid("Стаж (років)", Validator.ExperiencePattern, "число від 0 до 99"))
    };

    protected override AcrobatView[] SampleViews() => new[]
    {
        new AcrobatView { LastName = "Лисенко", FirstName = "Оксана", CertificateId = "AC12345", ExperienceYears = 4 },
        new AcrobatView { LastName = "Мороз", FirstName = "Тарас", CertificateId = "AC54321", ExperienceYears = 7 },
        new AcrobatView { LastName = "Савчук", FirstName = "Ірина", CertificateId = "AC11111", ExperienceYears = 2 }
    };
}
