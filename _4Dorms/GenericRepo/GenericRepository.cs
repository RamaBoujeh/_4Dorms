using _4Dorms.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using _4Dorms.Models;
using Microsoft.EntityFrameworkCore.Storage;


namespace _4Dorms.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly _4DormsDbContext _context;
        private readonly DbSet<T> _entities;
        private IDbContextTransaction _transaction; // Transaction support

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

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        public async Task AddDormitoryImagesRange(IEnumerable<DormitoryImage> dormitoryImages)
        {
            await _context.DormitoryImages.AddRangeAsync(dormitoryImages);
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

        public async Task<List<T>> GetListByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<Dormitory>> SearchDormitoriesAsync(Expression<Func<Dormitory, bool>> predicate)
        {
            return await _context.Dormitories.Include(d => d.ImageUrls).Where(predicate).ToListAsync();
        }
        // Transaction support methods
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
    }
}
