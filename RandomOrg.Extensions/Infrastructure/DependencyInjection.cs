using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoqHttpClient.Extensions;
using RandomOrg.Domain.Repositories;
using RandomOrg.Infrastructure.Repositories;

namespace RandomOrg.Infrastructure;

public static  class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, HostBuilderContext context)
    {
        //random.org http client
        services.AddScoped(s => new HttpClient() { BaseAddress = new Uri("https://www.random.org/quick-pick") });
        services.AddScoped<IRandomOrgLottery, RandomOrgLottery>(s =>
            new RandomOrgLottery(s.GetRequiredService<HttpClient>(), s.GetRequiredService<ILogger<RandomOrgLottery>>()));

        //for testing purposes
        var config = context.Configuration;
        services.Configure<SampleOptions>(config.GetSection(SampleOptions.SampleOptionsSection));
        //the sample factory is responsible to retrieve the randomorglottery based on instances of httpclients
        services.AddScoped<IRandomOrgLotterySampleFactory,RandomOrgLotterySampleFactory>();

        return services;
    }
}
