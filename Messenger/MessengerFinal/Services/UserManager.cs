using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class UserManager
{
    private string usersFilePath = "users.json"; // Шлях до файлу з користувачами

    public List<User> LoadUsersFromJson()
    {
        if (!File.Exists(usersFilePath))
        {
            return new List<User>(); // Повертаємо порожній список, якщо файл не існує
        }

        string json = File.ReadAllText(usersFilePath);
        return JsonConvert.DeserializeObject<List<User>>(json) ?? new List<User>();
    }

    public bool IsUserExists(string nickname)
    {
        var users = LoadUsersFromJson();
        return users.Any(user => user.Nickname.Equals(nickname, StringComparison.OrdinalIgnoreCase));
    }
}
