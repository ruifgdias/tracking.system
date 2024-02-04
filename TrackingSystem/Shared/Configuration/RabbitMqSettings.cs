namespace Shared.Configuration;

public class RabbitMqSettings
{
    public static string SettingName => "RabbitMqSettings";

    public string QueueName { get; set; } = null!;

    public string Uri { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
