using System;
using System.Text;
namespace Cinema
{
    internal class  Program
    {
        static void Main(string[] args)
        {
            // що б укр літери відображались правильно 
            Console.OutputEncoding = Encoding.UTF8;
            //змінна яка керує виходом з програми 
            bool running = true;
            while (running) 
            { Console.WriteLine("Кінотеатр");
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
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                    case "2":
                        Console.WriteLine("поки не зроблено.");
                        Pause();
                        break;
                    case "3":
                        Console.WriteLine("Поки не зроблено");
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

        // захист від випаткових натискань що б не пропав результат 
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}

