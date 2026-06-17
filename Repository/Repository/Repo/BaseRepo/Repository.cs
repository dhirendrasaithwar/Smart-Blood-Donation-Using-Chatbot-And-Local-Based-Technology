using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DBContext _dbContext;

        public Repository(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().ToList();
        }

        public IEnumerable<T> GetByPredicate(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate).ToList();
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
