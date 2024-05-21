using firstMVC.Models;
using firstMVC.Services.Interfases;
using System.IO;
using System.Text.Json;

namespace firstMVC.Services
{
    public class ProfessionService : IListSaveLoadEditAsync<Profession>
    {
        private string _filePath;
        public List<Profession> professions { get; private set; }
        public ProfessionService(string filePath) 
        {
            _filePath = filePath;
            LoadAsync().Wait();
        }
        public async Task AddOrEdit(Profession newProfession)
        {
            int index = professions.FindIndex(s => s.Id == newProfession.Id);
            if (index != -1)
            {
                professions[index] = newProfession;
            }
            else
            {
                professions.Add(newProfession);
            }

            await SaveAsync();
        }
        public async Task LoadAsync()
        {
            if(File.Exists(_filePath))
            {
                var file = File.OpenRead(_filePath);
                professions = await JsonSerializer.DeserializeAsync<List<Profession>>(file);
                file.Close();
            }
            else
            {
                professions = new List<Profession>();
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
