using MediatR;
using RandomOrg.Extensions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomOrg.Extensions.Handlers;

public class GetTicketsHandler : IRequestHandler<GetTicketsQuery, List<LotteryTicket>>
{
    private readonly RandomOrgLottery _lottery;

    public GetTicketsHandler(RandomOrgLottery lottery)
    {
        _lottery = lottery;
    }

    public async Task<List<LotteryTicket>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        return await _lottery.GetTzokerTickets(request.Tickets);
    }
}
