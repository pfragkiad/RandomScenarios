using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Http;
using Moq.Protected;
using Moq;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;

namespace RandomOrg;

public class App
{
    public static IHost GetApp(string[]? args, Action<HostBuilderContext, IServiceCollection> AddServices)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices(AddServices);
        return builder.Build();
    }


    public static IHost GetApp(string[] args)
    {
        return GetApp(args,(context, services) =>
        {
            //create a httpclientfactory (named)
            services
                .AddHttpClient(RandomOrgLottery.ClientName,
                    c => c.BaseAddress = new Uri("https://www.random.org/quick-pick"))
                .SetHandlerLifetime(TimeSpan.FromMinutes(50.0));

            //random.org http client
            //services.AddScoped<HttpClient>( s=> new HttpClient() { BaseAddress = new Uri("https://www.random.org/quick-pick") });
            services.AddScoped<RandomOrgLottery>()
            ;
        });
    }

    public static IHost GetTestApp(string[] args)
    {
        var moqFactory = MoqHttpClient.Extensions.Factory.GetMockHttpClientFactory(
            new Dictionary<string, string> { { RandomOrgLottery.ClientName, @"..\..\..\samples\sample1.html" } },
            new Dictionary<string, string> { { RandomOrgLottery.ClientName, RandomOrgLottery.BaseAddress } });

        return GetApp(args, (context, services) =>
        {
            services.AddScoped(s => new RandomOrgLottery(
                moqFactory.Object, s.GetRequiredService<ILogger<RandomOrgLottery>>()))
            ;
        })
        ;
    }
}

