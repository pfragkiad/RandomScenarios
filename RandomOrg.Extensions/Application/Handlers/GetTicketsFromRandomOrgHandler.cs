using MediatR;
using RandomOrg.Application.Queries;
using RandomOrg.Domain.Models;
using RandomOrg.Domain.Repositories;

namespace RandomOrg.Application.Handlers;

public class GetTicketsFromRandomOrgHandler : IRequestHandler<GetTicketsFromRandomOrgQuery, List<LotteryTicket>>
{
    private readonly IRandomOrgLottery _lottery;

    public GetTicketsFromRandomOrgHandler(IRandomOrgLottery lottery)
    {
        _lottery = lottery;
    }

    public async Task<List<LotteryTicket>> Handle(GetTicketsFromRandomOrgQuery request, CancellationToken cancellationToken)
    {
        return await _lottery.GetTzokerTickets(request.Tickets);
    }
}
