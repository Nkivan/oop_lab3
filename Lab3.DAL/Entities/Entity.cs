using System.Runtime.Serialization;

namespace Lab3.DAL.Entities;

/// <summary>
/// Базова сутність рівня DAL. Реалізує ISerializable: це механізм користувацької серіалізації,
/// де сам об'єкт вирішує, які дані й під якими іменами віддати у SerializationInfo.
/// Клас Entity (і похідні від нього) існують лише на рівні DAL.
/// </summary>
[Serializable]
public abstract class Entity : ISerializable
{
    protected Entity() { }

    // Конструктор користувацької десеріалізації: його викликає CustomDataProvider через рефлексію.
    protected Entity(SerializationInfo info, StreamingContext context) { }

    /// <summary>Унікальний ключ сутності (метод, а не властивість, щоб XML/JSON його не записували).</summary>
    public abstract string GetKey();

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { }
}
