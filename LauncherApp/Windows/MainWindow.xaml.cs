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
            try
            {
                InitializeComponent();
        }
            catch
            {
                MessageBox.Show("Произошла ошибка.","Сообщение",MessageBoxButton.OK,MessageBoxImage.Error);
            }
}

        private void StartApp_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App\LearningPractice-master", true);
                        wc.DownloadFile("https://github.com/NeGaPuPe/PassengerTransportation/archive/master.zip", AppDomain.CurrentDomain.BaseDirectory + @"\App\PracticeShop.zip");
                        var apppath = System.IO.Path.GetFullPath(@"App\PracticeShop.zip");
                        var apppath1 = System.IO.Path.GetFullPath("App");
                        ZipFile.ExtractToDirectory(apppath, apppath1);
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation.zip");
                        MessageBox.Show("Приложение успешно обновлено", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        StartApp.Content = "Запустить приложение";
                    }
                }
                else
                {
                    Application.Current.Shutdown();
                    Cmd($@"cd {AppDomain.CurrentDomain.BaseDirectory}\App\PassengerTransportation-master\PassengerTransportationProject\bin\Debug && PassengerTransportationProject.exe");
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
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
            try
            {
                InstallVersion installVersion = new InstallVersion();
                installVersion.Show();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App"))
                {
                    DownloadApp.Visibility = Visibility.Collapsed;
                    StartApp.Visibility = Visibility.Visible;

                    var a = new HttpClient();
                    a.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                    {
                        NoCache = true,
                    };
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation-master"))
                    {
                        string CurrentVersionApp = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App\PassengerTransportation-master\PassengerTransportationProject\Resources\Version\VersionApp.txt");
                        string VersionApp = (await (await a.GetAsync("https://raw.githubusercontent.com/NeGaPuPe/PassengerTransportation/master/PassengerTransportationProject/Resources/Version/VersionApp.txt?time=" + DateTime.Now)).Content.ReadAsStringAsync()).Replace("\n", "");
                        if (CurrentVersionApp != VersionApp)
                        {
                            if (Properties.Settings.Default.UpdateCheck == false)
                            {
                                StartApp.Content = "Запустить приложение";
                            }
                            else
                            {
                                StartApp.Content = "Обновить приложение";
                            }
                        }
                    }
                }
                
                else
                {
                    DownloadApp.Visibility = Visibility.Visible;
                    DeleteApp.Visibility = Visibility.Collapsed;
                    StartApp.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                MessageBox.Show("Отсутствует интернет.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void DeleteApp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить приложение?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\App", true);
                    Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Updates", true);
                    StartApp.Visibility = Visibility.Collapsed;
                    DownloadApp.Visibility = Visibility.Visible;
                    DeleteApp.Visibility = Visibility.Collapsed;
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SettingsWindow settingsWindow = new SettingsWindow();
                settingsWindow.Show();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
