using MDA.Messenger.RabbitMQ;

namespace MDA.Restaraunt.Booking.Entities
{
    public sealed class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();
        private readonly Producer _producer;
        public Restaurant()
        {
            _producer = new Producer("RabbitTestQueue");
            for (ushort i = 1; i < 10; i++)
            {
                _tables.Add(new Table(i) { Producer = _producer });
            }

        }

        public void BookFreeTable(int countOfPersons)
        {
            Messenger.PrintMsgSleep("Добрый день! Подождите я подберу столик и подтвержу бронь, оставайтесь на линии");
            
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);
            
            table?.SetState(State.Booked);

            Messenger.PrintAnswer(table is null ? 
                "Столов нет" :
                $"УВЕДОМЛЕНИЕ: Готово! Ваш стол номер - {table.Id}");
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            
            Task.Run(async () =>
            {
                await Messenger.PrintMsgSleepAsync("Добрый день! Подождите я подберу столик и подтвержу бронь, Вам придет уведомление");
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                        && t.State == State.Free);
                table?.SetState(State.Booked);
                _producer.SendToQueue(table is null ? "Столов нет" : $"УВЕДОМЛЕНИЕ: Готово! Ваш стол номер {table.Id}");
            });
            
        }

        public void RemoveBookTable(int tableNumber)
        {
            Messenger.PrintMsgSleep("Добрый день! Подождите я сниму бронь со стола, оставайтесь на линии");
            
            var table = _tables.FirstOrDefault(t => t.Id == tableNumber 
                                                    && t.State == State.Booked);
            
            table?.SetState(State.Free);

            Messenger.PrintAnswer(table is null ?
                "Стол бы не занят" :
                $"УВЕДОМЛЕНИЕ: Готово! Ваша бронь снята со стола - {table.Id}");
        }

        public void RemoveBookTableAsync(int tableNumber)
        {
            
            Task.Run(async () =>
            {
                await Messenger.PrintMsgSleepAsync("Добрый день! Подождите я сниму бронь со стола, Вам придет уведомление");
                var table = _tables.FirstOrDefault(t => t.Id == tableNumber
                                                        && t.State == State.Booked);
                table?.SetState(State.Free);
                _producer.SendToQueue(table is null ?
                    "Стол был не занят" :
                    $"УВЕДОМЛЕНИЕ: Готово! Ваша бронь снята со стола - {table.Id}");
            });
        }

        public void PrintTablesInfo()
        {
            string msg = "Информация о столиках:";

            foreach (var table in _tables)
            {
                msg += $"\t-Table #{table.Id} - {table.State}\n";
            }
            _producer.SendToQueue(msg);
        }

    }
}
