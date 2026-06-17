using System.Linq.Expressions;

namespace Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
