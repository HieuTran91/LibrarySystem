using LibraryProject.Data;
using LibraryProject.Models;

namespace LibraryProject.Repositories.GenericRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        void Attach(T entity);
    }
}
