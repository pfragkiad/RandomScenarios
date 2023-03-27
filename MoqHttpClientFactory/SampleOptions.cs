using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqHttpClient.Extensions;

/*
{
  "SampleOptions": {
    "BasePath": "..\\..\\..\\samples",
    "Samples": [
      {
        "Tag": "2 sets",
        "File": "sample1.html"
      },
      {
        "Tag": "1 set",
        "File": "sample2_only1Set.html"
      }
    ]
  }
}
 */

public record SampleEntry(string Tag, string File);

public class SampleOptions
{
    public const string SampleOptionsSection = nameof(SampleOptions);

    public string? BasePath { get; set; }

    public IList<SampleEntry>? Samples { get; set; }

    public Dictionary<string, SampleEntry>? SampleDictionary { get => Samples?.ToDictionary(s => s.Tag, s => s); }

    public string GetSamplePath(string tag) =>
        Path.Combine(BasePath ?? "", Samples?.FirstOrDefault(x => x.Tag == tag)?.File ?? "");
}
