using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Http;
using System.ComponentModel;
using System.Threading;

namespace LauncherApp.Windows
{
    public partial class InstallVersion : Window
    {
        public InstallVersion()
        {
            InitializeComponent();
        }
        private async void DownloadVersion_Click(object sender, RoutedEventArgs e)
        {
            ProgressInstall progressInstall = new ProgressInstall();
            MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            WebClient wc = new WebClient();
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App\LearningPractice-master"))
            {
                MessageBox.Show("Приложение уже установлено.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                progressInstall.Show();
                await Task.Delay(1100);
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\App");
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Updates");
                wc.DownloadFile("https://github.com/NeGaPuPe/PassengerTransportation/archive/master.zip", AppDomain.CurrentDomain.BaseDirectory + @"\App\App.zip");
                var apppath = System.IO.Path.GetFullPath(@"App\App.zip");
                var apppath1 = System.IO.Path.GetFullPath("App");
                wc.DownloadFile("https://github.com/NeGaPuPe/PassengerTransportation/archive/master.zip", AppDomain.CurrentDomain.BaseDirectory + @"\Updates\ActualVersionApp.zip");
                var updatepath = System.IO.Path.GetFullPath(@"Updates\ActualVersionApp.zip");
                var updatepath1 = System.IO.Path.GetFullPath("Updates");
                ZipFile.ExtractToDirectory(apppath, apppath1);
                if (ArchiveCheckbox.IsChecked == false)
                {
                    ZipFile.ExtractToDirectory(updatepath, updatepath1);
                    string oldpath = AppDomain.CurrentDomain.BaseDirectory + @"\Updates\PassengerTransportation-master";
                    string newpath = AppDomain.CurrentDomain.BaseDirectory + @"\Updates\ActualVersionApp";
                    Directory.Move(oldpath, newpath);
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Updates\ActualVersionApp.zip");
                }
                else
                {
                    ZipFile.ExtractToDirectory(updatepath, updatepath1);
                    string oldpath = AppDomain.CurrentDomain.BaseDirectory + @"\Updates\PassengerTransportation-master";
                    string newpath = AppDomain.CurrentDomain.BaseDirectory + @"\Updates\ActualVersionApp";
                    Directory.Move(oldpath, newpath);
                }
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App\App.zip");
                var a = new HttpClient();
                a.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                {
                    NoCache = true,
                };
                mainWindow.StartApp.Visibility = Visibility.Visible;
                mainWindow.DeleteApp.Visibility = Visibility.Visible;
                mainWindow.DownloadApp.Visibility = Visibility.Collapsed;
                Dispatcher.Invoke(() =>
                {
                    Close();
                });
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Отменить установку приложения?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Dispatcher.Invoke(() =>
                {
                    Close();
                });
            }
        }
    }
}
