using System.Runtime.Serialization;

namespace Lab3.DAL.Entities;


[Serializable]
public abstract class Entity : ISerializable
{
    protected Entity() { }

    
    protected Entity(SerializationInfo info, StreamingContext context) { }

    
    public abstract string GetKey();

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context) { }
}
