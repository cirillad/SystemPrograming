using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows; // Додайте для використання MessageBox
using Messanger.Views; // Додайте це

public class ChatClient
{
    private TcpClient client;
    private NetworkStream stream;

    public async Task ConnectToServer(string serverIp, string userName)
    {
        client = new TcpClient();
        await client.ConnectAsync(serverIp, 5000);
        stream = client.GetStream();

        // Відправка імені користувача серверу
        byte[] buffer = Encoding.UTF8.GetBytes(userName);
        await stream.WriteAsync(buffer, 0, buffer.Length);

        _ = Task.Run(ReceiveMessagesAsync);
    }

    public async Task SendMessage(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(buffer, 0, buffer.Length);
    }

    private async Task ReceiveMessagesAsync()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            int byteCount = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, byteCount);

            // Якщо отримано запит на чат
            if (message.Contains("wants to chat"))
            {
                // Відкрийте вікно підтвердження
                var result = MessageBox.Show($"{message}", "Chat Request", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Відкрийте вікно чату
                    var chatWindow = new ChatWindow("ContactNickname"); // Замість "ContactNickname" вкажіть актуальне ім'я
                    chatWindow.Show();
                }
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
