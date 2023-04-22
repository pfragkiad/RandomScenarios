using MediatR;
using RandomOrg.Domain.Models;

namespace RandomOrg.Application.Queries;

public record GetTicketsFromRandomOrgQuery(int Tickets) : IRequest<List<LotteryTicket>>;

