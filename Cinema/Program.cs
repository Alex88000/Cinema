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
                ShowMenu();                         // було 6 рядків WriteLine 
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)                     //кожен case в один рядок 
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowRevenueTable(); Pause(); break;
                    case "0": running = false; Console.WriteLine("До побачення."); break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }
        // Нове меню 
        static void ShowMenu()
        {
            Console.WriteLine("Кінотеатр");
            Console.WriteLine("1-довідка про програму");
            Console.WriteLine("2-вартість квитків");
            Console.WriteLine("3-можлива виручка");
            Console.WriteLine("0-вихід");
            Console.Write("Ваш вибір: ");
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
        // рахує заповненість у % нічого не виводить
        static double CalculateOccupancy(int sold, int total)
        {
            if (total <= 0)
            {
                return 0;      // рання відмова, на нуль ділити не можна
            }
            return 100.0 * sold / total;
        }

        static double CalculateRevenue(double price, int seats)
        {
            return price * seats;
        }

        // таблиця сеанса 
        static void ShowCostTable()
        {
            double priceFrom = 75.0;   // початок проміжку грн
            double priceTo = 200.0;    // кінець проміжку грн
            double priceStep = 12.5;   // дробовий крок
            
            Console.WriteLine("Касовий збір за сеанс залежно від ціни квитка");
            Console.WriteLine($"Заповненість залу: {CalculateOccupancy(SoldSeats, TotalSeats):F1}%");
            Console.WriteLine();
            Console.WriteLine($"{"Ціна, грн",12}{"Збір (60 міс.)",18}{"Збір (120 міс.)",18}");
            Console.WriteLine(new string('-', 48));

            // цикл малює рівну таблицю розрахунків
            for (double price = priceFrom; price <= priceTo + 1e-9; price += priceStep)
            {
                double soldRevenue = CalculateRevenue(price, SoldSeats);
                double fullRevenue = CalculateRevenue(price, TotalSeats);
                Console.WriteLine($"{price,12:F2}{soldRevenue,18:F2}{fullRevenue,18:F2}");
            }
        }
        const int MaxSize = 10;
        const int SoldSeats = 60;
        const int TotalSeats = 120;

        // Читає розмір таблиці та повертає 0 якщо введено щось некоректне
        static int ReadSize(string prompt)
        {
            Console.Write(prompt);
            string text = Console.ReadLine();

            if (!int.TryParse(text, out int value))
            {
                Console.WriteLine("Помилка:потрібно ввести ціле число.");
                return 0;
            }
            if (value <= 0)
            {
                Console.WriteLine("Помилка:число має бути більшим за нуль.");
                return 0;
            }
            if (value > MaxSize)
            {
                Console.WriteLine($"Помилка:не більше {MaxSize}.");
                return 0;
            }
            return value;
        }

        // Таблиця виручки рядки,зали,стовпці,сеанси.
        // Справа й знизу підсумки.
        static void ShowRevenueTable()
        {
            // Межі таблиці не зашиті в код: питаємо в користувача
            int rows = ReadSize($"Скільки залів (1–{MaxSize}): ");
            if (rows == 0)
            {
                return;   // Невірний ввід тому таблицю не малюємо
            }
            int cols = ReadSize($"Скільки сеансів на день (1–{MaxSize}): ");
            if (cols == 0)
            {
                return;
            }

            int ticketPrice = 120;               // ціна квитка в грн
            int[] colSums = new int[cols];       // підсумки стовпців
            int grandTotal = 0;                  // загальний підсумок

            Console.WriteLine();
            Console.WriteLine("Можлива виручка за день, грн (усі місця продано, квиток 120 грн)");
            Console.WriteLine("Рядки — зали, стовпці — кількість сеансів на день");
            Console.WriteLine();

            // Шапка друкується до зовнішнього циклу
            Console.Write($"{"Зал",8}");
            for (int col = 1; col <= cols; col++)
            {
                Console.Write($"{col,9}");
            }
            Console.WriteLine($"{"Разом",11}");
            Console.WriteLine(new string('-', 8 + 9 * cols + 11));

            for (int row = 1; row <= rows; row++)
            {
                int seats = 60 + 20 * row;   // місткість залу залежить від його номера
                int rowSum = 0;              // підсумок рядка обнуляється для кожного залу

                Console.Write($"{row,8}");
                for (int col = 1; col <= cols; col++)
                {
                    int value = seats * ticketPrice * col;
                    rowSum += value;
                    colSums[col - 1] += value;
                    Console.Write($"{value,9}");
                }
                grandTotal += rowSum;
                //Новий рядок таблиці
                Console.WriteLine($"{rowSum,11}");
            }

            //Рядок підсумків під таблицею, вирівняний зі стовпцями
            Console.WriteLine(new string('-', 8 + 9 * cols + 11));
            Console.Write($"{"Разом",8}");
            for (int col = 0; col < cols; col++)
            {
                Console.Write($"{colSums[col],9}");
            }
            Console.WriteLine($"{grandTotal,11}");
        }
    }
}


