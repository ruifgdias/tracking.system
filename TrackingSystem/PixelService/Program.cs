using MassTransit;
using PixelService.Extensions;
using PixelService.Producers.TrackingEventProducer;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(RabbitMqSettings.SettingName));
builder.Services.AddSingleton<ITrackingEventProducer, TrackingEventProducer>();

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

app.MapGet("/track", async (HttpContext context, ITrackingEventProducer trackingEventProducer) =>
{
    await trackingEventProducer.Send(context.GetTrackingEvent());

    return ApiResults.EmptyGif();
})
    .WithName("tracking")
    .WithOpenApi();

app.Run();