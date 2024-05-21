using firstMVC.Models;

namespace firstMVC.Services.Interfases
{
    public interface IListSaveLoadEditAsync<T> where T : class
    {
        public Task AddOrEdit(T newItem);
        public Task LoadAsync();
        public Task SaveAsync();
    }
}
