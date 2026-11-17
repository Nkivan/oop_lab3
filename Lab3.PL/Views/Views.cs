namespace Lab3.PL.Views;


internal interface IView
{
    string Describe();
}

internal class StudentView : IView
{
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public int Course { get; set; }
    public string StudentId { get; set; } = "";
    public string ArrivalCity { get; set; } = "";
    public string Passport { get; set; } = "";
    public int Dances { get; set; }

    public string Describe()
        => $"[Студент] {LastName} {FirstName}, курс {Course}, квиток {StudentId}, " +
           $"місто прибуття: {ArrivalCity}, паспорт {Passport}, танців: {Dances}";
}

internal class TaxiDriverView : IView
{
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string DriverLicense { get; set; } = "";
    public int Dances { get; set; }

    public string Describe()
        => $"[Таксист] {LastName} {FirstName}, посвідчення {DriverLicense}, танців: {Dances}";
}

internal class AcrobatView : IView
{
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string CertificateId { get; set; } = "";
    public int ExperienceYears { get; set; }
    public int Dances { get; set; }

    public string Describe()
        => $"[Акробат] {LastName} {FirstName}, сертифікат {CertificateId}, стаж {ExperienceYears} р., танців: {Dances}";
}
