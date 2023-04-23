using Microsoft.Extensions.DependencyInjection;
using RandomOrg.Infrastructure.Repositories;

namespace RandomOrg.Application
{
    public  static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(RandomOrgLottery).Assembly));

            return services;

        }
    }
}
