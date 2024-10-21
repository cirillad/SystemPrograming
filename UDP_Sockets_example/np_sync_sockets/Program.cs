using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace np_sync_sockets
{
    class Program
    {
        static string address = "127.0.0.1"; // поточний адрес
        static int port = 8080;              // порт для приема входящих запросов

        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            UdpClient listener = new UdpClient(ipPoint);

            // Завантажуємо питання та відповіді з JSON
            Dictionary<string, List<string>> responses = LoadResponsesFromJson("D:\\all\\development\\Code\\C#\\UDP_Sockets_example\\np_sync_sockets\\responses.json");

            try
            {
                Console.WriteLine("Server started! Waiting for connection...");

                while (true)
                {
                    // Отримуємо повідомлення від клієнта
                    byte[] data = listener.Receive(ref remoteEndPoint);
                    string clientMessage = Encoding.Unicode.GetString(data);
                    Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {clientMessage} from {remoteEndPoint}");

                    // Отримуємо відповідь на основі отриманого повідомлення
                    string response = GetResponse(clientMessage, responses);

                    // Відправляємо відповідь клієнту
                    byte[] responseData = Encoding.Unicode.GetBytes(response);
                    listener.Send(responseData, responseData.Length, remoteEndPoint);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                listener.Close();
            }
        }

        // Метод для завантаження відповідей з JSON-файлу
        static Dictionary<string, List<string>> LoadResponsesFromJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, List<string>>>(json);
        }

        // Метод для отримання випадкової відповіді на запит клієнта
        static string GetResponse(string clientMessage, Dictionary<string, List<string>> responses)
        {
            if (responses.ContainsKey(clientMessage))
            {
                var possibleResponses = responses[clientMessage];
                Random random = new Random();
                return possibleResponses[random.Next(possibleResponses.Count)];
            }
            else
            {
                return "No response available for this query.";
            }
        }
    }
}
