namespace MDA.Restaraunt.Messages
{
    public interface ITableBooked
    {
        public Guid OrderId { get; }

        public Guid ClientId { get; }

        public Dish? PreOrder { get; }

        public bool Success { get; }
        public UserChoose UserChoose { get; }

    }
    public class TableBooked : ITableBooked
    {
        public TableBooked(Guid orderId, Guid clientId, bool success, UserChoose userChoose, Dish? preOrder = null)
        {
            OrderId = orderId;
            ClientId = clientId;
            Success = success;
            PreOrder = preOrder;
            UserChoose = userChoose;
        }

        public Guid OrderId { get; }
        public Guid ClientId { get; }
        public Dish? PreOrder { get; }
        public bool Success { get; }
        public UserChoose UserChoose { get; }
    }
}
