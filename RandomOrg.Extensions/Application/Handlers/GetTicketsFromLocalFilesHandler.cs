using MediatR;
using RandomOrg.Application.Queries;
using RandomOrg.Domain.Models;
using RandomOrg.Domain.Repositories;

namespace RandomOrg.Application.Handlers;

public class GetTicketsFromLocalFilesHandler : IRequestHandler<GetTicketsFromLocalFilesQuery, List<LotteryTicket>>
{
    private readonly IRandomOrgLotterySampleFactory _factory;

    public GetTicketsFromLocalFilesHandler(IRandomOrgLotterySampleFactory factory)
    {
        _factory = factory;
    }

    public async Task<List<LotteryTicket>> Handle(GetTicketsFromLocalFilesQuery request, CancellationToken cancellationToken)
    {
        return await _factory[request.Tag].GetTzokerTickets(0);
    }
}
