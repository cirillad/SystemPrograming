using System;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter a number to calculate its factorial: ");
        int number = int.Parse(Console.ReadLine());

        long factorial = CalculateFactorial(number);
        Console.WriteLine($"Factorial of {number} is {factorial}");

        Task<int> digitCountTask = Task.Run(() => CountDigits(number));
        Task<int> digitSumTask = Task.Run(() => SumDigits(number));

        Task.WaitAll(digitCountTask, digitSumTask);

        Console.WriteLine($"Number of digits in {number}: {digitCountTask.Result}");
        Console.WriteLine($"Sum of digits in {number}: {digitSumTask.Result}");
    }

    static long CalculateFactorial(int n)
    {
        if (n < 0)
            throw new ArgumentException("Factorial is not defined for negative numbers.");

        if (n == 0 || n == 1)
            return 1;

        long[] results = new long[n];

        Parallel.For(1, n + 1, i =>
        {
            results[i - 1] = i;
        });

        long factorial = 1;

        foreach (long value in results)
        {
            factorial *= value;
        }

        return factorial;
    }

    static int CountDigits(int number)
    {
        return number.ToString().Length;
    }

    static int SumDigits(int number)
    {
        int sum = 0;
        while (number != 0)
        {
            sum += number % 10;
            number /= 10;
        }
        return sum;
    }
}
