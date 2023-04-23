using Microsoft.Extensions.Logging;
using RandomOrg.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RandomOrg.Infrastructure.Repositories;

public class RandomOrgLotteryBase
{
    protected readonly ILogger _logger;

    public RandomOrgLotteryBase(ILogger logger)
    {
        _logger = logger;
    }


    public List<LotteryTicket> GetTickets(string htmlContent)
    {
        List<LotteryTicket> tickets = new List<LotteryTicket>();

        if (string.IsNullOrWhiteSpace(htmlContent))
        {
            _logger.LogWarning("HTML content is empty.");
            return tickets;
        }

        /*
 <h2>Lottery Quick Pick</h2>

  <p>Here are your 2 lottery tickets:</p>
<pre class="data">33-35-36-39-45 / 02
04-09-16-25-35 / 18
</pre>
<p>Timestamp: 2023-03-26 11:44:05 UTC</p>
       */


        /*
  <h2>Lottery Quick Pick</h2>

  <p>Here is your lottery ticket:</p>
<pre class="data">01-05-10-17-20 / 04
</pre>
<p>Timestamp: 2023-03-26 22:47:30 UTC</p>
         */

        //get a substring of the response to speed up regex

        int start = htmlContent.IndexOf("<pre class=\"data\">");
        if (start == -1)
        {
            _logger.LogWarning("Could not find '<pre class=\"data\">' tag.");
            return tickets;
        }

        int end = htmlContent.IndexOf("</pre>", start);
        if (end == -1)
        {
            _logger.LogWarning("Could not find end of '</pre>' tag.");
            return tickets;
        }

        htmlContent = htmlContent[start..end];

        var matches = Regex.Matches(htmlContent, @"(?<first>((\d+)-)*(\d+))(\s+/\s+(?<second>((\d+)-)*(\d+)))?");
        if (matches.Any())
        {
            foreach (Match m in matches)
            {
                string firstSet = m.Groups["first"].Value;
                string secondSet = m.Groups["second"].Value;

                tickets.Add(new LotteryTicket(
                    firstSet.Split('-').Select(t => int.Parse(t)).ToArray(),
                    secondSet != "" ? secondSet.Split('-').Select(t => int.Parse(t)).ToArray() : Array.Empty<int>()
                    ));
            }
        }
        return tickets;
    }


}
