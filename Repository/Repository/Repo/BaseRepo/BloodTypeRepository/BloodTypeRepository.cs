namespace Repository
{
    public class BloodTypeRepository : Repository<BloodType>, IBloodTypeRepository
    {
        public BloodTypeRepository(DBContext db) : base(db) { }
    }
    public interface IBloodTypeRepository : IRepository<BloodType> { }
}
