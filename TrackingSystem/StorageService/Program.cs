using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configuration;
using StorageService.Configuration;
using StorageService.Consumers;
using StorageService.Service;

namespace MassTransitExample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = new ConfigurationBuilder()
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                           .Build();

                    var storageServiceSettings = configuration.GetSection(StorageServiceSettings.SettingName).Get<StorageServiceSettings>();

                    services.AddSingleton(storageServiceSettings);

                    var rabbitMqSettings = configuration.GetSection(RabbitMqSettings.SettingName).Get<RabbitMqSettings>();

                    var filePath = configuration.GetValue<string>("FilePath");

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<TrackingEventConsumer>(s =>
                        new TrackingEventConsumer(storageServiceSettings));

                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(new Uri($"rabbitmq://{rabbitMqSettings.Uri}"), h =>
                            {
                                h.Username(rabbitMqSettings.Username);
                                h.Password(rabbitMqSettings.Password);
                            });

                            cfg.ReceiveEndpoint(rabbitMqSettings.QueueName, e =>
                            {
                                e.ConfigureConsumer<TrackingEventConsumer>(context);
                            });
                        });
                    });

                    services.AddHostedService<BusService>();
                })
                //.ConfigureLogging((hostContext, logging) =>
                //{
                //    logging.AddConsole();
                //})
                .RunConsoleAsync();
        }
    }
}