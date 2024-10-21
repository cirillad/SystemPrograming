using System.Windows;

namespace ClientChat
{
    public partial class NicknameWindow : Window
    {
        public string Nickname { get; private set; }

        public NicknameWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Nickname = nicknameTextBox.Text;
            this.DialogResult = true; // Закриває вікно з результатом "True"
            this.Close();
        }
    }
}
