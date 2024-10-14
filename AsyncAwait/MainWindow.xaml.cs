using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace FileCopyApp
{
    public partial class MainWindow : Window
    {
        private bool isCanceled;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            string fromPath = FromPathTextBox.Text;
            string toPath = ToPathTextBox.Text;

            if (string.IsNullOrWhiteSpace(fromPath) || string.IsNullOrWhiteSpace(toPath) ||
                string.IsNullOrWhiteSpace(NumberOfCopiesTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(NumberOfCopiesTextBox.Text, out int numberOfCopies) || numberOfCopies <= 0)
            {
                MessageBox.Show("Invalid number of copies.");
                return;
            }

            isCanceled = false;

            try
            {
                await Task.Run(() => CopyFiles(fromPath, toPath, numberOfCopies));

                if (!isCanceled)
                {
                    MessageBox.Show("Files copied successfully.");
                }
                else
                {
                    MessageBox.Show("Copying stopped.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void CopyFiles(string sourceFile, string destinationDirectory, int numberOfCopies)
        {
            for (int i = 1; i <= numberOfCopies; i++)
            {
                if (isCanceled)
                {
                    break;
                }

                string fileName = Path.GetFileNameWithoutExtension(sourceFile);
                string extension = Path.GetExtension(sourceFile);
                string newFileName = $"{fileName}_copy_{i}{extension}";
                string destinationFile = Path.Combine(destinationDirectory, newFileName);

                try
                {
                    using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                    using (FileStream destinationStream = new FileStream(destinationFile, FileMode.CreateNew, FileAccess.Write))
                    {
                        sourceStream.CopyTo(destinationStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while copying file: {ex.Message}");
                    break;
                }
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isCanceled = true;
        }
    }
}
