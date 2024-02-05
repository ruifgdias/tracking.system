using Shared.Events;

namespace PixelService.Producers.TrackingEventProducer;

public interface ITrackingEventProducer
{
    Task Send(TrackingEvent trackingEvent, CancellationToken cancellationToken);
}
