namespace Repository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DBContext db) : base(db) { }
    }
    public interface IRoleRepository : IRepository<Role> { }
}
