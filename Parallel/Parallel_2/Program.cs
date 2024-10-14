namespace Parallel_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the start of the range: ");
            int start = int.Parse(Console.ReadLine());

            Console.Write("Enter the end of the range: ");
            int end = int.Parse(Console.ReadLine());

            Console.WriteLine("\nMultiplication table from {0} to {1}:\n", start, end);

            Parallel.For(start, end + 1, GenerateMultiplicationTable);

            Console.WriteLine("Done!");
        }

        static void GenerateMultiplicationTable(int number)
        {
            for (int j = 1; j <= 10; j++)
            {
                Console.WriteLine($"{number} * {j} = {number * j}");
            }
            Console.WriteLine();
        }
    }
}
