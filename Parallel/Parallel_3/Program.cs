using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "";

        List<int> numbers = ReadNumbersFromFile(filePath);

        if (numbers == null || numbers.Count == 0)
        {
            Console.WriteLine("No numbers to process.");
            return;
        }

        Console.WriteLine("Calculating factorials...");

        Parallel.ForEach(numbers, number =>
        {
            long factorial = CalculateFactorial(number);
            Console.WriteLine($"Factorial of {number} is {factorial}");
        });

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

    static long CalculateFactorial(int n)
    {
        if (n < 0)
            throw new ArgumentException("Factorial is not defined for negative numbers.");

        long result = 1;

        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }

        return result;
    }
}
