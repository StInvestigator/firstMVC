using firstMVC.Models;
using System.IO;
using System.Text.Json;

namespace firstMVC.Services
{
    public class UserService
    {
        private string _filePath;
        public Dictionary<int,User> users { get; private set; }
        public UserService(string filePath) 
        {
            _filePath = filePath;
            LoadAsync().Wait();
        }
        public async Task AddOrEditUser(int id, User newUser)
        {
            if(users.ContainsKey(id))
            {
                users[id] = newUser;
            }
            else
            {
                users.Add(id, newUser);
            }

            await SaveAsync();
        }
        public async Task LoadAsync()
        {
            if(File.Exists(_filePath))
            {
                var file = File.OpenRead(_filePath);
                users = await JsonSerializer.DeserializeAsync<Dictionary<int, User>>(file);
                file.Close();
            }
            else
            {
                users = new Dictionary<int, User>();
            }
        }
        public async Task SaveAsync()
        {
            if (File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }
            var file = File.OpenWrite(_filePath);
            await JsonSerializer.SerializeAsync(file, users);
            file.Close();
        }
    }
}
