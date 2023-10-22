using MediatR;
using RandomOrg.Application.Queries;
using RandomOrg.Domain.Models;
using RandomOrg.Domain.Repositories;

namespace RandomOrg.Application.Handlers;

public class GetTzokerTicketsFromLocalFilesHandler : IRequestHandler<GetTzokerTicketsFromLocalFilesQuery, List<LotteryTicket>>
{
    private readonly IRandomOrgLotterySampleFactory _factory;

    public GetTzokerTicketsFromLocalFilesHandler(IRandomOrgLotterySampleFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<LotteryTicket>> Handle(GetTzokerTicketsFromLocalFilesQuery request, CancellationToken cancellationToken)
    {
        return await _factory[request.Tag].GetTzokerTickets(0);
    }
}
