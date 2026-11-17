using System.Runtime.Serialization;

namespace Lab3.DAL.Entities;

[Serializable]
public class TaxiDriverEntity : PersonEntity
{
    public string DriverLicense { get; set; } = "";

    public TaxiDriverEntity() { }

    protected TaxiDriverEntity(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        DriverLicense = info.GetString(nameof(DriverLicense));
    }

    public override string GetKey() => DriverLicense;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(DriverLicense), DriverLicense);
    }
}
