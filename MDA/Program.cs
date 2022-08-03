using System.Diagnostics;
using MDA.Entities;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var rest = new Restaurant();
while (true)
{
    Console.WriteLine("Привет! Забронировать столик!" +
                      "\n1 - мы уведомим Вас по смс (асинхронно)" +
                      "\n2 - подождите на линии, мы Вас оповестим (синхронно)");

    if (!int.TryParse(Console.ReadLine(), out var choice) && choice is not (1 or 2))
    {
        Console.WriteLine("Введите, пожалуйста 1 или 2");
        continue;
    }

    var stopWatch = new Stopwatch();
    stopWatch.Start();
    if (choice == 1)
    {
        rest.BookFreeTableAsync(1);
    }
    else
    {
        rest.BookFreeTable(1);
    }

    Console.WriteLine("Спасибо за Ваше обращение!");
    stopWatch.Start();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}

