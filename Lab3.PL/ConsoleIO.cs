using Lab3.BLL.Validation;

namespace Lab3.PL;

/// <summary>Сигналізує про кінець вхідного потоку (наприклад, ввід перенаправлено з файлу).</summary>
internal sealed class InputEndedException : Exception { }

/// <summary>Усе читання з консолі та базове форматування виводу (рівень представлення).</summary>
internal static class ConsoleIO
{
    public static string ReadLine(string prompt)
    {
        Console.Write(prompt);
        string line = Console.ReadLine();
        if (line == null) throw new InputEndedException();
        return line.Trim();
    }

    /// <summary>Запитує значення, поки воно не відповідатиме регулярному виразу.</summary>
    public static string ReadValid(string label, string pattern, string hint)
    {
        while (true)
        {
            string value = ReadLine($"{label} ({hint}): ");
            if (Validator.IsValid(pattern, value)) return value;
            Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
        }
    }

    public static int ReadNumber(string prompt, int min, int max)
    {
        while (true)
        {
            string text = ReadLine($"{prompt} ({min}-{max}): ");
            if (int.TryParse(text, out int value) && value >= min && value <= max) return value;
            Console.WriteLine("  Некоректний вибір.");
        }
    }

    /// <summary>Показує пронумеровані варіанти й повертає індекс обраного (з нуля).</summary>
    public static int Choose(string title, string[] options)
    {
        Console.WriteLine(title);
        for (int i = 0; i < options.Length; i++)
            Console.WriteLine($"  {i + 1}. {options[i]}");
        return ReadNumber("Номер", 1, options.Length) - 1;
    }
}
