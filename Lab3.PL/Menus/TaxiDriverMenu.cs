using Lab3.BLL.Models;
using Lab3.BLL.Services;
using Lab3.BLL.Validation;
using Lab3.PL.Views;

namespace Lab3.PL.Menus;

internal sealed class TaxiDriverMenu : EntityMenu<TaxiDriverView, TaxiDriver>
{
    public TaxiDriverMenu(IEntityService<TaxiDriver> service) : base(service, new TaxiDriverViewMapper()) { }

    protected override string Title => "ТАКСИСТИ";
    protected override string DefaultFileName => "taxidrivers";

    protected override TaxiDriverView ReadView() => new TaxiDriverView
    {
        LastName = ConsoleIO.ReadValid("Прізвище", Validator.NamePattern, "літери, 2-30 символів"),
        FirstName = ConsoleIO.ReadValid("Ім'я", Validator.NamePattern, "літери, 2-30 символів"),
        DriverLicense = ConsoleIO.ReadValid("Посвідчення водія", Validator.DriverLicensePattern, "3 великі латинські літери + 6 цифр, напр. ABC123456")
    };

    protected override TaxiDriverView[] SampleViews() => new[]
    {
        new TaxiDriverView { LastName = "Сидоренко", FirstName = "Петро", DriverLicense = "ABC123456" },
        new TaxiDriverView { LastName = "Гнатюк", FirstName = "Василь", DriverLicense = "ABC654321" },
        new TaxiDriverView { LastName = "Руденко", FirstName = "Дмитро", DriverLicense = "ABC111222" }
    };
}
