namespace Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DBContext db) : base(db) { }
    }
    public interface IUserRepository : IRepository<User> { }
}
