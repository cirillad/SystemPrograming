using System.Collections.Generic;
using System.IO;
using Messenger.Models;
using Newtonsoft.Json;

public class ContactManager
{
    private string contactsFilePath;

    public ContactManager(string userNickname)
    {
        // Встановлюємо шлях до файлу контактів для конкретного користувача
        contactsFilePath = $"{userNickname}_contacts.json"; // Виглядає як 'nickname_contacts.json'
        EnsureContactsFileExists();
    }

    private void EnsureContactsFileExists()
    {
        // Якщо файл не існує, створюємо новий
        if (!File.Exists(contactsFilePath))
        {
            File.WriteAllText(contactsFilePath, JsonConvert.SerializeObject(new List<Contact>())); // Створюємо пустий JSON масив
        }
    }

    public List<Contact> LoadContactsFromJson()
    {
        string json = File.ReadAllText(contactsFilePath);
        return JsonConvert.DeserializeObject<List<Contact>>(json) ?? new List<Contact>();
    }

    public void SaveContactsToJson(List<Contact> contacts)
    {
        string json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
        File.WriteAllText(contactsFilePath, json);
    }
}
