using MassTransit;
using Shared.Events;

namespace StorageService.Consumers;

public interface ITrackingEventConsumer : IConsumer<TrackingEvent>
{
}
