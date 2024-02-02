namespace PixelService.Configuration
{
    public class RabbitMqSettings
    {
        public static string SettingName => "RabbitMqSettings";

        public string Uri { get; set; }

        public string QueueName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
