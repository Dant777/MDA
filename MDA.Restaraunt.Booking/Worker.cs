using MassTransit;
using MDA.Restaraunt.Messages;
using Microsoft.Extensions.Hosting;
using MDA.Restaraunt.Booking.Entities;

namespace MDA.Restaraunt.Booking
{
    public class Worker : BackgroundService
    {
        private readonly IBus _bus;
        private readonly Restaurant _restaurant;

        public Worker(IBus bus, Restaurant restaurant)
        {
            _bus = bus;
            _restaurant = restaurant;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                bool? result = true;
                Messenger.PrintTxt("Привет! Забронировать столик/отменить бронь!" +
                                   "\n\t1 - мы уведомим Вас по смс (асинхронно)" +
                                   "\n\t2 - подождите на линии, мы Вас оповестим (синхронно)" +
                                   "\n\t3 - мы уведомим Вас по смс об отмене брони (асинхронно)" +
                                   "\n\t4 - подождите на линии, мы Вас оповестим об отмене брони (синхронно)"
                    , MsgColor.Normal);
                string removeInput = String.Empty;
                int tableNum;
                UserChoose userChoose;
                string userInput = Console.ReadLine();

                switch (IsCorrectUserInput(userInput))
                {
                    case -1:
                        continue;
                    case 1:
                        result = await _restaurant.BookFreeTableAsync(1);
                        userChoose = UserChoose.Booked;
                        break;
                    case 2:
                        result = _restaurant.BookFreeTable(1);
                        userChoose = UserChoose.Booked;
                        break;
                    case 3:
                        Messenger.PrintTxt("Какой столик отменить?(Выберите номер)", MsgColor.Normal);
                        removeInput = Console.ReadLine();
                        tableNum = IsCorrectUserInput(removeInput);
                        if (tableNum == -1) continue;
                        result = await _restaurant.RemoveBookTableAsync(tableNum);
                        userChoose = UserChoose.RemoveBooked;
                        break;
                    case 4:
                        Messenger.PrintTxt("Какой столик отменить?(Выберите номер)", MsgColor.Normal);
                        removeInput = Console.ReadLine();
                        tableNum = IsCorrectUserInput(removeInput);
                        if (tableNum == -1) continue;
                        result = _restaurant.RemoveBookTable(tableNum);
                        userChoose = UserChoose.RemoveBooked;
                        break;
                    default:
                        Messenger.PrintTxt("ОШИБКА: Введите, пожалуйста 1, 2, 3, 4", MsgColor.Error);
                        continue;
                }


                Messenger.PrintTxt("Спасибо за Ваше обращение!", MsgColor.Answer);

                await Task.Delay(3000, stoppingToken);

                var dateTime = DateTime.Now;
                await _bus.Publish(
                    (IBookingRequest)new BookingRequest(NewId.NextGuid(), NewId.NextGuid(), null, dateTime),
                    stoppingToken); ;
            }
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
    }
}