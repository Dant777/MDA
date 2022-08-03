namespace MDA.Entities
{
    internal sealed class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();

        public Restaurant()
        {
            for (ushort i = 1; i < 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите я подберу столик и подтвержу бронь, оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);
            Thread.Sleep(5000);
            table?.SetState(State.Booked);

            Console.WriteLine(table is null ? 
                "Столов нет" :
                $"Готово! Ваш стол номер {table.Id}");
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите я подберу столик и подтвержу бронь, оставайтесь на линии");
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                        && t.State == State.Free);
                table?.SetState(State.Booked);
                Console.WriteLine(table is null ? "Столов нет" : $"УВЕДОМЛЕНИЕ: Готово! Ваш стол номер {table.Id}");
            });
            
        }
    }
}
