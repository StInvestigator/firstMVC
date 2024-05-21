﻿using firstMVC.Models;
using firstMVC.Services.Interfases;
using System.IO;
using System.Text.Json;

namespace firstMVC.Services
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
        public async Task LoadAsync()
        {
            if(File.Exists(_filePath))
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
