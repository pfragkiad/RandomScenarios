using MediatR;
using RandomOrg.Domain.Models;

namespace RandomOrg.Application.Queries;

public record GetTzokerTicketsFromLocalFilesQuery(string Tag) : IRequest<List<LotteryTicket>>;

