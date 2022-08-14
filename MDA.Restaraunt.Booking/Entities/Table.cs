using MDA.Messenger.RabbitMQ;

namespace MDA.Restaraunt.Booking.Entities
{
    /// <summary>
    /// Класс столика
    /// </summary>
    public sealed class Table
    {
        private static Timer timer;
        private const long interval = 1000 * 10;
        private Producer _producer;
        public Table(int id)
        {
            Id=id;
            State = State.Free;
            SeatsCount = Random.Shared.Next(2, 5);
        }

        /// <summary>
        /// Текущие состояние столика
        /// </summary>
        public State State { get; private set; }

        /// <summary>
        /// Кол-во посадочных мест за столик
        /// </summary>
        public int SeatsCount { get; } 

        /// <summary>
        /// Id столика
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Оправщик сообщений
        /// </summary>
        public Producer Producer { get => _producer; set => _producer = value; }

        /// <summary>
        /// Задание состояния столика
        /// </summary>
        /// <param name="state">Состояние столика</param>
        /// <returns>true - состояние изменено</returns>
        public bool SetState(State state)
        {
            if (state == State)
            {
                return false;
            }
            State = state;
            StartTimerAsync();
            return true;
        }


        private void StartTimerAsync()
        {

            Task.Run(async () =>
            {
                timer = new Timer(new TimerCallback(RemoveBook), null, interval, 0);
            });
        }

        private void RemoveBook(object obj)
        {
            if (State == State.Booked)
            {
                State = State.Free;
                string msg = $"УВЕДОМЛЕНИЕ: Время бронирования прошло, бронь снята со столика {Id}";
                if (Producer == null)
                {
                    Messenger.PrintTxt(msg, MsgColor.Answer);
                }
                else
                {
                    _producer.SendToQueue(msg);
                }
                
            }

        }
    }
}
