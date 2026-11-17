using Lab3.BLL.Validation;

namespace Lab3.BLL.Models;

public class Acrobat : Person
{
    private string _certificateId = "";
    private int _experienceYears;

    public string CertificateId
    {
        get => _certificateId;
        set { Validator.Require(Validator.CertificatePattern, value, "Сертифікат акробата"); _certificateId = value; }
    }

    public int ExperienceYears
    {
        get => _experienceYears;
        set { Validator.Require(Validator.ExperiencePattern, value.ToString(), "Стаж"); _experienceYears = value; }
    }

    public override string Key => CertificateId;

    public Acrobat(string lastName, string firstName, string certificateId, int experienceYears, int dances = 0)
        : base(lastName, firstName, dances)
    {
        CertificateId = certificateId;
        ExperienceYears = experienceYears;
    }
}
