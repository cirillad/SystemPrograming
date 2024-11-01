using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp
{
    public class ChatClient
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public event Action<string> MessageReceived; // Подія для отримання повідомлень

        private string ipAddress; // Поле для зберігання IP-адреси
        private int port;         // Поле для зберігання порту
        private string nickname;  // Поле для зберігання нікнейму

        public ChatClient(string ipAddress, int port, string nickname)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.nickname = nickname;
        }

        public async Task ConnectAsync()
        {
            try
            {
                _client = new TcpClient(ipAddress, port);
                _stream = _client.GetStream();
                await ReceiveMessagesAsync();
            }
            catch (Exception ex)
            {
                // Обробка помилок при підключенні
                throw new Exception("Could not connect to server: " + ex.Message);
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                MessageReceived?.Invoke(message); // Викликаємо подію для обробки отриманого повідомлення
            }
        }

        public async Task SendMessageAsync(string message)
        {
            if (!string.IsNullOrWhiteSpace(message) && _stream != null)
            {
                string formattedMessage = $"{nickname}: {message}"; // Додаємо нікнейм перед повідомленням
                byte[] data = Encoding.UTF8.GetBytes(formattedMessage);
                await _stream.WriteAsync(data, 0, data.Length);
            }
        }

        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
        }
    }
}
