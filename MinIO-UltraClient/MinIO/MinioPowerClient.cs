using Minio;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinIO_UltraClient.MinIO
{
    public class MinioPowerClient
    {
        public IMinioClient client { get; private set; } = null!;
        // Cached credentials for the MinIO server
        public string endpointUri { get; private set; }
        private string endpointAccessKey;
        private string endpointSecretKey;

        public MinioPowerClient(String endpointUri, String endpointAccessKey, String endpointSecretKey)
        {
            this.endpointUri = endpointUri;
            this.endpointAccessKey = endpointAccessKey;
            this.endpointSecretKey = endpointSecretKey;
            connect();
        }

        private string generateUserDefaultBucketName(string name)
        {
            const int randomSeed = 8332;
            // Generate a random number based on the name and a seed
            int randomNumber = (name.GetHashCode() ^ randomSeed) % 10000; // Ensure it's a 4-digit number
            randomNumber = Math.Abs(randomNumber); // Ensure it's non-negative
            // Format the bucket name with the prefix and random number
            return $"user-{name}-default-{randomNumber:D4}"; // Ensure 4 digits with leading zeros
        }

        /// <summary>
        /// Initalizes the MinIO client connection using the provided credentials.
        /// </summary>
        private void connect()
        {
            client = new Minio.MinioClient()
                .WithEndpoint(endpointUri)
                .WithCredentials(endpointAccessKey, endpointSecretKey)
                .Build();
        }
    }
}
