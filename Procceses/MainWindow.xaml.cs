using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowAll(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = Process.GetProcesses();
        }

        private void KillProcess(object sender, RoutedEventArgs e)
        {
            Process prToKill = (grid.SelectedItem as Process);
            if (prToKill != null)
            {
                prToKill.Kill();
                MessageBox.Show("Program killed");
            }
            else
            {
                MessageBox.Show("Select a process to kill.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenExeByLink(object sender, RoutedEventArgs e)
        {
            string path = nameProcess.Text;

            try
            {
                Process.Start(path);
                MessageBox.Show("Program is opening");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося відкрити файл: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Метод для показу детальної інформації про процес
        private void ShowProcessDetails(object sender, RoutedEventArgs e)
        {
            Process selectedProcess = (grid.SelectedItem as Process);

            if (selectedProcess != null)
            {
                try
                {
                    // Формування детальної інформації про процес
                    string details = $"Process Name: {selectedProcess.ProcessName}\n" +
                                     $"ID: {selectedProcess.Id}\n" +
                                     $"Start Time: {selectedProcess.StartTime}\n" +
                                     $"Total Processor Time: {selectedProcess.TotalProcessorTime}\n" +
                                     $"Priority: {selectedProcess.PriorityClass}\n" +
                                     $"Working Set: {selectedProcess.WorkingSet64 / 1024} KB";

                    MessageBox.Show(details, "Process Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не вдалося отримати інформацію про процес: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Select a process to view details.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
