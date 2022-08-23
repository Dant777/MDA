using System.ComponentModel.DataAnnotations;

namespace MDA.Restaraunt.Messages.Repository;

public class BookingRequestModel
{
    [Key]
    public int BookingRequestId { get; set; }
    public Guid OrderId { get;  set; }
    public Guid ClientId { get;  set; }
    public Dish? PreOrder { get;  set; }
    public DateTime CreationDate { get;  set; }
}