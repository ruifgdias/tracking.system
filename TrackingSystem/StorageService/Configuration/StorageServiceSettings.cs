namespace StorageService.Configuration
{
    public class StorageServiceSettings
    {
        public static string SettingName => "StorageServiceSettings";

        public string FilePath { get; set; } = null!;
    }
}
