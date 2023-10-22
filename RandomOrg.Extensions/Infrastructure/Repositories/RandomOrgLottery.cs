using Microsoft.Extensions.Logging;
using RandomOrg.Domain.Models;
using RandomOrg.Domain.Repositories;
using System.Text.RegularExpressions;

namespace RandomOrg.Infrastructure.Repositories;

public class RandomOrgLottery : RandomOrgLotteryBase, IRandomOrgLottery
{
    private readonly HttpClient _httpClient;

    //public RandomOrgLotteryTicket(HttpClient httpClient)
    //{
    //    _httpClient = httpClient;
    //}

    public const string ClientName = "random.org";
    public const string BaseAddress = "https://www.random.org/quick-pick/";

    public RandomOrgLottery(
        //IHttpClientFactory httpClientFactory,
        HttpClient httpClient,
        ILogger<RandomOrgLottery> logger) :base( logger)
    {
        // _httpClient = httpClientFactory.CreateClient(ClientName);

        _httpClient = httpClient;
    }



    //https://www.random.org/quick-pick/?tickets=2&lottery=5x45.1x20
    public async Task<List<LotteryTicket>> GetTzokerTickets(int ticketsCount) =>
       await GetTickets(ticketsCount, 45, 5, 20, 1);

    public async Task<List<LotteryTicket>> GetLottoTickets(int ticketsCount) =>
        await GetTickets(ticketsCount, 49, 6, 1, 0);



    //https://www.random.org/quick-pick/?tickets=2&lottery=5x45.2x5
    public async Task<List<LotteryTicket>> GetTickets(
        int ticketsCount,
        int firstSetMax, int firstSetCount,
        int secondSetMax, int secondSetCount)
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

}
