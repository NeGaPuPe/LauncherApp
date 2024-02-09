using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
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
using System.IO.Compression;
using LauncherApp.Windows;

namespace LauncherApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

          InitializeComponent();
        }

        private void StartApp_Click(object sender, RoutedEventArgs e)
        {
            WebClient wc = new WebClient();
            if (StartApp.Content == "Обновить приложение")
            {
                MessageBoxResult result = MessageBox.Show("Версия вашего приложения устарела. Вы можете обновить ваше приложение или отложить обновление. Отложить обновление?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                    Cmd($@"cd {AppDomain.CurrentDomain.BaseDirectory}\App\PassengerTransportation-master\PassengerTransportationProject\bin\Debug && PassengerTransportationProject.exe");
                }
                else
                {
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation-master", true);
                    wc.DownloadFile("https://github.com/NeGaPuPe/PassengerTransportation/archive/master.zip", AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation.zip");
                    var apppath = System.IO.Path.GetFullPath(@"App\PassengerTransportation.zip");
                    var apppath1 = System.IO.Path.GetFullPath("App");
                    ZipFile.ExtractToDirectory(apppath, apppath1);
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation.zip");
                    MessageBox.Show("Приложение успешно обновлено", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWin.Cursor = Cursors.Wait;
                    StartApp.Content = "Запустить приложение";
                }
            }
            else
            {
                Application.Current.Shutdown();
                Cmd($@"cd {AppDomain.CurrentDomain.BaseDirectory}\App\PassengerTransportation-master\PassengerTransportationProject\bin\Debug && PassengerTransportationProject.exe");
            }
        }

        public Process Cmd(string line)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c {line}",
                WindowStyle = ProcessWindowStyle.Hidden,
            });
        }

        private void DownloadApp_Click(object sender, RoutedEventArgs e)
        {
            InstallVersion installVersion = new InstallVersion();
            installVersion.Show();
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App"))
            {
                Mouse.OverrideCursor = null;
                DownloadApp.Visibility = Visibility.Collapsed;
                StartApp.Visibility = Visibility.Visible;
                VersionAppTB.Visibility = Visibility.Visible;
                var a = new HttpClient();
                a.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                {
                    NoCache = true,
                };
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation-master"))
                {
                    string CurrentVersionApp = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation-master\PassengerTransportationProject\Resources\Version\Version.txt");
                    string VersionApp = (await (await a.GetAsync("https://raw.githubusercontent.com/NeGaPuPe/PassengerTransportation/master/PassengerTransportationProject/Resources/Version/Version.txt?time=" + DateTime.Now)).Content.ReadAsStringAsync()).Replace("\n", "");
                    VersionAppTB.Visibility = Visibility.Visible;
                    VersionAppTB.Text += CurrentVersionApp;
                    if (CurrentVersionApp != VersionApp)
                    {
                        Properties.Settings.Default.UpdateCheck = true;
                    }
                    else
                    {
                        Properties.Settings.Default.UpdateCheck = false;
                    }
                    if (Properties.Settings.Default.UpdateCheck == false)
                    {
                        StartApp.Content = "Запустить приложение";
                        VersionAppTB.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        StartApp.Content = "Обновить приложение";
                        VersionAppTB.Visibility = Visibility.Visible;
                    }
                }
            }

            else
            {
                DownloadApp.Visibility = Visibility.Visible;
                DeleteApp.Visibility = Visibility.Collapsed;
                StartApp.Visibility = Visibility.Collapsed;
                VersionAppTB.Visibility = Visibility.Hidden;
            }
        }

        private void DeleteApp_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить приложение?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App", true);
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Updates", true);
                StartApp.Visibility = Visibility.Collapsed;
                DownloadApp.Visibility = Visibility.Visible;
                DeleteApp.Visibility = Visibility.Collapsed;
                VersionAppTB.Visibility = Visibility.Hidden;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
        }
    }
}
