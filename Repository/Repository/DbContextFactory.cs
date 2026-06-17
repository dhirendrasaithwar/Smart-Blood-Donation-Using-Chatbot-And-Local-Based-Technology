using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class DbContextFactory : IDesignTimeDbContextFactory<DBContext>
{
    public DBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DBContext>();

        optionsBuilder.UseSqlServer(

            "Server=(localdb)\\MSSQLLocalDB;Database=BloodDonations;Trusted_Connection = True;TrustServerCertificate=True;"

        );

        return new DBContext(optionsBuilder.Options);
    }
}