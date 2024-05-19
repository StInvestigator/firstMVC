using firstMVC.Models;

namespace firstMVC.Services.Interfases
{
    public interface IDictionarySaveLoadEditAsync<T> where T : class
    {
        public Task AddOrEditUser(int id, T newItem);
        public Task LoadAsync();
        public Task SaveAsync();
    }
}
