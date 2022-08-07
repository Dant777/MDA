using System.Diagnostics;
using MDA.Restaraunt.Booking.Entities;


Console.OutputEncoding = System.Text.Encoding.UTF8;

var rest = new Restaurant();
while (true)
{
    Messenger.PrintTxt("Привет! Забронировать столик/отменить бронь!" +
                      "\n\t1 - мы уведомим Вас по смс (асинхронно)" +
                      "\n\t2 - подождите на линии, мы Вас оповестим (синхронно)" +
                      "\n\t3 - мы уведомим Вас по смс об отмене брони (асинхронно)" +
                      "\n\t4 - подождите на линии, мы Вас оповестим об отмене брони (синхронно)" +
                      "\n\t5 - Иноформация о столиках");


    string removeInput = String.Empty;
    int tableNum;
    string userInput = Console.ReadLine();
    var stopWatch = new Stopwatch();
    stopWatch.Start();
    switch (IsCorrectUserInput(userInput))
    {
        case -1:
            continue;
        case 1:
            rest.BookFreeTableAsync(1);
            break;
        case 2:
            rest.BookFreeTable(1);
            break;
        case 3:
            Messenger.PrintTxt("Какой столик отменить?(Выберите номер)");
            removeInput = Console.ReadLine();
            tableNum = IsCorrectUserInput(removeInput);
            if (tableNum == -1) continue;
            rest.RemoveBookTableAsync(tableNum);
            break;
        case 4:
            Messenger.PrintTxt("Какой столик отменить?(Выберите номер)");
            removeInput = Console.ReadLine();
            tableNum = IsCorrectUserInput(removeInput);
            if(tableNum == -1) continue;
            rest.RemoveBookTable(tableNum);
            break;
        case 5:
            rest.PrintTablesInfo();
            break;
        default:
            Messenger.PrintError("ОШИБКА: Введите, пожалуйста 1, 2, 3, 4");
            continue;
    }


    Messenger.PrintAnswer("Спасибо за Ваше обращение!");
    stopWatch.Start();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}

static int IsCorrectUserInput(string usetInput)
{
    bool isNum = int.TryParse(usetInput, out var choice);
    if (!isNum)
    {
        Messenger.PrintError("ОШИБКА: Введите, пожалуйста цифры");
        return -1;
    }

    return choice;
}