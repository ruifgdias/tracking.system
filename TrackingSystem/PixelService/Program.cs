using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", c =>
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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/tracking", (HttpContext context) =>
{
    // --- Collect Data
    //1.Referrer header
    var referrer = context.Request.Headers.Referer;

    //2.User - Agent header
    var userAgent = context.Request.Headers.Referer;

    //3.Visitor IP address
    var ipAddress = context.Connection.RemoteIpAddress;

    // --- Git Response
    // String base64 gif image
    var base64String = "R0lGODlhAQABAIAAAP///wAAACwAAAAAAQABAAACAkQBADs=";
    var gifBytes = Convert.FromBase64String(base64String);
    var memoryStream = new MemoryStream(gifBytes);

    return Results.Stream(memoryStream, "image/gif", "px.gif");
})
    .WithName("tracking")
    .WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
