using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatServer
{
    private TcpListener _listener;
    private List<TcpClient> _clients = new List<TcpClient>();

    public void Start(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Console.WriteLine("Server started...");

        Task.Run(async () =>
        {
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                Console.WriteLine("Client connected.");
                Task.Run(() => HandleClient(client));
            }
        });
    }

    private async Task HandleClient(TcpClient client)
    {
        var stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received: {message}");
            BroadcastMessage(message);
        }

        _clients.Remove(client);
        client.Close();
    }

    private void BroadcastMessage(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        foreach (var client in _clients)
        {
            var stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }
    }

    public void Stop()
    {
        _listener.Stop();
    }

    public static void Main(string[] args)
    {
        ChatServer server = new ChatServer();
        int port = 5000; // Встановіть порт для сервера

        server.Start(port);

        Console.WriteLine("Press Enter to stop the server...");
        Console.ReadLine();

        server.Stop();
        Console.WriteLine("Server stopped.");
    }
}
