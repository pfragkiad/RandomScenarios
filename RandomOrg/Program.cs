using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RandomOrg;

internal class Program
{
    static async Task Main(string[] args)
    {
        //var app = App.GetApp(args);
        var app = App.GetTestApp(args);

        var lottery = app.Services.GetRequiredService<RandomOrgLottery>();

        //string htmlContent = File.ReadAllText(@"..\..\..\samples\sample1.html");
        //string htmlContent = File.ReadAllText(@"..\..\..\samples\sample2_only1Set.html");
        //var tickets = lottery.GetTickets(htmlContent);

        var tickets = await lottery.GetTzokerTickets(2);

        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        for(int i=0;i<tickets.Count;i++) 
            logger.LogInformation("Ticket #{i}: {t}", i+1, tickets[i]);

    }
}