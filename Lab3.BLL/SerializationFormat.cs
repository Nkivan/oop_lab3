namespace Lab3.BLL;

/// <summary>
/// Тип серіалізації на рівні BLL. Рівень представлення знає лише його і нічого не знає
/// про DAL-тип SerializationType.
/// </summary>
public enum SerializationFormat
{
    Binary,
    Custom,
    Xml,
    Json
}
