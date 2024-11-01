using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Messenger.Models;


namespace ChatApp
{
    public partial class MainMessengerWindow : Window
    {
        private string CurrentUserNickname; // Нікнейм поточного користувача
        private ObservableCollection<Contact> contacts; // Зміна на ObservableCollection
        private ChatClient chatClient; // Клієнт для чату

        public MainMessengerWindow(string currentUserNickname)
        {
            InitializeComponent();
            CurrentUserNickname = currentUserNickname;

            // Ініціалізація клієнта чату
            chatClient = new ChatClient("127.0.0.1", 5000, CurrentUserNickname);
            chatClient.MessageReceived += OnMessageReceived; // Підписка на подію отримання повідомлень

            // Підключення до сервера
            ConnectToServer();
            LoadContacts();
        }

        private async void ConnectToServer()
        {
            try
            {
                await chatClient.ConnectAsync(); // Підключення до сервера
                MessageBox.Show("Connected to the server."); // Повідомлення про успішне підключення
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to the server: {ex.Message}"); // Обробка помилок підключення
            }
        }

        private void LoadContacts()
        {
            ContactManager contactManager = new ContactManager(CurrentUserNickname);
            contacts = new ObservableCollection<Contact>(contactManager.LoadContactsFromJson());
            ContactsListBox.ItemsSource = contacts;
        }

        private void OnMessageReceived(string message)
        {
            Dispatcher.Invoke(() => AddMessageToChat(message)); // Додаємо повідомлення в чат
        }

        private void AddMessageToChat(string message)
        {
            TextBlock messageBlock = new TextBlock { Text = message };
            MessagesPanel.Children.Add(messageBlock);
            ChatScrollViewer.ScrollToEnd(); // Прокрутка до кінця
        }

        private void ContactsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обробка зміни вибору контакту, якщо потрібно
        }

        private void RefreshContactsListBox()
        {
            ContactsListBox.ItemsSource = null;
            ContactsListBox.ItemsSource = contacts;
        }

        private void AddContact_Click(object sender, RoutedEventArgs e)
        {
            AddContactWindow addContactWindow = new AddContactWindow();
            if (addContactWindow.ShowDialog() == true)
            {
                string nickname = addContactWindow.Nickname;
                string label = addContactWindow.Label;

                if (IsUserExists(nickname))
                {
                    if (!IsNicknameExists(nickname))
                    {
                        var newContact = new Contact { Nickname = nickname, Label = label };
                        contacts.Add(newContact);

                        ContactManager contactManager = new ContactManager(CurrentUserNickname);
                        List<Contact> existingContacts = contactManager.LoadContactsFromJson();
                        existingContacts.Add(newContact);
                        contactManager.SaveContactsToJson(existingContacts);

                        RefreshContactsListBox();
                    }
                    else
                    {
                        MessageBox.Show("Contact with this nickname already exists.");
                    }
                }
                else
                {
                    MessageBox.Show("User does not exist.");
                }
            }
        }

        private bool IsUserExists(string nickname)
        {
            UserManager userManager = new UserManager();
            List<User> users = userManager.LoadUsersFromJson();
            return users.Any(user => user.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsNicknameExists(string nickname)
        {
            return contacts.Any(c => c.Nickname.Equals(nickname));
        }

        private void RemoveContact_Click(object sender, RoutedEventArgs e)
        {
            if (ContactsListBox.SelectedItem != null)
            {
                var contactToRemove = ContactsListBox.SelectedItem as Contact;
                contacts.Remove(contactToRemove);

                UpdateContactsJson();
                RefreshContactsListBox();
            }
            else
            {
                MessageBox.Show("Please select a contact to remove.");
            }
        }

        private void RenameContact_Click(object sender, RoutedEventArgs e)
        {
            if (ContactsListBox.SelectedItem != null)
            {
                var contact = ContactsListBox.SelectedItem as Contact;
                string newLabel = Microsoft.VisualBasic.Interaction.InputBox("Enter new label:", "Rename Contact", contact.Label);
                if (!string.IsNullOrWhiteSpace(newLabel))
                {
                    contact.Label = newLabel;
                    UpdateContactsJson();
                    RefreshContactsListBox();
                }
            }
            else
            {
                MessageBox.Show("Please select a contact to rename.");
            }
        }

        private void UpdateContactsJson()
        {
            ContactManager contactManager = new ContactManager(CurrentUserNickname);
            List<Contact> contactsList = contacts.ToList();
            contactManager.SaveContactsToJson(contactsList);
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            if (!string.IsNullOrWhiteSpace(message))
            {
                await chatClient.SendMessageAsync(message);
                MessageTextBox.Clear(); // Очищення текстового поля
            }
        }

    }
}
