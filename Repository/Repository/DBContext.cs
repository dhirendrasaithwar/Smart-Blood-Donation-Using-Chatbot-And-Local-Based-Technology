using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class DBContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        public DbSet<BloodType> BloodTypes { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<BloodStock> BloodStocks { get; set; }
        public DbSet<ChatBot>  ChatBots { get; set; }
public DbSet<DonarRequest>  DonarRequests { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new UserRoleConfig());
            builder.ApplyConfiguration(new RoleConfig());
            builder.ApplyConfiguration(new BloodRequestConfig());
            builder.ApplyConfiguration(new BloodTypeConfig());
            builder.ApplyConfiguration(new BloodStockQuantityConfig());
            builder.ApplyConfiguration(new DonationConfig());
            builder.ApplyConfiguration(new ChatBotConfig());
            builder.ApplyConfiguration(new DonarRequestConfiguration());
        }
    }
}
