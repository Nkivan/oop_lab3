using System.Text;
using Lab3.BLL.Services;
using Lab3.PL.Menus;

namespace Lab3.PL;


public static class Menu
{
    public static void MainMenu()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        StudentMenu students = new StudentMenu(ServiceFactory.CreateStudentService());
        TaxiDriverMenu taxiDrivers = new TaxiDriverMenu(ServiceFactory.CreateTaxiDriverService());
        AcrobatMenu acrobats = new AcrobatMenu(ServiceFactory.CreateAcrobatService());

        try
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== ГОЛОВНЕ МЕНЮ =====");
                Console.WriteLine("1. Студенти (в т.ч. завдання варіанта)");
                Console.WriteLine("2. Таксисти");
                Console.WriteLine("3. Акробати");
                Console.WriteLine("0. Вихід");

                switch (ConsoleIO.ReadLine("Ваш вибір: "))
                {
                    case "1": students.Run(); break;
                    case "2": taxiDrivers.Run(); break;
                    case "3": acrobats.Run(); break;
                    case "0": return;
                    default: Console.WriteLine("Невідома команда."); break;
                }
            }
        }
        catch (InputEndedException)
        {
            
        }
    }
}
