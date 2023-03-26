using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoqHttpClient.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace RandomOrg.Extensions;

public class RandomOrgLotterySampleFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILoggerFactory _loggerFactory;

    private readonly SampleOptions? _options;

    public RandomOrgLotterySampleFactory(
        ILoggerFactory loggerFactory, IOptions<SampleOptions> sampleOptions)
    {
        _loggerFactory = loggerFactory;
        _options = sampleOptions.Value;

        List<HttpClientContent> samples = new List<HttpClientContent>();
        for (int iSample = 0; iSample < _options?.Samples?.Count; iSample++)
        {
            string tag = _options.Samples[iSample].Tag;
            string sample = _options.GetSamplePath(tag);
            samples.Add(new HttpClientContent(tag, RandomOrgLottery.BaseAddress, File.ReadAllText(sample)));
        }
        var moqFactory = Factory.GetMockHttpClientFactory(samples);
        _httpClientFactory = moqFactory.Object;
    }

    public RandomOrgLottery Get(string tag)
    {
        var httpClient = _httpClientFactory.CreateClient(tag);

        return new RandomOrgLottery(httpClient, _loggerFactory.CreateLogger<RandomOrgLottery>());
    }
}
