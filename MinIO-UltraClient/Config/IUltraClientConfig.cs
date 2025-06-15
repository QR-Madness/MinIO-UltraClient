namespace MinIO_UltraClient.Config
{
    public interface IUltraClientConfig
    {
        string ProfileName { get; set; }
        string AccessKey { get; set; }
        string SecretKey { get; set; }
        string Uri { get; set; }
        int FtpPort { get; set; }
    }

    // Optionally, a concrete implementation for convenience
    public class UltraClientConfig : IUltraClientConfig
    {
        public string ProfileName { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;
        public int FtpPort { get; set; }
    }
}