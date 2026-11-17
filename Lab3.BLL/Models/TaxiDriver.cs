using Lab3.BLL.Validation;

namespace Lab3.BLL.Models;

public class TaxiDriver : Person
{
    private string _driverLicense = "";

    public string DriverLicense
    {
        get => _driverLicense;
        set { Validator.Require(Validator.DriverLicensePattern, value, "Посвідчення водія"); _driverLicense = value; }
    }

    public override string Key => DriverLicense;

    public TaxiDriver(string lastName, string firstName, string driverLicense, int dances = 0)
        : base(lastName, firstName, dances)
    {
        DriverLicense = driverLicense;
    }
}
