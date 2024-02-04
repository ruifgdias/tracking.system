using MassTransit;
using Microsoft.Extensions.Hosting;

namespace StorageService.Service;

public class BusService : IHostedService
{
    private readonly IBusControl _busControl;

    public BusService(IBusControl busControl)
    {
        _busControl = busControl;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Consumer running! 🚀");
        return _busControl.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _busControl.StopAsync(cancellationToken);
    }
}
