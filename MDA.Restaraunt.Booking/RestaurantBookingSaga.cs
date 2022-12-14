using MassTransit;
using MDA.Restaraunt.Booking.Schedules;
using MDA.Restaraunt.Messages;
using MDA.Restaraunt.Messages.Repository;

namespace MDA.Restaraunt.Booking;

public sealed class RestaurantBookingSaga : MassTransitStateMachine<RestaurantBooking>
{
    private readonly IBookingRequestRepository _bookingRequestRepository;
    public RestaurantBookingSaga(IBookingRequestRepository bookingRequestRepository)
    {
        _bookingRequestRepository = bookingRequestRepository;

        InstanceState(x => x.CurrentState);

        Event(() => BookingRequested,
            x =>
                x.CorrelateById(context => context.Message.OrderId)
                    .SelectId(context => context.Message.OrderId));

        Event(() => TableBooked,
            x =>
                x.CorrelateById(context => context.Message.OrderId));

        Event(() => KitchenReady,
            x =>
                x.CorrelateById(context => context.Message.OrderId));

        CompositeEvent(() => BookingApproved,
            x => x.ReadyEventStatus, KitchenReady, TableBooked);

        Event(() => BookingRequestFault,
            x =>
                x.CorrelateById(m => m.Message.Message.OrderId));

        Schedule(() => BookingExpired,
            x => x.ExpirationId, x =>
            {
                x.Delay = TimeSpan.FromSeconds(10);
                x.Received = e => e.CorrelateById(context => context.Message.OrderId);
            });
        Schedule(() => DBDelete,
            x => x.BookingDbExpirationId, x =>
            {
                x.Delay = TimeSpan.FromSeconds(5);
                x.Received = e => e.CorrelateById(context => context.Message.OrderId);
            });

        Initially(
            When(BookingRequested)
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.OrderId;
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.ClientId = context.Data.ClientId;
                    Console.WriteLine("Saga: " + context.Data.CreationDate);
                })
                .Schedule(BookingExpired,
                    context => context.Init<IBookingExpire>(new BookingExpire() { OrderId = context.Saga.CorrelationId }))
                .Schedule(DBDelete,
                    context => context.Init<IDBDeleteSchedule>(new DbDeleteSchedule() { OrderId = context.Instance.OrderId }),
                    context => TimeSpan.FromSeconds(1))
                .TransitionTo(AwaitingBookingApproved)
        );

        During(AwaitingBookingApproved,
            When(BookingApproved)
                .Unschedule(BookingExpired)
                .Publish(context =>
                    (INotify)new Notify(context.Instance.OrderId,
                        context.Instance.ClientId,
                        $"Стол успешно забронирован"))
                .Finalize(),

            When(BookingRequestFault)
                .Then(context => Console.WriteLine($"Ошибочка вышла!"))
                .Publish(context => (INotify)new Notify(context.Instance.OrderId,
                    context.Instance.ClientId,
                    $"Приносим извинения, стол забронировать не получилось."))
                .Publish(context => (IBookingCancellation)
                    new BookingCancellation(context.Data.Message.OrderId))
                .Finalize(),

            When(BookingExpired.Received)
                .Then(context => Console.WriteLine($"Отмена заказа {context.Instance.OrderId}"))
                .Finalize(),
            When(DBDelete.AnyReceived)
                .Then(context =>
                {
                    var orderId = context.Instance.OrderId;
                    Console.WriteLine($"Удаление из хранилища сообщения - {orderId}");
                    _bookingRequestRepository.DeleteByOrderIdAsync(orderId);

                })

        );

        SetCompletedWhenFinalized();
    }
    public State AwaitingBookingApproved { get; private set; }
    public Event<IBookingRequest> BookingRequested { get; private set; }
    public Event<ITableBooked> TableBooked { get; private set; }
    public Event<IKitchenReady> KitchenReady { get; private set; }

    public Event<Fault<IBookingRequest>> BookingRequestFault { get; private set; }

    public Schedule<RestaurantBooking, IBookingExpire> BookingExpired { get; private set; }
    public Schedule<RestaurantBooking, IDBDeleteSchedule> DBDelete { get; private set; }
    public Event BookingApproved { get; private set; }
}