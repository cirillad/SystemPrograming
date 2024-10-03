using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private int[] numbers;
        private int min;
        private int max;
        private double avg;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Random random = new Random();
                numbers = new int[10000];
                for (int i = 0; i < numbers.Length; i++)
                {
                    numbers[i] = random.Next(1, 10000);
                }

                Dispatcher.Invoke(() =>
                {
                    NumbersBox.Text = string.Join(", ", numbers);
                });

                Thread minThread = new Thread(FindMin);
                Thread maxThread = new Thread(FindMax);
                Thread avgThread = new Thread(FindAvg);

                minThread.Start();
                maxThread.Start();
                avgThread.Start();
            });
        }

        private void FindMin()
        {
            min = numbers.Min();
            Dispatcher.Invoke(() => MinText.Text = "Min: " + min);
        }

        private void FindMax()
        {
            max = numbers.Max();
            Dispatcher.Invoke(() => MaxText.Text = "Max: " + max);
        }

        private void FindAvg()
        {
            avg = numbers.Average();
            Dispatcher.Invoke(() => AvgText.Text = "Average: " + avg.ToString("F2"));
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                string filePath = "numbers_analysis.txt";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Generated Numbers:");
                    writer.WriteLine(string.Join(", ", numbers));
                    writer.WriteLine();
                    writer.WriteLine($"Min: {min}");
                    writer.WriteLine($"Max: {max}");
                    writer.WriteLine($"Average: {avg:F2}");
                }

                MessageBox.Show("Results saved to file: " + filePath);

            });
        }
    }
}
