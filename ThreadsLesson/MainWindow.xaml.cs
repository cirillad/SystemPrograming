using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMultithreadingApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            outputListBox.Items.Clear();

            if (int.TryParse(startRangeTextBox.Text, out int startRange) &&
                int.TryParse(endRangeTextBox.Text, out int endRange) &&
                int.TryParse(threadsCountTextBox.Text, out int threadCount))
            {
                await StartThreadsAsync(startRange, endRange, threadCount);
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for start, end, and thread count.");
            }
        }

        private Task StartThreadsAsync(int startRange, int endRange, int threadCount)
        {
            return Task.Run(() =>
            {
                int totalNumbers = endRange - startRange + 1;
                int numbersPerThread = totalNumbers / threadCount;
                int remainingNumbers = totalNumbers % threadCount;

                for (int i = 0; i < threadCount; i++)
                {
                    int threadIndex = i;
                    int rangeStart = startRange + threadIndex * numbersPerThread;
                    int rangeEnd = (threadIndex == threadCount - 1)
                        ? endRange
                        : rangeStart + numbersPerThread - 1;

                    Task.Run(() =>
                    {
                        for (int j = rangeStart; j <= rangeEnd; j++)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                outputListBox.Items.Add($"Thread {threadIndex + 1}: {j}");
                            });

                            Thread.Sleep(50); 
                        }
                    });
                }
            });
        }
    }
}
