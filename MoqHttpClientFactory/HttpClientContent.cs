using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqHttpClient.Extensions;

public record HttpClientContent(string ClientName, string? BaseAddress, string Content);
