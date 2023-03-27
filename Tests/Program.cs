using System.Diagnostics;

namespace Tests;

internal class Program
{
    static void Main(string[] args)
    {
        var words = Enumerable.Range(0, 10).ToArray();

        Index r = ^1;
        Range r2 = 1..;
        Index r3 = 1;

        //var sq = Sequence(100);

        int i = 10;
        Range r252 = ^(i + 1)..; 
 //      r252.
        
        Span<int> s = words[^1..];

        Debugger.Break();


    }
}