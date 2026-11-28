using System;
using System.Globalization;
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
            const int MaxSize = 10;
            const int SoldSeats = 60;
            const int TotalSeats = 120;
            const int Capacity = 100;
            //паралельні масиви один запис=один і той самий індекс у всіх масивах
            int[] ids = new int[Capacity];              // код сеансу
            string[] titles = new string[Capacity];     // назва фільму
            int[] hallIds = new int[Capacity];          // код залу
            DateTime[] dates = new DateTime[Capacity];  // дата й час
            double[] prices = new double[Capacity];     // ціна квитка
            int[] sold = new int[Capacity];             // продано місць
            int[] totals = new int[Capacity];           // усього місць
            int count = 0;                              // скільки записів зайнято
            int nextId = 1;                             // наступний номер (призначає програма)
            while (running)
            {
                ShowMenu();                         // було 6 рядків WriteLine 
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)                     //кожен case в один рядок 
                {
                    case "2":
                        AddSession(ids, titles, hallIds, dates, prices, sold, totals, Capacity, ref count, ref nextId);
                        Pause();
                        break;
                    case "4": ShowAbout(); Pause(); break;
                    case "5":
                        {
                            double priceFrom = ReadDouble("Початкова ціна грн (1-1000): ", 1, 1000);
                            double priceTo = ReadDouble("Кінцева ціна грн: ", priceFrom, 1000);
                            double priceStep = ReadDouble("Крок ціни грн (0,5-1000): ", 0.5, 1000);
                            Console.WriteLine();
                            ShowCostTable(priceFrom, priceTo, priceStep, SoldSeats, TotalSeats);
                            Pause();
                            break;
                        }
                    case "6":
                        {
                            int rows = ReadInt($"Скільки залів (1–{MaxSize}): ", 1, MaxSize);
                            int cols = ReadInt($"Скільки сеансів на день (1–{MaxSize}): ", 1, MaxSize);
                            ShowRevenueTable(rows, cols);
                            Pause();
                            break;
                        }
                    
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
            Console.WriteLine("2-додати сеанс");
            Console.WriteLine("4-довідка про програму");
            Console.WriteLine("5-вартість квитків");
            Console.WriteLine("6-можлива виручка");
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

        //таблиця сеанса 
        static void ShowCostTable(double priceFrom, double priceTo, double priceStep,
                           int soldSeats, int totalSeats)
        {
            Console.WriteLine("Касовий збір за сеанс залежно від ціни квитка");
            Console.WriteLine($"Заповненість залу: {CalculateOccupancy(soldSeats, totalSeats):F1}%");
            Console.WriteLine();
            PrintCostTable(priceFrom, priceTo, priceStep, soldSeats, totalSeats);
        }
        // табулювання: початок, кінець, крок
        static void PrintCostTable(double priceFrom, double priceTo, double priceStep, int soldSeats, int totalSeats)
        {
            Console.WriteLine($"{"Ціна, грн",12}{$"Збір ({soldSeats} міс.)",18}{$"Збір ({totalSeats} міс.)",18}");
            Console.WriteLine(new string('-', 48));

            // цикл малює рівну таблицю розрахунків
            for (double price = priceFrom; price <= priceTo + 1e-9; price += priceStep)
            {
                double soldRevenue = CalculateRevenue(price, soldSeats);
                double fullRevenue = CalculateRevenue(price, totalSeats);
                Console.WriteLine($"{price,12:F2}{soldRevenue,18:F2}{fullRevenue,18:F2}");
            }
        }
        // місце де записуються всі масиви.Повертає false якщо сховище заповнене.
        static bool AddRecord(int[] ids, string[] titles, int[] hallIds, DateTime[] dates,
                              double[] prices, int[] sold, int[] totals,
                              int capacity, ref int count, ref int nextId,
                              string title, int hallId, DateTime date, double price,
                              int soldSeats, int totalSeats)
        {
            if (count == capacity)
            {
                return false;       //перевірка ДО запису
            }
            ids[count] = nextId;    //номер призначає програма
            titles[count] = title;
            hallIds[count] = hallId;
            dates[count] = date;
            prices[count] = price;
            sold[count] = soldSeats;
            totals[count] = totalSeats;
            count++;
            nextId++;
            return true;
        }

        //збір все в локальні змінні і потім пишемо в масиви
        static void AddSession(int[] ids, string[] titles, int[] hallIds, DateTime[] dates,
                               double[] prices, int[] sold, int[] totals,
                               int capacity, ref int count, ref int nextId)
        {
            if (count == capacity)
            {
                Console.WriteLine("Сховище заповнене.");
                return;
            }
            string title = ReadText("Назва фільму: ");
            int hallId = ReadInt("Код залу (1–10): ", 1, 10);
            DateTime date = ReadDate("Дата й час (дд.мм.рррр гг:хх): ");
            double price = ReadDouble("Ціна квитка грн (1-1000): ", 1, 1000);
            int totalSeats = ReadInt("Усього місць (1–500): ", 1, 500);
            int soldSeats = ReadInt($"Продано місць (0–{totalSeats}): ", 0, totalSeats);

            AddRecord(ids, titles, hallIds, dates, prices, sold, totals,
                      capacity, ref count, ref nextId,
                      title, hallId, date, price, soldSeats, totalSeats);
            Console.WriteLine($"Додано сеанс №{ids[count - 1]}.");   // count уже збільшено
        }

        //рядок не може бути порожнім
        static string ReadText(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (text == null) text = "";
                text = text.Trim();
                if (text != "")
                {
                    return text;
                }
                Console.WriteLine("Помилка: рядок не може бути порожнім.");
            }
        }

        //дата і час у форматі дд.мм.рррр гг:хх
        static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (DateTime.TryParseExact(text, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out DateTime value))
                {
                    return value;
                }
                Console.WriteLine("Помилка: формат дд.мм.рррр гг:хх, наприклад 28.11.2026 18:30.");
            }
        }

        //питає доки не отримає число в межах min max
        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();
                if (text == null) text = "";
                text = text.Trim().Replace(',', '.');   //щоб приймало і кому і крапку

                if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Помилка: потрібно ввести число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }

        // те саме для цілих чисел
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = Console.ReadLine();

                if (!int.TryParse(text, out int value))
                {
                    Console.WriteLine("Помилка: потрібно ввести ціле число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }
       

        // Таблиця виручки рядки,зали,стовпці,сеанси.
        // Справа й знизу підсумки.
        static void ShowRevenueTable(int rows, int cols)
        {
            
            

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


