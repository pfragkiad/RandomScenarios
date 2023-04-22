using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoqHttpClient.Extensions;
using RandomOrg.Application;
using RandomOrg.Domain.Repositories;
using RandomOrg.Infrastructure;
using RandomOrg.Infrastructure.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomOrg.Presentation;

public static class App
{
    public static IHost GetApp(string[]? args)
    {
        IHostBuilder builder = Host.CreateDefaultBuilder(args);
        builder
            .ConfigureServices((context,services) => {
                services
                .AddApplication()
                .AddInfrastructure(context);
            })
            .UseSerilog((context, configuration) =>
            {
                //NUGET: Serial.Settings.Configuration
                //for configuration see: https://github.com/serilog/serilog-settings-configuration
                configuration.ReadFrom.Configuration(context.Configuration); //read Serilog options from appsettings.json

                //configuration.MinimumLevel.Debug();
                //configuration.WriteTo.Console(restrictedToMinimumLevel:Serilog.Events.LogEventLevel.Information);
                //configuration.WriteTo.File(path: "logs/myapp.txt", rollingInterval: RollingInterval.Hour);
            }); //I;
        return builder.Build();
    }
    
}
