using MassTransit.Testing;
using MDA.Restaraunt.Booking.Consumers;
using MDA.Restaraunt.Booking.Entities;
using MDA.Restaraunt.Messages;
using MDA.Restaraunt.Messages.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MDA.Tests
{
    [TestFixture]
    public class RestaurantConsumersTest
    {
        private ServiceProvider _provider;
        private ITestHarness _harness;

        [OneTimeSetUp]
        public async Task Init()
        {
            _provider = new ServiceCollection()
                .AddMassTransitInMemoryTestHarness(cfg =>
                {
                    cfg.AddConsumer<BookingRequestConsumer>();
                })
                .AddLogging()
                .AddTransient<Restaurant>()
                .AddSingleton<IBookingRequestRepository, BookingRequestRepository>()
                .BuildServiceProvider(true);
            _harness = _provider.GetTestHarness();
            await _harness.Start();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
            await _provider.DisposeAsync();
        }

        [Test]
        public async Task Any_booking_request_consumed()
        {
            var orderIdd = Guid.NewGuid();

            await _harness.Bus.Publish(
                (IBookingRequest)new BookingRequest(
                    orderIdd,
                    Guid.NewGuid(),
                    null,
                    DateTime.Now));

            Assert.That(await _harness.Consumed.Any<IBookingRequest>());
        }
    }
}
