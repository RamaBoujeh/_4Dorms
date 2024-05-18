using _4Dorms.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using _4Dorms.Models;


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

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Remove(int id)
        {
            var entityToRemove = _entities.Find(id);
            if (entityToRemove != null)
            {
                _entities.Remove(entityToRemove);
            }
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.AnyAsync(predicate);
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

        public async Task<Student> StudentGetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.Email == email && s.Password == password);
        }

        public async Task<DormitoryOwner> DormOwnerGetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.DormitoryOwners.FirstOrDefaultAsync(d => d.Email == email && d.Password == password);
        }

        public async Task<Administrator> AdminGetByEmailAndPasswordAsync(string email, string password)
        {
            return await _context.Administrators.FirstOrDefaultAsync(a => a.Email == email && a.Password == password);
        }

    }
}
