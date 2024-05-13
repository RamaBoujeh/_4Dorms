using _4Dorms.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;


namespace _4Dorms.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly _4DormsDbContext _context;
        private readonly DbSet<T> _entities;

        public GenericRepository(_4DormsDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async void Add(T entity)
        {
            _entities.Add(entity);
        }
        public void Remove(int id)
        {
            var entityToRemove = _entities.Find(id);
            _entities.Remove(entityToRemove);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
        }

        public async Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }
        public IQueryable<T> Query()
        {
            return _entities.AsQueryable();
        }
        public async Task<IEnumerable<T>> SearchDormitories(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }
    }
}
