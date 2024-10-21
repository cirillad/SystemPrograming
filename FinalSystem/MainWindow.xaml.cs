using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace WordSearchApp
{
    public partial class MainWindow : Window
    {
        private List<string> searchResults = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Multiselect = false;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                DirectoryPath.Text = Path.GetDirectoryName(dialog.FileName);
            }
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            ResultsList.Items.Clear();
            searchResults.Clear();

            string directory = DirectoryPath.Text;
            string searchWord = SearchWord.Text;

            if (string.IsNullOrEmpty(directory) || string.IsNullOrEmpty(searchWord))
            {
                MessageBox.Show("Please specify both the directory and the word.");
                return;
            }

            var files = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories);
            int totalFiles = files.Count();
            int processedFiles = 0;

            ProgressBar.Maximum = totalFiles;

            foreach (var file in files)
            {
                int wordCount = await SearchWordInFileAsync(file, searchWord);
                if (wordCount > 0)
                {
                    string result = $"File: {Path.GetFileName(file)}, Path: {file}, Count: {wordCount}";
                    searchResults.Add(result);
                    ResultsList.Items.Add(result);
                }

                processedFiles++;
                ProgressBar.Value = processedFiles;
            }
        }

        private async Task<int> SearchWordInFileAsync(string filePath, string word)
        {
            try
            {
                string content = await File.ReadAllTextAsync(filePath);
                return content.Split(new string[] { word }, StringSplitOptions.None).Length - 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void SaveResults_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllLines(saveFileDialog.FileName, searchResults);
            }
        }
    }
}
