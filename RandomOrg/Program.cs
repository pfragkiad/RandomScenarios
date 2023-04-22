using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoqHttpClient.Extensions;
using RandomOrg.Application.Queries;
using RandomOrg.Domain.Repositories;
using RandomOrg.Infrastructure.Repositories;
using RandomOrg.Presentation;

namespace RandomOrg;

internal class Program
{
    static void Test1(string[] args)
    {

        var app = App.GetApp(args);

        var lottery = app.Services.GetRequiredService<RandomOrgLottery>();
        string htmlContent = File.ReadAllText(@"..\..\..\samples\sample1.html");
        //string htmlContent = File.ReadAllText(@"..\..\..\samples\sample2_only1Set.html");
        var tickets = lottery.GetTickets(htmlContent);

    }

    static async Task SampleTests(string[] args)
    {
        var app = App.GetApp(args);
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        var lotteryFactory = app.Services.GetService<IRandomOrgLotterySampleFactory>();
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

        var mediator = app.Services.GetRequiredService<IMediator>();

        foreach (string tag in options.Samples.Select(s => s.Tag))
        {
            logger.LogInformation("Sample : {tag}", tag);

            //var lottery = lotteryFactory[tag];
            //var tickets = await lottery.GetTzokerTickets(2);
            var tickets = await mediator.Send(new GetTicketsFromLocalFilesQuery(tag));

            for (int i = 0; i < tickets.Count; i++)
                logger.LogInformation("Ticket #{i}: {t}", i + 1, tickets[i]);
        }
    }

    static async Task Main(string[] args)
    {
        await SampleTests(args);

        //await RunRandomOrgTzoker(args);
    }

    private static async Task RunRandomOrgTzoker(string[] args)
    {
        var app = App.GetApp(args);
        var logger = app.Services.GetRequiredService<ILogger<Program>>();


        Console.Write("Select the number of tickets: ");
        string? response = Console.ReadLine();
        int value = 2;
        if (!int.TryParse(response, out value))
            logger.LogWarning("Could not parse response. {v} is assumed!", value);

        //var lottery = app.Services.GetRequiredService<RandomOrgLottery>();
        //var tickets = await lottery.GetTzokerTickets(value);

        var mediator = app.Services.GetRequiredService<IMediator>();
        var tickets = await mediator.Send(new GetTicketsFromRandomOrgQuery(value));

        for (int i = 0; i < tickets.Count; i++)
            logger.LogInformation("Ticket #{i}: {t}", i + 1, tickets[i]);
    }
}