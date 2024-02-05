using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
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

namespace LauncherApp.Windows
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (UpdateCheck.IsChecked == false)
                {
                    Properties.Settings.Default.UpdateCheck = false;
                    Properties.Settings.Default.Save();
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App\LearningPractice-master"))
                    {
                        var a = new HttpClient();
                        a.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                        {
                            NoCache = true,
                        };
                        string CurrentVersionApp = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App\LearningPractice-master\PracticeShopProject\Resources\Version\VersionApp.txt");
                        string VersionApp = (await (await a.GetAsync("https://raw.githubusercontent.com/NeGaPuPe/LearningPractice/master/PracticeShopProject/Resources/Version/VersionApp.txt?time=" + DateTime.Now)).Content.ReadAsStringAsync()).Replace("\n", "");
                        if (CurrentVersionApp != VersionApp)
                        {
                            if (Properties.Settings.Default.UpdateCheck == false)
                            {
                                mainWindow.StartApp.Content = "Запустить приложение";
                            }
                            else
                            {
                                mainWindow.StartApp.Content = "Обновить приложение";
                            }
                        }
                    }
                }
                else
                {
                    Properties.Settings.Default.UpdateCheck = true;
                    Properties.Settings.Default.Save();
                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\App\LearningPractice-master"))
                    {
                        var a = new HttpClient();
                        a.DefaultRequestHeaders.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue
                        {
                            NoCache = true,
                        };
                        string CurrentVersionApp = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\App\LearningPractice-master\PracticeShopProject\Resources\Version\VersionApp.txt");
                        string VersionApp = (await (await a.GetAsync("https://raw.githubusercontent.com/NeGaPuPe/LearningPractice/master/PracticeShopProject/Resources/Version/VersionApp.txt?time=" + DateTime.Now)).Content.ReadAsStringAsync()).Replace("\n", "");
                        if (CurrentVersionApp != VersionApp)
                        {
                            if (Properties.Settings.Default.UpdateCheck == false)
                            {
                                mainWindow.StartApp.Content = "Запустить приложение";
                            }
                            else
                            {
                                mainWindow.StartApp.Content = "Обновить приложение";
                            }
                        }
                    }
                }
                Dispatcher.Invoke(() =>
                {
                    Close();
                });
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        static List<string> GetHardwareInfo(string WIN32_Class, string ClassItemField)
        {
            List<string> result = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + WIN32_Class);
            switch (ClassItemField)
            {
                case "Capacity":
                    int Capacity = 0;
                    foreach (ManagementObject m in searcher.Get()) Capacity += Convert.ToInt32(Math.Round(Convert.ToDouble(m[ClassItemField]) / 1024 / 1024));
                    result.Add(Capacity.ToString() + " Мб");
                    break;
                default:
                    foreach (ManagementObject obj in searcher.Get()) result.Add(obj[ClassItemField].ToString().Trim());
                    break;
            }
            return result;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.UpdateCheck == false)
                {
                    UpdateCheck.IsChecked = false;
                }
                else
                {
                    UpdateCheck.IsChecked = true;
                }
                MemoryVolume.Text += GetHardwareInfo("Win32_PhysicalMemory", "Capacity").First();
                CPUName.Text += GetHardwareInfo("Win32_Processor", "Name").First();
                ManufacturerCPU.Text += GetHardwareInfo("Win32_Processor", "Manufacturer").First();
                GPUName.Text += GetHardwareInfo("Win32_VideoController", "Name").First();
                VersioGPUDriver.Text += GetHardwareInfo("Win32_VideoController", "DriverVersion").First();
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
                Dispatcher.Invoke(() =>
                {
                    Close();
                });
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
