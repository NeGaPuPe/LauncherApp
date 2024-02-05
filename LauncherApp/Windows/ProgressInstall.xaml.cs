using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class ProgressInstall : Window
    {
        public ProgressInstall()
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

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_Dowork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerAsync();
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void worker_Dowork(object sender, DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i <= 100; i++)
                {
                    (sender as BackgroundWorker).ReportProgress(i);
                    Thread.Sleep(17);
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressDownload.Value = e.ProgressPercentage;
            if (ProgressDownload.Value == 100)
            {
                MessageBox.Show("Приложение успешно установлено.", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}
