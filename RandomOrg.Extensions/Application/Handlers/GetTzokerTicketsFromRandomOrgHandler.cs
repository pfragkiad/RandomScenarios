using MediatR;
using RandomOrg.Application.Queries;
using RandomOrg.Domain.Models;
using RandomOrg.Domain.Repositories;

namespace RandomOrg.Application.Handlers;

public class GetTzokerTicketsFromRandomOrgHandler : IRequestHandler<GetTzokerTicketsFromRandomOrgQuery, List<LotteryTicket>>
{
    private readonly IRandomOrgLottery _lottery;

    public GetTzokerTicketsFromRandomOrgHandler(IRandomOrgLottery lottery)
    {
        _lottery = lottery;
    }

    public async Task<List<LotteryTicket>> Handle(GetTzokerTicketsFromRandomOrgQuery request, CancellationToken cancellationToken)
    {
        return await _lottery.GetTzokerTickets(request.Tickets);
    }
}
