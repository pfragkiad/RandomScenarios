namespace RandomOrg.Domain.Models;

public record LotteryTicket(int[] FirstSet, int[] SecondSet)
{
    public override string ToString()
    {
        string s = string.Join("-", FirstSet.Select(n => $"{n:00}"));
        if (SecondSet.Any()) s += " | " + string.Join("-", SecondSet.Select(n => $"{n:00}"));
        return s;
    }
}