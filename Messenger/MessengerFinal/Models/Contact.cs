namespace Messenger.Models
{
    public class Contact
    {
        public string Nickname { get; set; }
        public string Label { get; set; }

        public override string ToString()
        {
            return $"{Label} ({Nickname})"; // Відображення у списку контактів
        }
    }
}
