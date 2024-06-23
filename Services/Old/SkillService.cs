using firstMVC.Models;
using firstMVC.Services.Interfases;
using System.IO;
using System.Text.Json;

namespace firstMVC.Services.Old
{
    public class SkillService : IListSaveLoadEditAsync<Skill>
    {
        private string _filePath;
        public List<Skill> skills { get; private set; }
        public SkillService(string filePath)
        {
            _filePath = filePath;
            LoadAsync().Wait();
        }
        public async Task AddOrEdit(Skill newSkill)
        {
            int index = skills.FindIndex(s => s.Id == newSkill.Id);
            if (index != -1)
            {
                skills[index] = newSkill;
            }
            else
            {
                skills.Add(newSkill);
            }

            await SaveAsync();
        }
        public async Task LoadAsync()
        {
            if (File.Exists(_filePath))
            {
                var file = File.OpenRead(_filePath);
                skills = await JsonSerializer.DeserializeAsync<List<Skill>>(file);
                file.Close();
            }
            else
            {
                skills = new List<Skill>();
            }
        }
        public async Task SaveAsync()
        {
            if (File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
            }
            var file = File.OpenWrite(_filePath);
            await JsonSerializer.SerializeAsync(file, skills);
            file.Close();
        }
    }
}
