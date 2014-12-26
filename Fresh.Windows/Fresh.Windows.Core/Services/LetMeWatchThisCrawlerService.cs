using Fresh.Windows.Core.Services.Interfaces;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services
{
    public class LetMeWatchThisCrawlerService : ICrawlerService
    {
        private const string LetMeWatchThis = @"http://www.primewire.ag";
        private const string LetMeWatchThisQuery = @"{0}/index.php?search_keywords={1}&key=c0950867e511cf04&search_section=2";

        public async Task<string> GetLink(string tvShow, int season, int episode, params string[] exclude)
        {
            var encodedString = WebUtility.UrlEncode(tvShow);
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(string.Format(LetMeWatchThisQuery, LetMeWatchThis, encodedString));

            IList<string> results = GetQueryResults(response);

            var first = results[0];

            response = await httpClient.GetStringAsync(LetMeWatchThis + first);

            foreach (var link in await GetEpisodeLinks(response, season, episode))
                try
                {
                    var videoLink = await GetVideoLink(link);

                    if (videoLink == null || exclude.Contains(videoLink))
                        continue;

                    return videoLink;
                }
                catch
                { }

            return null;
        }

        private static async Task<string> GetVideoLink(string link)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetStringAsync(LetMeWatchThis + link);

            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var iframeLink = doc.DocumentNode.Descendants("frame").
                ElementAt(1).
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

        private static async Task<IList<string>> GetEpisodeLinks(string response, int season, int episode)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var episodeLink = doc.DocumentNode.Descendants("body").
                First().
                Descendants("div").
                Where(div => div.GetAttributeValue("class", null) == "show_season" && div.GetAttributeValue("data-id", null) == season.ToString()).
                First().
                Descendants("a").
                Where(a => a.Attributes["href"].Value.EndsWith(string.Format("season-{0}-episode-{1}", season, episode))).
                First().Attributes["href"].Value;

            var client = new HttpClient();

            response = await client.GetStringAsync(LetMeWatchThis + episodeLink);

            doc.LoadHtml(response);

            var links = doc.DocumentNode.Descendants("body").
                First().
                Descendants("span").
                Where(span => span.GetAttributeValue("class", null) == "movie_version_link").
                Select(span => span.Descendants("a").First()).
                Where(a => a.GetAttributeValue("class", null) != "no_c_link").
                Select(a => a.Attributes["href"].Value);

            return links.ToList();
        }

        private static IList<string> GetQueryResults(string response)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            return doc.DocumentNode.Descendants("body").
                First().
                Descendants("div").
                Where(div => div.GetAttributeValue("class", string.Empty).Contains("index_item")).
                Select(div => div.Descendants("a").First().Attributes["href"].Value).
                ToList();
        }

    }
}
