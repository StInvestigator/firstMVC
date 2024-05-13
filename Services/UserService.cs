using firstMVC.Models;
using System.Text.Json;

namespace firstMVC.Services
{
    public class UserService
    {
        private string _filePath;
        public User user { get; private set; }
        public UserService(string filePath) 
        {
            _filePath = filePath;
            LoadAsync().Wait();
        }
        public async Task LoadAsync()
        {
            if(File.Exists(_filePath))
            {
                var file = File.OpenRead(_filePath);
                user = await JsonSerializer.DeserializeAsync<User>(file);
                file.Close();
            }
            else
            {
                user = new User();
            }
        }
        public async Task SaveAsync()
        {
            var file = File.OpenWrite(_filePath);
            await JsonSerializer.SerializeAsync(file, user);
            file.Close();
        }
    }
}
