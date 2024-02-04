using MassTransit;
using Shared.Events;
using StorageService.Configuration;

namespace StorageService.Consumers;

public class TrackingEventConsumer : IConsumer<TrackingEvent>
{
    private readonly StorageServiceSettings storageServiceSettings;

    public TrackingEventConsumer(StorageServiceSettings storageServiceSettings)
    {
        this.storageServiceSettings = storageServiceSettings;
    }

    public async Task Consume(ConsumeContext<TrackingEvent> context)
    {
        var trackingEvent = context.Message;
        var line = $"{trackingEvent.VisitDatetime}|{trackingEvent.referrer}|{trackingEvent.UserAgent}|{trackingEvent.IpAddress}";
        await File.WriteAllTextAsync(storageServiceSettings.FilePath, line);

        Console.WriteLine(line);
        Console.WriteLine($"Message Processed: {context.MessageId} ✅");
    }
}
