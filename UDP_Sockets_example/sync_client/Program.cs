using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace sync_client
{
    class Program
    {
        static string address = "127.0.0.1"; // адрес сервера
        static int port = 8080;              // порт сервера

        static void Main(string[] args)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                IPEndPoint remoteIpPoint = new IPEndPoint(IPAddress.Any, 0);

                UdpClient client = new UdpClient();
                string message = "";

                while (message != "end")
                {
                    Console.Write("Enter a message: ");
                    message = Console.ReadLine();

                    byte[] data = Encoding.Unicode.GetBytes(message);
                    client.Send(data, data.Length, ipPoint);

                    // Отримуємо відповідь від сервера
                    data = client.Receive(ref remoteIpPoint);
                    string response = Encoding.Unicode.GetString(data);

                    Console.WriteLine("Server response: " + response);
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
