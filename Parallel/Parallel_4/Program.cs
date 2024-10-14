using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "numbers.txt";

        List<int> numbers = ReadNumbersFromFile(filePath);

        if (numbers == null || numbers.Count == 0)
        {
            Console.WriteLine("No numbers to process.");
            return;
        }

        var parallelNumbers = numbers.AsParallel();

        int sum = parallelNumbers.Sum();
        int max = parallelNumbers.Max();
        int min = parallelNumbers.Min();

        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Max: {max}");
        Console.WriteLine($"Min: {min}");

        Console.WriteLine("Done!");
    }


    static List<int> ReadNumbersFromFile(string filePath)
    {
        List<int> numbers = new List<int>();
        try
        {
            foreach (var line in File.ReadAllLines(filePath))
            {
                if (int.TryParse(line, out int number))
                {
                    numbers.Add(number);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("File not found. Please make sure the file exists.");
        }
        return numbers;
    }
}
