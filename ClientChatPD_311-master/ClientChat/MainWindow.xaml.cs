using System.Collections.ObjectModel;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ClientChat
{
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        UdpClient client;
        ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        private string nickname;

        public MainWindow()
        {
            InitializeComponent();

            NicknameWindow nicknameWindow = new NicknameWindow();

            if (nicknameWindow.ShowDialog() == true)
            {
                nickname = nicknameWindow.Nickname;
                this.DataContext = messages;
                client = new UdpClient();
                string address = ConfigurationManager.AppSettings["ServerAddress"]!;
                short port = short.Parse(ConfigurationManager.AppSettings["ServerPort"]!);
                serverEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
            }
            else
            {
                this.Close();
            }
        }

        private void Leave_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = "$<leave>";
            SendMessage(message);
            this.Close();
        }

        private void Join_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = $"$<join> {nickname}"; 
            SendMessage(message);
            Listen();
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            string message = msgTextBox.Text;

            if (!string.IsNullOrWhiteSpace(message))
            {
                string formattedMessage = $"{nickname}: {message}";
                SendMessage(formattedMessage);
                msgTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a valid message.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private async void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }

        private async void Listen()
        {
            try
            {
                while (true)
                {
                    var data = await client.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(data.Buffer);

                    if (message.StartsWith("$<join>"))
                    {
                        string[] parts = message.Split(new[] { ' ' }, 2);
                        string newUser = parts.Length > 1 ? parts[1] : "Unknown user";
                        messages.Add(new MessageInfo($"{newUser} has joined the chat", DateTime.Now));
                    }
                    else if (message.StartsWith("$<leave>"))
                    {
                        string leavingUser = message.Substring(8);
                        messages.Add(new MessageInfo($"{leavingUser} has left the chat", DateTime.Now));
                    }
                    else
                    {
                        messages.Add(new MessageInfo(message, DateTime.Now)); 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
