using System.Linq.Expressions;

namespace _4Dorms.GenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Add(T entity);
        Task<bool> SaveChangesAsync();
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        void Remove(int id);
        void Update(T entity);
        IQueryable<T> Query();
    }
}
