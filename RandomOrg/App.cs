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
using RandomOrg.Extensions;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

using MoqHttpClient.Extensions;

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
        return GetApp(args, (context, services) =>
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
        return GetApp(args, (context, services) =>
        {
            var config = context.Configuration;

            //string basePath = config["BasePath"];
            //string sample1 = config["Samples:0"];

            //SampleOptions options= new SampleOptions();
            //config.Bind(options);

            //SampleOptions options = config.Get<SampleOptions>()!;

            services.Configure<SampleOptions>(config.GetSection(SampleOptions.SampleOptionsSection));
            services.AddScoped<RandomOrgLotterySampleFactory>();
            ;
        })
        ;
    }
}

