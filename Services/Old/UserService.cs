using firstMVC.Models;
using firstMVC.Models.Forms;
using firstMVC.Services.Interfases;
using System.IO;
using System.Text.Json;

namespace firstMVC.Services.Old
{
    public class UserService : IListSaveLoadEditAsync<User>
    {
        private string _filePath;
        public List<User> users { get; private set; }

        public UserService(string filePath)
        {
            _filePath = filePath;
            LoadAsync().Wait();
        }
        public async Task AddOrEdit(User newUser)
        {
            int index = users.FindIndex(s => s.Id == newUser.Id);
            if (index != -1)
            {
                newUser.Skills = users[index].Skills;
                users[index] = newUser;
            }
            else
            {
                newUser.Skills = new List<UserSkill>();
                users.Add(newUser);
            }
            await SaveAsync();
        }

        public async Task AddOrEdit(UserForm form, Image? img, List<Image>? gallery)
        {
            await AddOrEdit(new User
            {
                Id = form.Id,
                Name = form.Name,
                Age = form.Age,
                Balance = form.Balance,
                Birthday = form.Birthday,
                IsMale = form.IsMale,
                //ProfessionId = form.ProfessionId,
                Image = img,
                Gallery = gallery
            });
        }
        public void UpdateSkills(Skill newSkill)
        {
            foreach (var us in users)
            {
                var skill = us.Skills.Find(s => s.Skill.Id == newSkill.Id);
                if (skill != null)
                {
                    skill.Skill = newSkill;
                }
            }
        }
        public void DeleteSkills(Skill toDelete)
        {
            foreach (var us in users)
            {
                var skill = us.Skills.Find(s => s.Skill.Id == toDelete.Id);
                if (skill != null)
                {
                    us.Skills.Remove(skill);
                }
            }
        }
        public async Task LoadAsync()
        {
            if (File.Exists(_filePath))
            {
                var file = File.OpenRead(_filePath);
                users = await JsonSerializer.DeserializeAsync<List<User>>(file);
                file.Close();
            }
            else
            {
                users = new List<User>();
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
