using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MinIO_UltraClient.Config
{
    public static class UltraClientConfigurationManager
    {
        private static readonly string FilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MinIODashboard", "config.json");

        // Lock object for thread safety
        private static readonly object _lock = new();

        public static Dictionary<string, object>? LoadConfig()
        {
            if (!File.Exists(FilePath)) return null;

            try
            {
                var encrypted = File.ReadAllBytes(FilePath);
                var decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                var json = Encoding.UTF8.GetString(decrypted);
                return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SecureStorage] LoadConfig error: {ex}");
                return null;
            }
        }

        public static void SaveConfig(Dictionary<string, object> config)
        {
            try
            {
                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(json), null, DataProtectionScope.CurrentUser);
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
                File.WriteAllBytes(FilePath, encrypted);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SecureStorage] SaveConfig error: {ex}");
            }
        }

        public static IUltraClientConfig? LoadUltraClientConfig(string profile = "default")
        {
            lock (_lock)
            {
                var config = LoadConfig();
                if (config == null || !config.TryGetValue(profile, out var profileObj) || profileObj is null)
                    return null;

                // Deserialize the profile object to UltraClientConfig
                var json = JsonSerializer.Serialize(profileObj);
                return JsonSerializer.Deserialize<UltraClientConfig>(json);
            }
        }

        public static void SaveUltraClientConfig(IUltraClientConfig ultraClientConfig)
        {
            lock (_lock)
            {
                var config = LoadConfig() ?? new Dictionary<string, object>();
                config[ultraClientConfig.ProfileName] = ultraClientConfig;
                SaveConfig(config);
            }
        }
    }
}