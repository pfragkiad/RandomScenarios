using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoqHttpClient.Extensions;
using RandomOrg.Domain.Repositories;

namespace RandomOrg.Infrastructure.Repositories;

public class RandomOrgLotterySampleFactory : IRandomOrgLotterySampleFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerFactory _loggerFactory;

    private readonly SampleOptions? _options;

    public RandomOrgLotterySampleFactory(
        ILoggerFactory loggerFactory,
        IOptions<SampleOptions> sampleOptions)
    {
        _loggerFactory = loggerFactory;
        _options = sampleOptions.Value;

        //get client content from cached files
        List<HttpClientContent> samples = new();
        for (int iSample = 0; iSample < _options?.Samples?.Count; iSample++)
        {
            string tag = _options.Samples[iSample].Tag;
            string sample = _options.GetSamplePath(tag);
            samples.Add(new HttpClientContent(tag, RandomOrgLottery.BaseAddress, File.ReadAllText(sample)));
        }

        var moqFactory = MockHttpClientFactory.GetMockHttpClientFactory(samples);
        _httpClientFactory = moqFactory.Object;
    }

    public IRandomOrgLottery this[string tag]
    {
        get
        {
            var httpClient = _httpClientFactory.CreateClient(tag);
            return new RandomOrgLottery(httpClient, _loggerFactory.CreateLogger<RandomOrgLottery>());
        }
    }
}
