using System;
using System.Text;
namespace Cinema
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // що б укр літери відображались правильно 
            Console.OutputEncoding = Encoding.UTF8;
            //змінна яка керує виходом з програми 
            bool running = true;
            while (running)
            {
                Console.WriteLine("Кінотеатр");
                Console.WriteLine("1-довідка про програму");
                Console.WriteLine("2-вартість квитків");
                Console.WriteLine("3-можлива виручка");
                Console.WriteLine("0-вихід");
                Console.WriteLine("ваш вибір");
                //  ентер чи літера не повалять програму
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        // ShowRevenueTable(); — таблиця на два входи буде потім 
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        // пауза що б результат не зникав після очищення екрана
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }
        // Довідка 
        static void ShowAbout()
        {
            Console.WriteLine("Тема: Кінотеатр.");
            Console.WriteLine("Облік кіносеансів у залах кінотеатру.");
            Console.WriteLine("Сутності: Користувач, Сеанс, Зал (довідник).");
            Console.WriteLine("Головна сутність: Сеанс. Полів: 7.");
            Console.WriteLine();
            Console.WriteLine("Поля сутності «Сеанс»:");
            Console.WriteLine("  int      — код сеансу (призначає програма)");
            Console.WriteLine("  string   — назва фільму");
            Console.WriteLine("  int      — код залу (з довідника «Зал»)");
            Console.WriteLine("  DateTime — дата й час сеансу");
            Console.WriteLine("  double   — ціна квитка");
            Console.WriteLine("  int      — продано місць");
            Console.WriteLine("  int      — усього місць");
            Console.WriteLine();
            Console.WriteLine("Підсумкова величина: заповненість залів у відсотках");
            Console.WriteLine("і касовий збір за день.");
        }

        // таблиця сеанса 
        static void ShowCostTable()
        {
            double priceFrom = 75.0;   // початок проміжку грн
            double priceTo = 200.0;    // кінець проміжку грн
            double priceStep = 12.5;   // дробовий крок
            int soldSeats = 60;        // місць продано
            int totalSeats = 120;      // місць у залі

            Console.WriteLine("Касовий збір за сеанс залежно від ціни квитка");
            Console.WriteLine();
            Console.WriteLine($"{"Ціна, грн",12}{"Збір (60 міс.)",18}{"Збір (120 міс.)",18}");
            Console.WriteLine(new string('-', 48));

            // цикл малює рівну таблицю розрахунків
            for (double price = priceFrom; price <= priceTo + 1e-9; price += priceStep)
            {
                double soldRevenue = price * soldSeats;
                double fullRevenue = price * totalSeats;
                Console.WriteLine($"{price,12:F2}{soldRevenue,18:F2}{fullRevenue,18:F2}");
            }
        }
    }
}


