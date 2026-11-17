using System.Runtime.Serialization;

namespace Lab3.DAL.Entities;

[Serializable]
public class AcrobatEntity : PersonEntity
{
    public string CertificateId { get; set; } = "";
    public int ExperienceYears { get; set; }

    public AcrobatEntity() { }

    protected AcrobatEntity(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        CertificateId = info.GetString(nameof(CertificateId));
        ExperienceYears = info.GetInt32(nameof(ExperienceYears));
    }

    public override string GetKey() => CertificateId;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(CertificateId), CertificateId);
        info.AddValue(nameof(ExperienceYears), ExperienceYears);
    }
}
