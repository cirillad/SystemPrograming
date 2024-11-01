using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ChatApp
{
    public partial class LoginWindow : Window
    {
        private string filePath = "users.json";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (ValidateLogin(username, password))
            {
                new MainMessengerWindow(username).Show(); // Передаємо нікнейм
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }


        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private bool ValidateLogin(string username, string password)
        {
            var users = LoadUsers();
            return users.Exists(u => u.Nickname == username && u.Password == password);
        }

        private List<User> LoadUsers()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            return new List<User>();
        }
    }
}
