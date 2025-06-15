using Minio.DataModel;
using Minio.DataModel.Args;
using MinIO_UltraClient.Config;
using MinIO_UltraClient.MinIO;
using System.Net.NetworkInformation;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Load saved endpoint access if available
            LoadSavedEndpointAccess();
        }

        private void LoadSavedEndpointAccess()
        {
            // Load saved access key, secret key, and URI from secure storage
            IUltraClientConfig? ultraClientConfig = UltraClientConfigurationManager.LoadUltraClientConfig();

            if (ultraClientConfig == null)
            {
                // If no saved configuration, return early
                return;
            }

            // Correctly deconstruct the configuration object
            string accessKey = ultraClientConfig.AccessKey;
            string secretKey = ultraClientConfig.SecretKey;
            string uri = ultraClientConfig.Uri;
            int ftpPort = ultraClientConfig.FtpPort;

            // If available, populate the UI fields with these values
            if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey) && !string.IsNullOrEmpty(uri))
            {
                AccessKeyInput.Password = accessKey;
                SecretKeyInput.Password = secretKey;
                EndpointUriInput.Text = uri;
                FtpPortInput.Text = ftpPort.ToString();
            }
        }

        private void ConnectUsingAccessKeyButton_Click(object sender, RoutedEventArgs e)
        {
            // Attempt a connection
            try
            {
                Ping ping = new Ping();
                ping.Send(EndpointUriInput.Text, 3);
            }
            catch (Exception ex)
            {
                // Show error message if connection fails
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            UltraClientConfigurationManager.SaveUltraClientConfig(
                new UltraClientConfig
                {
                    ProfileName = "default",
                    AccessKey = AccessKeyInput.Password,
                    SecretKey = SecretKeyInput.Password,
                    Uri = EndpointUriInput.Text,
                    FtpPort = int.Parse(FtpPortInput.Text),
                }
            );

            // @TODO Launch dashboard
        }
    }
}