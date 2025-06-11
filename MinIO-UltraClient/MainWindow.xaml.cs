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
        }

        public void Show_IAM_Login()
        {
            // Create an instance of the IAM_Login UserControl
            IAM_Login iamLogin = new IAM_Login(this);
            // Set the Content of the MainWindow to the IAM_Login UserControl
            this.Content = iamLogin;
            // Optionally, you can set the title or other properties of the MainWindow
            this.Title = "IAM Login";
        }
    }
}