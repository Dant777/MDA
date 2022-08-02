namespace MDA.Entities
{
    internal enum State
    {
        Free=0,
        Booked=1
    }

    internal sealed class Table
    {
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
            return true;
        }
    }
}
