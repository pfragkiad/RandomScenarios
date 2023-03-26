using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoqHttpClient.Extensions;
using RandomOrg.Extensions;
using System.Diagnostics;

namespace RandomOrg;

internal class Program
{
    static void Test1(string[] args)
    {

        var app = App.GetTestApp(args);

        var lottery = app.Services.GetRequiredService<RandomOrgLottery>();
        string htmlContent = File.ReadAllText(@"..\..\..\samples\sample1.html");
        //string htmlContent = File.ReadAllText(@"..\..\..\samples\sample2_only1Set.html");
        var tickets = lottery.GetTickets(htmlContent);

    }

    static async Task SampleTests(string[] args)
    {
        var app = App.GetTestApp(args);
        var logger = app.Services.GetRequiredService<ILogger<Program>>();


        var lotteryFactory = app.Services.GetService<RandomOrgLotterySampleFactory>();
        if (lotteryFactory is null)
        {
            logger.LogCritical("Factory is not defined in the services.");
            return;
        }


        SampleOptions? options = app.Services.GetRequiredService<IOptions<SampleOptions>>().Value;
        if (options is null)
        {
            logger.LogCritical("Sample options cannot be found in the Configuration.");
            return;
        }


        if (options.Samples is null || !options.Samples.Any())
        {
            logger.LogCritical("No sample files have been defined.");
            return;
        }


        foreach (string tag in options.Samples.Select(s => s.Tag))
        {
            logger.LogInformation("Sample : {tag}", tag);

            var lottery = lotteryFactory.Get(tag);

            var tickets = await lottery.GetTzokerTickets(2);
            for (int i = 0; i < tickets.Count; i++)
                logger.LogInformation("Ticket #{i}: {t}", i + 1, tickets[i]);
        }
    }


    static async Task Main(string[] args)
    {
        //var app = App.GetApp(args);
        await SampleTests(args);

    }
}