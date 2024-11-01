using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ChatApp
{
    public partial class RegisterWindow : Window
    {
        private string filePath = "users.json";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (RegisterUser(username, password))
            {
                MessageBox.Show("Registration successful!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Username already exists.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }

        private bool RegisterUser(string username, string password)
        {
            var users = LoadUsers();

            if (users.Exists(u => u.Nickname == username))
            {
                return false;
            }

            users.Add(new User { Nickname = username, Password = password });
            SaveUsers(users);
            return true;
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

        private void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(filePath, json);
        }
    }
}
