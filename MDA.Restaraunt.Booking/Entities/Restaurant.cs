using MDA.Restaraunt.Messages;

namespace MDA.Restaraunt.Booking.Entities
{
    /// <summary>
    /// Класс ресторана
    /// </summary>
    public sealed class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();

        public Restaurant()
        {

            for (ushort i = 1; i < 10; i++)
            {
                _tables.Add(new Table(i));
            }

        }

        /// <summary>
        /// Забронировать свободный столик
        /// </summary>
        /// <param name="countOfPersons">Кол-во персон</param>
        public bool? BookFreeTable(int countOfPersons)
        {
            Messenger.PrintMsgSleep("Добрый день! Подождите я подберу столик и подтвержу бронь, оставайтесь на линии", MsgColor.Normal);

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);

           return table?.SetState(State.Booked);

            //Messenger.PrintTxt(table is null ?
            //    "Столов нет" :
            //    $"УВЕДОМЛЕНИЕ: Готово! Ваш стол номер - {table.Id}", MsgColor.Answer);
        }

        /// <summary>
        /// Асинхронное бронирование свободного столика
        /// </summary>
        /// <param name="countOfPersons">Кол-во персон</param>
        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {

            await Messenger.PrintMsgSleepAsync("Добрый день! Подождите я подберу столик и подтвержу бронь, Вам придет уведомление", MsgColor.Normal);
            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);
            return table?.SetState(State.Booked);

        }

        /// <summary>
        /// Удаление брони столика
        /// </summary>
        /// <param name="tableNumber">Номер столика</param>
        public bool? RemoveBookTable(int tableNumber)
        {
            Messenger.PrintMsgSleep("Добрый день! Подождите я сниму бронь со стола, оставайтесь на линии", MsgColor.Normal);

            var table = _tables.FirstOrDefault(t => t.Id == tableNumber
                                                    && t.State == State.Booked);

            return table?.SetState(State.Free);

            //Messenger.PrintTxt(table is null ?
            //    "Стол бы не занят" :
            //    $"УВЕДОМЛЕНИЕ: Готово! Ваша бронь снята со стола - {table.Id}", MsgColor.Answer);
        }

        /// <summary>
        /// Асинхронное удаление брони столика
        /// </summary>
        /// <param name="tableNumber">Номер столика</param>
        public async Task<bool?> RemoveBookTableAsync(int tableNumber)
        {

            await Messenger.PrintMsgSleepAsync("Добрый день! Подождите я сниму бронь со стола, Вам придет уведомление", MsgColor.Normal);
            var table = _tables.FirstOrDefault(t => t.Id == tableNumber
                                                    && t.State == State.Booked);
            return table?.SetState(State.Free);
        }

    }
}
