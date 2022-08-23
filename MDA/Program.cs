using System.Diagnostics;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;


Console.OutputEncoding = System.Text.Encoding.UTF8;

var rest = new MDA.Restaraunt.Booking.Entities.Restaurant();
while (true)
{
    Messenger.PrintTxt("Привет! Забронировать столик/отменить бронь!" +
                      "\n\t1 - мы уведомим Вас по смс (асинхронно)" +
                      "\n\t2 - подождите на линии, мы Вас оповестим (синхронно)" +
                      "\n\t3 - мы уведомим Вас по смс об отмене брони (асинхронно)" +
                      "\n\t4 - подождите на линии, мы Вас оповестим об отмене брони (синхронно)"
                      , MsgColor.Normal);


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
            Messenger.PrintTxt("Какой столик отменить?(Выберите номер)", MsgColor.Normal);
            removeInput = Console.ReadLine();
            tableNum = IsCorrectUserInput(removeInput);
            if (tableNum == -1) continue;
            rest.RemoveBookTableAsync(tableNum);
            break;
        case 4:
            Messenger.PrintTxt("Какой столик отменить?(Выберите номер)", MsgColor.Normal);
            removeInput = Console.ReadLine();
            tableNum = IsCorrectUserInput(removeInput);
            if(tableNum == -1) continue;
            rest.RemoveBookTable(tableNum);
            break;
        default:
            Messenger.PrintTxt("ОШИБКА: Введите, пожалуйста 1, 2, 3, 4", MsgColor.Error);
            continue;
    }


    Messenger.PrintTxt("Спасибо за Ваше обращение!", MsgColor.Answer);
    stopWatch.Start();
    var ts = stopWatch.Elapsed;
    Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
}

static int IsCorrectUserInput(string usetInput)
{
    bool isNum = int.TryParse(usetInput, out var choice);
    if (!isNum)
    {
        Messenger.PrintTxt("ОШИБКА: Введите, пожалуйста цифры", MsgColor.Error);
        return -1;
    }

    return choice;
}