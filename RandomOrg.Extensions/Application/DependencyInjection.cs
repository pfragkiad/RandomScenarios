using Microsoft.Extensions.DependencyInjection;
using RandomOrg.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
