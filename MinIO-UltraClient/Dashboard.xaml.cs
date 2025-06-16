using Minio;
using Minio.DataModel;
using MinIO_UltraClient.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinIO_UltraClient
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        // IUltraClientConfig (Injected by main window)
        private readonly IUltraClientConfig _ultraClientConfig;
        /// <summary>
        /// Minio client instance for interacting with the MinIO server.
        /// </summary>
        private IMinioClient? _minioClient = null;

        private Bucket[] buckets = Array.Empty<Bucket>();

        /// <summary>
        ///  Constructor that accepts IUltraClientConfig as a parameter
        /// </summary>
        /// <param name="ultraClientConfig">Saved configuration profile.</param>
        public Dashboard(IUltraClientConfig ultraClientConfig)
        {
            InitializeComponent();
            this._ultraClientConfig = ultraClientConfig;
            this._minioClient = new MinioClient()
                .WithEndpoint(ultraClientConfig.Uri, ultraClientConfig.ServerPort)
                .WithCredentials(ultraClientConfig.AccessKey, ultraClientConfig.SecretKey)
                .Build();
        }

        private bool FetchBuckets()
        {
            try
            {
                // Fetch the list of buckets from the MinIO server
                var bucketList = _minioClient.ListBucketsAsync().GetAwaiter().GetResult();
                buckets = bucketList.Buckets.ToArray();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching buckets: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void RefreshBucketListButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
