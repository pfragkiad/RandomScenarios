
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoqHttpClient.Extensions;
using RandomOrg.Domain.Repositories;
using RandomOrg.Infrastructure.Repositories;

namespace RandomOrg.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, HostBuilderContext context)
    {
        //random.org http client
        services.AddRandomOrgLottery();

        //for testing purposes, the sample factory is responsible to retrieve the randomorglottery based on instances of httpclients
        services.AddRandomOrgLotterySampleFactory(context.Configuration);

        return services;
    }

    public static IServiceCollection AddRandomOrgLottery(this IServiceCollection services)
    {
        services.AddScoped(s => new HttpClient() { BaseAddress = new Uri("https://www.random.org/quick-pick") });
        services.AddScoped<IRandomOrgLottery, RandomOrgLottery>(s => new RandomOrgLottery(s.GetRequiredService<HttpClient>(), s.GetRequiredService<ILogger<RandomOrgLottery>>()));
        return services;
    }

    public static IRandomOrgLottery GetRandomOrgLottery(this IServiceProvider services) =>
        services.GetRequiredService<IRandomOrgLottery>();

    public static IServiceCollection AddRandomOrgLotterySampleFactory(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSampleOptions(configuration);
        services.AddScoped<IRandomOrgLotterySampleFactory, RandomOrgLotterySampleFactory>();
        return services;
    }


    public static IRandomOrgLotterySampleFactory GetRandomOrgLotterySampleFactory(this IServiceProvider services) =>
        services.GetRequiredService<IRandomOrgLotterySampleFactory>();


    public static IServiceCollection AddSampleOptions(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<SampleOptions>(configuration.GetSection(SampleOptions.SampleOptionsSection));


    public static SampleOptions? GetSampleOptions(this IServiceProvider services) =>
        services.GetService<IOptionsMonitor<SampleOptions>>()?.CurrentValue;
}
