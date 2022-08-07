using MDA.Messenger.RabbitMQ;

namespace MDA.Restaraunt.Booking.Entities
{
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

        public State State { get; private set; }
        public int SeatsCount { get; } 
        public int Id { get; }
        public Producer Producer { get => _producer; set => _producer = value; }

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


        public void StartTimerAsync()
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
                    Messenger.PrintAnswer(msg);
                }
                else
                {
                    _producer.SendToQueue(msg);
                }
                
            }

        }
    }
}
