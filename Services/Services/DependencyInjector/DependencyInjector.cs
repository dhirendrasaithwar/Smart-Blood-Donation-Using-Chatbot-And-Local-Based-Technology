using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repository;

namespace Services
{
    public static class DependencyInjector
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStaticListService, StaticListService>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IBloodTypeServices, BloodTypeServices>();
            services.AddScoped<IBloodRequestServices, BloodRequestServices>();
            services.AddScoped<IDonationServices, DonationServices>();
            services.AddScoped<IBloodStockService, BloodStockService>();
            services.AddScoped<IChatBotService, ChatBotService>();
            services.AddScoped<IDonarRequestService, DonarRequestService>();
            services.AddMemoryCache();

            return services;
        }
    }
}
