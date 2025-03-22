using LibraryProject.Data;
using LibraryProject.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryProject.Repositories.GenericRepository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            var name =  typeof(T).Name ;
            var en = entity;
            Console.WriteLine($"[DEBUG] INSERT INTO name: en");
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            Console.WriteLine("OK");
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
             await _dbSet.ToListAsync();


        public async Task<T> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);


        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
