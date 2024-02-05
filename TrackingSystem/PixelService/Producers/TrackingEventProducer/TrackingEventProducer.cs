using MassTransit;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using Shared.Events;

namespace PixelService.Producers.TrackingEventProducer
{
    public class TrackingEventProducer(IBus bus, IOptions<RabbitMqSettings> rabbitMqSettings) : ITrackingEventProducer
    {
        public async Task Send(TrackingEvent trackingEvent, CancellationToken cancellationToken)
        {
            var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{rabbitMqSettings.Value.QueueName}"));
            await endpoint.Send(trackingEvent, cancellationToken);
        }
    }
}
