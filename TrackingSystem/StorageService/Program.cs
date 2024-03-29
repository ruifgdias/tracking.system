﻿using MassTransit;
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

                    ArgumentNullException.ThrowIfNull(nameof(storageServiceSettings));

                    services.AddSingleton(storageServiceSettings);
                    services.AddSingleton<ITrackingEventConsumer,TrackingEventConsumer>();

                    var rabbitMqSettings = configuration.GetSection(RabbitMqSettings.SettingName).Get<RabbitMqSettings>();

                    var filePath = configuration.GetValue<string>("FilePath");

                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<ITrackingEventConsumer>(s =>
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
                                e.ConfigureConsumer<ITrackingEventConsumer>(context);
                            });
                        });
                    });

                    services.AddHostedService<BusService>();
                })
                .RunConsoleAsync();
        }
    }
}