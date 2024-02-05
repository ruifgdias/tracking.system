using MassTransit;
using Shared.Events;
using StorageService.Configuration;

namespace StorageService.Consumers;

public class TrackingEventConsumer : ITrackingEventConsumer
{
    private readonly StorageServiceSettings storageServiceSettings;

    public TrackingEventConsumer(StorageServiceSettings storageServiceSettings)
    {
        this.storageServiceSettings = storageServiceSettings;
    }

    public async Task Consume(ConsumeContext<TrackingEvent> context)
    {
        var trackingEvent = context.Message;
        var line = $"{trackingEvent.VisitDatetime}|{trackingEvent.Referrer}|{trackingEvent.UserAgent}|{trackingEvent.IpAddress}";
        await File.AppendAllLinesAsync(storageServiceSettings.FilePath, [line], context.CancellationToken);

        Console.WriteLine($"Message Processed: {context.MessageId} ✅");
    }
}
