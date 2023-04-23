using RandomOrg.Application;

namespace RandomOrg.Domain.Repositories;

public interface IRandomOrgLotterySampleFactory
{
    IRandomOrgLottery this[string tag] { get; }
}