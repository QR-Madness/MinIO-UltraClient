using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MinIO_UltraClient.Config
{
    public static class SecureStorage
    {
        private static readonly string FilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MinIODashboard", "config.json");

        // Lock object for thread safety
        private static readonly object _lock = new();

        private static Dictionary<string, Dictionary<string, string>> LoadConfig()
        {
            if (!File.Exists(FilePath)) return new();

            try
            {
                var encrypted = File.ReadAllBytes(FilePath);
                var decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                var json = Encoding.UTF8.GetString(decrypted);
                return JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json) ?? new();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SecureStorage] LoadConfig error: {ex}");
                return new();
            }
        }

        private static void SaveConfig(Dictionary<string, Dictionary<string, string>> config)
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
                // Optionally, rethrow or handle as needed
            }
        }

        public static void SaveDefaultEndpointAccess(string accessKey, string secretKey, string uri)
        {
            SaveEndpointAccess("default", accessKey, secretKey, uri);
        }

        public static void SaveEndpointAccess(string profile, string accessKey, string secretKey, string uri)
        {
            lock (_lock)
            {
                var config = LoadConfig();

                if (!config.ContainsKey(profile))
                    config[profile] = new();

                config[profile]["accessKey"] = accessKey;
                config[profile]["secretKey"] = secretKey;
                config[profile]["uri"] = uri;

                SaveConfig(config);
            }
        }

        public static (string? AccessKey, string? SecretKey, string? Uri) GetEndpointAccess(string profile = "default")
        {
            lock (_lock)
            {
                var config = LoadConfig();

                if (config.TryGetValue(profile, out var profileData))
                {
                    profileData.TryGetValue("accessKey", out var accessKey);
                    profileData.TryGetValue("secretKey", out var secretKey);
                    profileData.TryGetValue("uri", out var uri);
                    return (accessKey, secretKey, uri);
                }

                return (null, null, null);
            }
        }

        public static (String? AccessKey, String? SecretKey, String? Uri) GetDefaultEndpointAccess()
        {
            return SecureStorage.GetEndpointAccess("default");
        }
    }
}