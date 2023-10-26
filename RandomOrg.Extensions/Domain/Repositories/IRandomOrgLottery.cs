using RandomOrg.Domain.Models;

namespace RandomOrg.Domain.Repositories;

public interface IRandomOrgLottery
{
    Task<List<LotteryTicket>> GetLottoTickets(int ticketsCount);
    Task<List<LotteryTicket>> GetTickets(int ticketsCount, int firstSetMax, int firstSetCount, int secondSetMax=0, int secondSetCount=0);
    List<LotteryTicket> GetTickets(string htmlContent);
    Task<List<LotteryTicket>> GetTzokerTickets(int ticketsCount);
}