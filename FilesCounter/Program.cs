using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TextFileAnalyzerConsole
{
    public class FileAnalysisResult
    {
        private int _words;
        private int _lines;
        private int _punctuation;

        public void AddWords(int count)
        {
            Interlocked.Add(ref _words, count);
        }

        public void AddLines(int count)
        {
            Interlocked.Add(ref _lines, count);
        }

        public void AddPunctuation(int count)
        {
            Interlocked.Add(ref _punctuation, count);
        }

        public int Words => _words;

        public int Lines => _lines;

        public int Punctuation => _punctuation;
    }

    class Program
    {
        private static FileAnalysisResult _totalResult = new FileAnalysisResult();

        static void Main(string[] args)
        {
            string directoryPath = @"D:\accs\accs 2\accs 16.09.2024\my_accs 16.09.2024"; 
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory '{directoryPath}' does not exist.");
                return;
            }

            var files = Directory.GetFiles(directoryPath, "*.txt");

            if (files.Length == 0)
            {
                Console.WriteLine("No text files found in the directory.");
                return;
            }

            List<Task> tasks = new List<Task>();
            foreach (var file in files)
            {
                tasks.Add(Task.Run(() => AnalyzeFile(file)));
            }

            Task.WhenAll(tasks).ContinueWith(_ =>
            {
                Console.WriteLine("\nAnalysis completed:");
                Console.WriteLine($"Total Words: {_totalResult.Words}");
                Console.WriteLine($"Total Lines: {_totalResult.Lines}");
                Console.WriteLine($"Total Punctuation: {_totalResult.Punctuation}");
            }).Wait();
        }

        private static void AnalyzeFile(string filePath)
        {
            string content = File.ReadAllText(filePath);

            int wordCount = Regex.Matches(content, @"\b\w+\b").Count;
            int lineCount = File.ReadAllLines(filePath).Length;
            int punctuationCount = Regex.Matches(content, @"[.,;:–—‒…!?""''«»(){}[\]<>/]").Count;

            _totalResult.AddWords(wordCount);
            _totalResult.AddLines(lineCount);
            _totalResult.AddPunctuation(punctuationCount);

            Console.WriteLine($"File: {Path.GetFileName(filePath)}");
            Console.WriteLine($"Words: {wordCount}, Lines: {lineCount}, Punctuation: {punctuationCount}\n");
        }
    }
}
