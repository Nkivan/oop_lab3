using System.Runtime.Serialization;

namespace Lab3.DAL.Entities;

[Serializable]
public class StudentEntity : PersonEntity
{
    public int Course { get; set; }
    public string StudentId { get; set; } = "";
    public string ArrivalCity { get; set; } = "";
    public string Passport { get; set; } = "";

    public StudentEntity() { }   

    protected StudentEntity(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        Course = info.GetInt32(nameof(Course));
        StudentId = info.GetString(nameof(StudentId));
        ArrivalCity = info.GetString(nameof(ArrivalCity));
        Passport = info.GetString(nameof(Passport));
    }

    public override string GetKey() => StudentId;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(Course), Course);
        info.AddValue(nameof(StudentId), StudentId);
        info.AddValue(nameof(ArrivalCity), ArrivalCity);
        info.AddValue(nameof(Passport), Passport);
    }
}
