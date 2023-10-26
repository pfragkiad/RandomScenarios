using Microsoft.Extensions.Logging;
using RandomOrg.Domain.Models;
using RandomOrg.Domain.Repositories;
using System.Text.RegularExpressions;

namespace RandomOrg.Infrastructure.Repositories;

public class RandomOrgLottery : IRandomOrgLottery
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RandomOrgLottery> _logger;

    //public RandomOrgLotteryTicket(HttpClient httpClient)
    //{
    //    _httpClient = httpClient;
    //}

    public const string ClientName = "random.org";
    public const string BaseAddress = "https://www.random.org/quick-pick/";

    public RandomOrgLottery(
        //IHttpClientFactory httpClientFactory,
        HttpClient httpClient,
        ILogger<RandomOrgLottery> logger)
    {
        // _httpClient = httpClientFactory.CreateClient(ClientName);

        _httpClient = httpClient;
        _logger = logger;
    }



    //https://www.random.org/quick-pick/?tickets=2&lottery=5x45.1x20
    public async Task<List<LotteryTicket>> GetTzokerTickets(int ticketsCount) =>
       await GetTickets(ticketsCount, 45, 5, 20, 1);

    public async Task<List<LotteryTicket>> GetLottoTickets(int ticketsCount) =>
        await GetTickets(ticketsCount, 49, 6, 0, 0);



    //https://www.random.org/quick-pick/?tickets=2&lottery=5x45.2x5
    public async Task<List<LotteryTicket>> GetTickets(
        int ticketsCount,
        int firstSetMax, int firstSetCount,
        int secondSetMax=0, int secondSetCount=0)
    {
        //"https://www.random.org/quick-pick/?tickets=2&lottery=5x45.1x20";

        List<LotteryTicket> tickets = new List<LotteryTicket>();

        if (ticketsCount < 0 || ticketsCount > 100)
        {
            _logger.LogCritical("The number {tickets} is out of range. Tickets count must be in teh range [1, 100].", ticketsCount);
            return tickets;
        }

        string query = $"?tickets={ticketsCount}&lottery={firstSetCount}x{firstSetMax}.{secondSetCount}x{secondSetMax}";

        try
        {
            _logger.LogInformation("Waiting for response from {address}...", BaseAddress);

            string response = await _httpClient.GetStringAsync(query);
            tickets = GetTickets(response);
        }
        catch (Exception exception)
        {
            _logger.LogCritical("${exception}", exception);

        }

        return tickets;
    }

    public List<LotteryTicket> GetTickets(string htmlContent)
    {
        List<LotteryTicket> tickets = new List<LotteryTicket>();

        if (string.IsNullOrWhiteSpace(htmlContent))
        {
            _logger.LogWarning("HTML content is empty.");
            return tickets;
        }

        /*
 <h2>Lottery Quick Pick</h2>

  <p>Here are your 2 lottery tickets:</p>
<pre class="data">33-35-36-39-45 / 02
04-09-16-25-35 / 18
</pre>
<p>Timestamp: 2023-03-26 11:44:05 UTC</p>
       */


        /*
  <h2>Lottery Quick Pick</h2>

  <p>Here is your lottery ticket:</p>
<pre class="data">01-05-10-17-20 / 04
</pre>
<p>Timestamp: 2023-03-26 22:47:30 UTC</p>
         */

        //get a substring of the response to speed up regex

        int start = htmlContent.IndexOf("<pre class=\"data\">");
        if (start == -1)
        {
            _logger.LogWarning("Could not find '<pre class=\"data\">' tag.");
            return tickets;
        }

        int end = htmlContent.IndexOf("</pre>", start);
        if (end == -1)
        {
            _logger.LogWarning("Could not find end of '</pre>' tag.");
            return tickets;
        }

        htmlContent = htmlContent[start..end];

        var matches = Regex.Matches(htmlContent, @"(?<first>((\d+)-)*(\d+))(\s+/\s+(?<second>((\d+)-)*(\d+)))?");
        if (matches.Any())
        {
            foreach (Match m in matches)
            {
                string firstSet = m.Groups["first"].Value;
                string secondSet = m.Groups["second"].Value;

                tickets.Add(new LotteryTicket(
                    firstSet.Split('-').Select(t => int.Parse(t)).ToArray(),
                    secondSet != "" ? secondSet.Split('-').Select(t => int.Parse(t)).ToArray() : Array.Empty<int>()
                    ));
            }
        }
        return tickets;
    }


}
