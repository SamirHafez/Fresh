using System.Collections.Generic;
using System.Threading.Tasks;
using Fresh.Windows.Core.Services.Interfaces;
using System.Net;
using System.Net.Http;
using System.Linq;
using HtmlAgilityPack;

namespace Fresh.Windows.Core.Services
{
    public class FreeTVCrawler : ICrawlerService
    {
        private const string FreeTv = @"http://www.free-tv-video-online.me";
        private const string FreeTvQuery = @"{0}/search/?q={1}&md=shows";
        private const string FreeTvSeason = @"/season_{0}.html";

        public async Task<string> GetLink(string tvShow, int season, int episode)
        { 
            tvShow = tvShow.Replace("&", "and");

            var encodedString = WebUtility.UrlEncode(tvShow);
            var httpClient = new HttpClient(); 

            var response = await httpClient.GetStringAsync(string.Format(FreeTvQuery, FreeTv, encodedString));

            IList<string> results = GetQueryResults(response);

            var first = results[0];

            response = await httpClient.GetStringAsync(string.Format(FreeTv + first + FreeTvSeason, season));

            foreach (var link in GetEpisodeLinks(response, episode))
                try
                {
                    return await GetVideoLink(link);
                }
                catch
                { }

            return null;
        }

        private static async Task<string> GetVideoLink(string link)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(link);

            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var iframeLink = doc.DocumentNode.Descendants("body").
                First().
                Descendants("iframe").
                First().
                Attributes["src"].
                Value;

            response = await httpClient.GetStringAsync(iframeLink);

            doc.LoadHtml(response);

            var videoLink = response.Split('\"').
                Where(part =>
                part.Contains(".mp4") && part.StartsWith(@"http://")).
                FirstOrDefault();

            return videoLink;
        }

        private static IEnumerable<string> GetEpisodeLinks(string response, int episode)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var episodeHeader = doc.DocumentNode.Descendants("body").
                First().
                Descendants("tr").
                Where(tr => tr.GetAttributeValue("class", string.Empty) == "3" && tr.Descendants("a").First().GetAttributeValue("name", string.Empty) == "e" + episode).
                First();

            HtmlNode auxNode = episodeHeader.NextSibling.NextSibling.NextSibling;
            while (auxNode.GetAttributeValue("class", null) != "3")
            {
                if (auxNode.Name == "tr")
                    yield return auxNode.Descendants("a").First().Attributes["href"].Value;

                auxNode = auxNode.NextSibling;
            }
        }

        private static IList<string> GetQueryResults(string response)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            return doc.DocumentNode.Descendants("body").
                First().
                Descendants("td").
                Where(td => td.GetAttributeValue("class", null) == "mnlcategorylist").
                Select(td => td.Descendants("a").First().Attributes["href"].Value).
                ToList();
        }
    }
}
