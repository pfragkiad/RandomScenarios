using RandomOrg.Domain.Models;

namespace RandomOrg.Domain.Repositories;

public interface IRandomOrgLottery
{
    Task<List<LotteryTicket>> GetLottoTickets(int ticketsCount);
    Task<List<LotteryTicket>> GetTickets(int ticketsCount, int firstSetMax, int firstSetCount, int secondSetMax, int secondSetCount);
    List<LotteryTicket> GetTickets(string htmlContent);
    Task<List<LotteryTicket>> GetTzokerTickets(int ticketsCount);
}