using MediatR;
using RandomOrg.Domain.Models;

namespace RandomOrg.Application.Queries;

public record GetTicketsFromLocalFilesQuery(string Tag) : IRequest<List<LotteryTicket>>;

