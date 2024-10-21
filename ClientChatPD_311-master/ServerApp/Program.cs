using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;

namespace ServerApp
{
    public class ChatServer
    {
        const short port = 4040;
        const string JOIN_CMD = "$<join>";
        const string LEAVE_CMD = "$<leave>";
        UdpClient server;
        IPEndPoint clientEndPoint = null;
        List<IPEndPoint> members;
        Dictionary<IPEndPoint, string> userNames; 

        public ChatServer()
        {
            server = new UdpClient(port);
            members = new List<IPEndPoint>();
            userNames = new Dictionary<IPEndPoint, string>();
        }

        public void Start()
        {
            while (true)
            {
                byte[] data = server.Receive(ref clientEndPoint);
                string message = Encoding.UTF8.GetString(data);
                Console.WriteLine($"Message : {message} from : {clientEndPoint}. " +
                    $"Date : {DateTime.Now.ToShortTimeString()}");

                if (message.StartsWith(JOIN_CMD))
                {
                    string nickname = message.Substring(JOIN_CMD.Length + 1); 
                    AddMember(clientEndPoint, nickname);
                }
                else if (message.StartsWith(LEAVE_CMD))
                {
                    RemoveMember(clientEndPoint);
                }
                else
                {
                    SendToAll(data); 
                }
            }
        }


        private void AddMember(IPEndPoint member, string nickname)
        {
            if (userNames.ContainsValue(nickname))
            {
                string errorMessage = $"$<error> Nickname '{nickname}' is already taken. Please choose another one.";
                server.SendAsync(Encoding.UTF8.GetBytes(errorMessage), errorMessage.Length, member);
                Console.WriteLine($"Nickname '{nickname}' is already in use.");
                return; 
            }

            members.Add(member);
            userNames[member] = nickname;
            Console.WriteLine($"Member {nickname} was added!");

            string joinMessage = $"$<join> {nickname} has joined the chat.";
            SendToAll(Encoding.UTF8.GetBytes(joinMessage));
        }


        private void RemoveMember(IPEndPoint member)
        {
            if (members.Contains(member))
            {
                string nickname = userNames[member];
                members.Remove(member);
                userNames.Remove(member);
                Console.WriteLine($"Member {nickname} was removed!");
                SendToAll(Encoding.UTF8.GetBytes($"$<leave> {nickname}"));
            }
        }

        private void SendToAll(byte[] data)
        {
            foreach (var member in members)
            {
                server.SendAsync(data, data.Length, member);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();
            server.Start();
        }
    }
}
