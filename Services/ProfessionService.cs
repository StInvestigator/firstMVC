using firstMVC.Models;
using firstMVC.Services.Interfases;
using System.IO;
using System.Text.Json;

namespace firstMVC.Services
{
    public class ProfessionService : IDictionarySaveLoadEditAsync<Profession>
    {
        private string _filePath;
        public Dictionary<int, Profession> professions { get; private set; }
        public ProfessionService(string filePath) 
        {
            _filePath = filePath;
            LoadAsync().Wait();
        }
        public async Task AddOrEditUser(int id, Profession newProfession)
        {
            if(professions.ContainsKey(id))
            {
                professions[id] = newProfession;
            }
            else
            {
                professions.Add(id, newProfession);
            }

            await SaveAsync();
        }
        public async Task LoadAsync()
        {
            if(File.Exists(_filePath))
            {
                var file = File.OpenRead(_filePath);
                professions = await JsonSerializer.DeserializeAsync<Dictionary<int, Profession>>(file);
                file.Close();
            }
            else
            {
                professions = new Dictionary<int, Profession>();
            }
        }
        public async Task SaveAsync()
        {
            if (File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }
            var file = File.OpenWrite(_filePath);
            await JsonSerializer.SerializeAsync(file, professions);
            file.Close();
        }
    }
}
