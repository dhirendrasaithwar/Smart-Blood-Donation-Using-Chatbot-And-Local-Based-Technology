using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ManageBloodRequest : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public ManageBloodRequest(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {


            await SolveStatus();

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task SolveStatus()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DBContext>();

        var list = await db.BloodRequests.ToListAsync();

        if (list != null && list.Count > 0)
        {
            foreach (var bloodRequest in list)
            {
                if (bloodRequest.Status == null) continue;

                if (DateTime.Now > bloodRequest.RequiredDate &&
                    string.Equals(bloodRequest.Status, "PENDING", StringComparison.OrdinalIgnoreCase))
                {
                    bloodRequest.Status = "NotManaged";
                    db.BloodRequests.Update(bloodRequest);
                }
            }
            await db.SaveChangesAsync();
        }
    }
}