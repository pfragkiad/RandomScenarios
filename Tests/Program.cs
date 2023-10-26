using Microsoft.Extensions.DependencyInjection; //ServiceCollection
using System.Diagnostics;
using System.Security.Authentication.ExtendedProtection;

namespace Tests;


public class MyHttpClient {
    private readonly HttpClient _client;

    public MyHttpClient(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://ARKOUDES");
    }

    public string Read() => _client.BaseAddress!.ToString();
}

internal class Program
{
    static void Main(string[] args)
    {

        //https://www.youtube.com/watch?v=1rw9eDFTTlY
        ServiceCollection serviceCollection = new(); //NUGET: Microsoft.Extensions.DependencyInjection

        //generic client
        //serviceCollection.AddHttpClient(); //NUGET: Microsoft.Extensions.Http

        //named client
        serviceCollection.AddHttpClient("keftes", c =>
        {
            c.BaseAddress = new Uri("http://KEFTES");
            c.DefaultRequestHeaders.Add("MPIFTEKIA", "8");
        });

        //typed client
        serviceCollection.AddHttpClient<MyHttpClient>(); //does NOT need any other AddHttpClient in order to work!

        ServiceProvider provider = serviceCollection.BuildServiceProvider();


        var typedClient = provider.GetRequiredService<MyHttpClient>();
        Console.WriteLine(typedClient.Read()); 

        //IndexTests();

    }

    private static void ConfigureServices(IServiceCollection services)
    {
    }



    private static void IndexTests()
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