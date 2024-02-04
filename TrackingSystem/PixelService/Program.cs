using System.Globalization;
using MassTransit;
using Microsoft.Extensions.Options;
using PixelService.Extensions;
using Shared.Configuration;
using Shared.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection(RabbitMqSettings.SettingName));

builder.Services.AddMassTransit(x =>
{
    var rabbitMqSettings = builder.Configuration.GetSection(RabbitMqSettings.SettingName).Get<RabbitMqSettings>();    

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host($"rabbitmq://{rabbitMqSettings!.Uri}/", c =>
        {
            c.Username(rabbitMqSettings.Username);
            c.Password(rabbitMqSettings.Password);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/tracking", async (HttpContext context, IBus bus, IOptions<RabbitMqSettings> rabbitMqSettings) =>
{
    var message = GetTrackingInformation(context);

    await SendTrackingEvent(bus, message, rabbitMqSettings.Value.QueueName);

    return ApiResults.EmptyGif();
})
    .WithName("tracking")
    .WithOpenApi();

app.Run();

static TrackingEvent GetTrackingInformation(HttpContext context)
{
    var referrer = context.Request.Headers.Referer;
    var userAgent = context.Request.Headers.UserAgent;
    var ipAddress = context.Connection.RemoteIpAddress;

    return new TrackingEvent(ipAddress?.ToString(), referrer, userAgent, DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
}

static async Task SendTrackingEvent(IBus bus, TrackingEvent message, string queueName)
{
    var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{queueName}"));
    await endpoint.Send(message);
}