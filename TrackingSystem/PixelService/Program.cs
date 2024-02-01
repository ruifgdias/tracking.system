using MassTransit;
using PixelService.Extensions;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost/", c =>
        {
            c.Username("dev");
            c.Password("dev");
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

app.MapGet("/tracking", async (HttpContext context, IBus bus) =>
{
    var message = GetTrackingInformation(context);

    await SendTrackingEvent(bus, message);

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

    return new TrackingEvent(ipAddress?.ToString(), referrer, userAgent);
}

static async Task SendTrackingEvent(IBus bus, TrackingEvent message)
{
    var endpoint = await bus.GetSendEndpoint(new Uri("queue:tracking-events-v1"));
    await endpoint.Send(message);
}