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

public class GetTicketsSampleHandler : IRequestHandler<GetTicketsSampleQuery, List<LotteryTicket>>
{
    private readonly RandomOrgLotterySampleFactory _factory;

    public GetTicketsSampleHandler(RandomOrgLotterySampleFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<LotteryTicket>> Handle(GetTicketsSampleQuery request, CancellationToken cancellationToken)
    {
        return await _factory[request.Tag].GetTzokerTickets(0);
    }
}
