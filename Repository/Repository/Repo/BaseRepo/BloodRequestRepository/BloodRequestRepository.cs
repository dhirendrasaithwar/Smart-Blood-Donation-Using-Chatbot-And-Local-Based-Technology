namespace Repository
{
    public class BloodRequestRepository: Repository<BloodRequest>, IBloodRequestRepository
    {
        public BloodRequestRepository(DBContext db) : base(db) { }
    }
    public interface IBloodRequestRepository : IRepository<BloodRequest> { }
    
}
