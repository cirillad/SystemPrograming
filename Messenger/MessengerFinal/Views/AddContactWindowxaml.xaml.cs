using System.Windows;

namespace ChatApp
{
    public partial class AddContactWindow : Window
    {
        public string Nickname => NicknameTextBox.Text;
        public string Label => LabelTextBox.Text;

        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void AddContactButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Nickname) && !string.IsNullOrWhiteSpace(Label))
            {
                DialogResult = true; // Закриваємо вікно з результатом
                Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields.");
            }
        }
    }
}
