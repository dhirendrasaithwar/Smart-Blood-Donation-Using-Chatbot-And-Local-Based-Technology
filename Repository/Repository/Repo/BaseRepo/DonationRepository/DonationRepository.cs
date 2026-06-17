namespace Repository
{
    public class DonationRepository : Repository<Donation>, IDonationRepository
    {
        public DonationRepository(DBContext db) : base(db) { }
    }
    public interface IDonationRepository : IRepository<Donation> { }
}
