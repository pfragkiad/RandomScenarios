using MediatR;
using RandomOrg.Domain.Models;

namespace RandomOrg.Application.Queries;

public record GetTicketsFromRandomOrgQuery(
    int TicketsCount,
    int FirstSetMax,
    int FirstSetCount,
    int SecondSetMax = 0,
    int SecondSetCount = 0) : IRequest<List<LotteryTicket>>;

