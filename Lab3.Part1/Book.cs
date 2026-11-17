using System.Globalization;
using System.Runtime.Serialization;

namespace Lab3.Part1;


[Serializable]
public class Book : ISerializable, IEquatable<Book>
{
    private int _serialNumber;
    private string _title = "";
    private int _publicationYear;
    private decimal _pricePerCopy;
    private int _copies;

    public int SerialNumber
    {
        get => _serialNumber;
        set { if (value <= 0) throw new ArgumentException("Серійний номер має бути додатним."); _serialNumber = value; }
    }

    public string Title
    {
        get => _title;
        set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Назва не може бути порожньою."); _title = value; }
    }

    public int PublicationYear
    {
        get => _publicationYear;
        set { if (value < 1450 || value > DateTime.Now.Year) throw new ArgumentException("Некоректний рік видання."); _publicationYear = value; }
    }

    public decimal PricePerCopy
    {
        get => _pricePerCopy;
        set { if (value < 0) throw new ArgumentException("Вартість не може бути від'ємною."); _pricePerCopy = value; }
    }

    public int Copies
    {
        get => _copies;
        set { if (value < 0) throw new ArgumentException("Кількість примірників не може бути від'ємною."); _copies = value; }
    }

    public Book() { }   

    public Book(int serialNumber, string title, int publicationYear, decimal pricePerCopy, int copies)
    {
        SerialNumber = serialNumber;
        Title = title;
        PublicationYear = publicationYear;
        PricePerCopy = pricePerCopy;
        Copies = copies;
    }

    

    protected Book(SerializationInfo info, StreamingContext context)
    {
        SerialNumber = info.GetInt32(nameof(SerialNumber));
        Title = info.GetString(nameof(Title));
        PublicationYear = info.GetInt32(nameof(PublicationYear));
        PricePerCopy = decimal.Parse(info.GetString(nameof(PricePerCopy)), CultureInfo.InvariantCulture);
        Copies = info.GetInt32(nameof(Copies));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(SerialNumber), SerialNumber);
        info.AddValue(nameof(Title), Title);
        info.AddValue(nameof(PublicationYear), PublicationYear);

        info.AddValue(nameof(PricePerCopy), PricePerCopy.ToString(CultureInfo.InvariantCulture));
        info.AddValue(nameof(Copies), Copies);
    }


    public void IncreasePrice(decimal percent)
    {
        if (percent <= -100) throw new ArgumentException("Відсоток має бути більшим за -100.");
        PricePerCopy = Math.Round(PricePerCopy * (1 + percent / 100m), 2);
    }

  
    public decimal TotalCost() => PricePerCopy * Copies;

  
    public string GetInfo()
        => $"№{SerialNumber}: «{Title}», {PublicationYear} р., {PricePerCopy.ToString("F2", CultureInfo.InvariantCulture)} грн × {Copies} = " +
           $"{TotalCost().ToString("F2", CultureInfo.InvariantCulture)} грн";

    public override string ToString() => GetInfo();

  
    public bool Equals(Book other)
        => other != null && SerialNumber == other.SerialNumber && Title == other.Title &&
           PublicationYear == other.PublicationYear && PricePerCopy == other.PricePerCopy && Copies == other.Copies;

    public override bool Equals(object obj) => Equals(obj as Book);
    public override int GetHashCode() => HashCode.Combine(SerialNumber, Title, PublicationYear, PricePerCopy, Copies);
}
