using _4Dorms.Models;
using System.Linq.Expressions;

namespace _4Dorms.GenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task Add(T entity);
        Task<bool> SaveChangesAsync();
        Task<T> FindByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        void Remove(int id);
        void Update(T entity);
        IQueryable<T> Query();
        Task AddRange(IEnumerable<T> entities);
        Task<Student> StudentGetByEmailAndPasswordAsync(string email, string password);
        Task<DormitoryOwner> DormOwnerGetByEmailAndPasswordAsync(string email, string password);
        Task<Administrator> AdminGetByEmailAndPasswordAsync(string email, string password);
        Task<List<T>> GetListByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<Dormitory>> SearchDormitoriesAsync(Expression<Func<Dormitory, bool>> predicate);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

    }
}
