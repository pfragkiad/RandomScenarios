using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace RandomOrg.Extensions;

public class RandomOrgLottery
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RandomOrgLottery> _logger;

    //public RandomOrgLotteryTicket(HttpClient httpClient)
    //{
    //    _httpClient = httpClient;
    //}

    public const string ClientName = "random.org";
    public const string BaseAddress = "https://www.random.org/quick-pick";

    public RandomOrgLottery(
        //IHttpClientFactory httpClientFactory,
        HttpClient httpClient,
        ILogger<RandomOrgLottery> logger)
    {
        // _httpClient = httpClientFactory.CreateClient(ClientName);
        _httpClient = httpClient;
        _logger = logger;
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

        //get a substring of the response to speed up regex

        int start = htmlContent.IndexOf("Here are your");
        if (start == -1)
        {
            _logger.LogWarning("Could not find 'Here are your' text.");
            return tickets;
        }
        start = htmlContent.IndexOf("lottery tickets", start);
        if (start == -1)
        {
            _logger.LogWarning("Could not find 'lottery tickets' text.");
            return tickets;
        }

        int end = htmlContent.IndexOf("Timestamp", start);
        if (end == -1)
        {
            _logger.LogWarning("Could not find 'Timestamp' text.");
            return tickets;
        }

        htmlContent = htmlContent.Substring(start, end - start);

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

    public async Task<List<LotteryTicket>> GetTzokerTickets(int ticketsCount) =>
       await GetTickets(ticketsCount, 45, 5, 20, 1);


    //https://www.random.org/quick-pick/?tickets=2&lottery=5x45.2x5
    public async Task<List<LotteryTicket>> GetTickets(
        int ticketsCount,
        int firstSetMax, int firstSetCount,
        int secondSetMax, int secondSetCount)
    {
        string query = $"/?tickets={ticketsCount}&lottery={firstSetCount}x{firstSetMax}.{secondSetCount}x{secondSetMax}";

        List<LotteryTicket> tickets = new List<LotteryTicket>();

        try
        {
            string response = await _httpClient.GetStringAsync(query);
            tickets = GetTickets(response);
        }
        catch (Exception exception)
        {
            _logger.LogCritical("${exception}", exception);

        }

        return tickets;
    }

}
