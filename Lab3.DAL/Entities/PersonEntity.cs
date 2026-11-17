using System.Runtime.Serialization;

namespace Lab3.DAL.Entities;

/// <summary>Спільні дані осіб: ім'я, прізвище, кількість виконаних танців.</summary>
[Serializable]
public abstract class PersonEntity : Entity
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public int Dances { get; set; }

    protected PersonEntity() { }

    protected PersonEntity(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        FirstName = info.GetString(nameof(FirstName));
        LastName = info.GetString(nameof(LastName));
        Dances = info.GetInt32(nameof(Dances));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(FirstName), FirstName);
        info.AddValue(nameof(LastName), LastName);
        info.AddValue(nameof(Dances), Dances);
    }
}
