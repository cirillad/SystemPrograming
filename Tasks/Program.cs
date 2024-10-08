using System;
using System.Threading.Tasks;

namespace Tasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Task1();
            //Task2();
            //Task3();
            //Task4();
            Task5();
        }

        static void Task1()
        {
            Task task1 = new Task(() => Console.WriteLine(DateTime.Now));
            task1.Start();

            Task task2 = Task.Factory.StartNew(() => Console.WriteLine(DateTime.Now));

            Task task3 = Task.Run(() => Console.WriteLine(DateTime.Now));

            Task.WaitAll(task1, task2, task3);
        }

        static void Task2()
        {
            Task task = Task.Run(() => DisplayPrimes(0, 1000));
            task.Wait(); 
        }

        static void Task3()
        {
            int start = 0, end = 1000;

            Task<int> task = Task.Run(() => CountPrimes(start, end));
            task.Wait(); 

            Console.WriteLine($"Кількість простих чисел: {task.Result}");
        }

        static void Task4()
        {
            int[] numbers = { 1, 5, 3, 8, 2, 7 };

            Task.Run(() =>
            {
                Console.WriteLine($"Мінімум: {numbers.Min()}");
                Console.WriteLine($"Максимум: {numbers.Max()}");
                Console.WriteLine($"Середнє: {numbers.Average()}");
                Console.WriteLine($"Сума: {numbers.Sum()}");
            }).Wait();
        }

        static void Task5()
        {
            int[] array = { 5, 1, 8, 1, 2, 7, 2, 3, 5, 9 };

            Task<int[]> removeDuplicatesTask = Task.Run(() =>
            {
                Console.WriteLine("Removing duplicates...");
                return array.Distinct().ToArray();
            });

            Task<int[]> sortTask = removeDuplicatesTask.ContinueWith(task =>
            {
                int[] result = task.Result;
                Console.WriteLine("Sorting array...");
                Array.Sort(result);
                return result;
            });

            Task<int> binarySearchTask = sortTask.ContinueWith(task =>
            {
                int[] sortedArray = task.Result;
                Console.WriteLine("Binary search for value 7...");
                return Array.BinarySearch(sortedArray, 7);
            });

            binarySearchTask.ContinueWith(task =>
            {
                int position = task.Result;
                if (position >= 0)
                {
                    Console.WriteLine($"Value found at index {position}");
                }
                else
                {
                    Console.WriteLine("Value not found");
                }
            });

            binarySearchTask.Wait();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static int CountPrimes(int start, int end)
        {
            int count = 0;
            for (int i = start; i <= end; i++)
            {
                if (IsPrime(i))
                    count++;
            }
            return count;
        }

        static bool IsPrime2(int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0) return false;
            return true;
        }

        static void DisplayPrimes(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (IsPrime(i))
                    Console.WriteLine(i);
            }
        }

        static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            for (int i = 2; i <= Math.Sqrt(number); i++)
                if (number % i == 0) return false;
            return true;
        }
    }
}
