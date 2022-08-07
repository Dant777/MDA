namespace MDA.Entities
{
    internal sealed class Table
    {
        static Timer timer;
        long interval = 1000 * 20;

        public Table(int id)
        {
            Id=id;
            State = State.Free;
            SeatsCount = Random.Shared.Next(2, 5);
        }

        public State State { get; private set; }
        public int SeatsCount { get; } 
        public int Id { get; }

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
                Messenger.PrintAnswer($"Время бронирования прошло, бронь снята со столика {Id}");
            }

        }
    }
}
