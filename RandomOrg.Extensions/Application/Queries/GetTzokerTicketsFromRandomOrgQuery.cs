﻿using MediatR;
using RandomOrg.Domain.Models;

namespace RandomOrg.Application.Queries;

public record GetTzokerTicketsFromRandomOrgQuery(int Tickets) : IRequest<List<LotteryTicket>>;

