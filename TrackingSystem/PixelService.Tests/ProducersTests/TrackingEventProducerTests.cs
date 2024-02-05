using MassTransit;
using Microsoft.Extensions.Options;
using Moq;
using PixelService.Producers.TrackingEventProducer;
using Shared.Configuration;
using Shared.Events;

namespace PixelService.Tests.ProducersTests
{
    [TestClass]
    public class TrackingEventProducerTests
    {
        private readonly TrackingEventProducer sut;
        private readonly Mock<IBus> busMocked;
        private readonly Mock<ISendEndpoint> sendEndpointMocked;
        private readonly Mock<IOptions<RabbitMqSettings>> settingsMocked;

        public TrackingEventProducerTests()
        {
            // Setup Mocked Dependencies
            this.busMocked = new Mock<IBus>();
            this.sendEndpointMocked = new Mock<ISendEndpoint>();
            this.busMocked.Setup(b => b.GetSendEndpoint(It.IsAny<Uri>())).ReturnsAsync(sendEndpointMocked.Object);
            this.sendEndpointMocked.Setup(b => b.Send(It.IsAny<TrackingEvent>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            this.settingsMocked = new Mock<IOptions<RabbitMqSettings>>();
            this.settingsMocked.Setup(x => x.Value).Returns( new RabbitMqSettings() { QueueName = "QueueName" });
            this.sut = new TrackingEventProducer(busMocked.Object, settingsMocked.Object);
        }

        [TestMethod]
        public async Task Send_ValidMessage_SendEventOnce()
        {
            // Arrange
            var trackingEvent = new TrackingEvent("IpAddress", "Referrer", "UserAgent", "SomeDate");

            // Act
            await this.sut.Send(trackingEvent, cancellationToken: default);

            // Assert
            this.sendEndpointMocked.Verify(endpoint => endpoint.Send(trackingEvent, default), Times.Once);
        }
    }
}
