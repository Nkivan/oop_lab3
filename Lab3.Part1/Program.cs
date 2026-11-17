using System.Text;

namespace Lab3.Part1;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // Файли складаються в папку output поруч із програмою
        string folder = Path.Combine(AppContext.BaseDirectory, "output");
        Directory.CreateDirectory(folder);

        Console.WriteLine("################ ЧАСТИНА 1: дослідження серіалізації ################");
        Console.WriteLine();

        // Завдання 1-2: масив із 4 об'єктів
        Book[] original = CreateBooks();
        Console.WriteLine("Вихідний масив Book[] (4 об'єкти):");
        foreach (Book book in original) Console.WriteLine("   " + book);

        // Завдання 4: така сама колекція List<Book> для порівняння
        List<Book> originalList = new List<Book>(original);

        IBookSerializer[] serializers =
        {
            new BinaryBookSerializer(),
            new CustomBookSerializer(),
            new XmlBookSerializer(),
            new JsonBookSerializer()
        };

        foreach (IBookSerializer serializer in serializers)
            Demonstrate(serializer, folder, original, originalList);

        PrintConclusion();
        Console.WriteLine();
        Console.WriteLine($"Усі файли збережено в: {folder}");
        Console.WriteLine("Натисніть Enter для завершення...");
        Console.ReadLine();
    }

    private static void Demonstrate(IBookSerializer serializer, string folder, Book[] array, List<Book> list)
    {
        Console.WriteLine();
        Console.WriteLine($"=== {serializer.Name} ===");

        // Завдання 2-3: серіалізація масиву у файл і відновлення в НОВИЙ масив
        string arrayPath = Path.Combine(folder, "books_array" + serializer.Extension);
        serializer.Serialize(arrayPath, array);
        Book[] restoredArray = serializer.Deserialize<Book[]>(arrayPath);

        Console.WriteLine($"Масив:    записано {new FileInfo(arrayPath).Length} байт у {Path.GetFileName(arrayPath)}");
        Console.WriteLine($"          відновлено в новий масив ({restoredArray.Length} шт.), " +
                          $"не той самий об'єкт: {!ReferenceEquals(array, restoredArray)}, " +
                          $"значення збігаються: {array.SequenceEqual(restoredArray)}");

        // Завдання 4: серіалізація колекції List<Book> та порівняння з масивом
        string listPath = Path.Combine(folder, "books_list" + serializer.Extension);
        serializer.Serialize(listPath, list);
        List<Book> restoredList = serializer.Deserialize<List<Book>>(listPath);

        Console.WriteLine($"Колекція: записано {new FileInfo(listPath).Length} байт у {Path.GetFileName(listPath)}");
        Console.WriteLine($"          відновлено в нову List<Book> ({restoredList.Count} шт.), " +
                          $"значення збігаються: {list.SequenceEqual(restoredList)}");

        bool sameFiles = File.ReadAllBytes(arrayPath).SequenceEqual(File.ReadAllBytes(listPath));
        Console.WriteLine($"Файли масиву й колекції побайтово однакові: {sameFiles}");

        if (serializer.Extension != ".bin")
        {
            Console.WriteLine("Початок файлу масиву:");
            foreach (string line in File.ReadLines(arrayPath).Take(8))
                Console.WriteLine("   | " + line);
        }
    }

    private static void PrintConclusion()
    {
        Console.WriteLine();
        Console.WriteLine("=== Порівняння масиву й колекції ===");
        Console.WriteLine("У файлі масив і List<Book> виглядають однаково: серіалізатор записує послідовність елементів.");
        Console.WriteLine("Різниця проявляється після відновлення: Book[] має фіксований розмір,");
        Console.WriteLine("а List<Book> можна розширювати (Add) і скорочувати (Remove) без створення нового об'єкта.");

        List<Book> demo = new List<Book>(CreateBooks());
        demo.Add(new Book(999, "Нова книга", 2024, 100m, 10));
        Console.WriteLine($"Приклад: після Add до List<Book> стало {demo.Count} книг, а в масиві так зробити не можна.");
    }

    // Чотири книги за умовою варіанта 2
    private static Book[] CreateBooks() => new[]
    {
        new Book(305, "Кобзар", 1840, 250.00m, 100),
        new Book(102, "Лісова пісня", 1911, 180.50m, 50),
        new Book(407, "Тіні забутих предків", 1911, 150.00m, 80),
        new Book(210, "Захар Беркут", 1883, 200.00m, 30)
    };
}
