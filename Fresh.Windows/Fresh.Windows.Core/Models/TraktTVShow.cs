using System.Collections.Generic;

namespace Fresh.Windows.Core.Models
{
    public class TraktIds
    {
        public int Trakt { get; set; }
        public string Slug { get; set; }
        public int? Tvdb { get; set; }
        public string Imdb { get; set; }
        public int? Tmdb { get; set; }
        public int? Tvrage { get; set; }
    }

    public class TraktAirs
    {
        public string Day { get; set; }
        public string Time { get; set; }
        public string Timezone { get; set; }
    }

    public class TraktFanart
    {
        public string Full { get; set; }
        public string Medium { get; set; }
        public string Thumb { get; set; }
    }

    public class TraktPoster
    {
        public string Full { get; set; }
        public string Medium { get; set; }
        public string Thumb { get; set; }
    }

    public class TraktScreenshot
    {
        public string Full { get; set; }
        public string Medium { get; set; }
        public string Thumb { get; set; }
    }

    public class TraktLogo
    {
        public string Full { get; set; }
    }

    public class TraktClearart
    {
        public string Full { get; set; }
    }

    public class TraktBanner
    {
        public string Full { get; set; }
    }

    public class TraktThumb
    {
        public string Full { get; set; }
    }

    public class TraktImages
    {
        public TraktFanart Fanart { get; set; }
        public TraktPoster Poster { get; set; }
        public TraktLogo Logo { get; set; }
        public TraktClearart Clearart { get; set; }
        public TraktBanner Banner { get; set; }
        public TraktThumb Thumb { get; set; }
        public TraktScreenshot Screenshot { get; set; }
    }

    public class TraktTrendingTVShow
    {
        public int Watchers { get; set; }
        public TraktTVShow Show { get; set; }
    }

    public class TraktTVShow
    {
        public string Title { get; set; }
        public int? Year { get; set; }
        public TraktIds Ids { get; set; }
        public string Overview { get; set; }
        public string First_Aired { get; set; }
        public TraktAirs Airs { get; set; }
        public int? Runtime { get; set; }
        public string Certification { get; set; }
        public string Network { get; set; }
        public string Country { get; set; }
        public string Updated_At { get; set; }
        public object Trailer { get; set; }
        public string Homepage { get; set; }
        public string Status { get; set; }
        public double Rating { get; set; }
        public int Votes { get; set; }
        public string Language { get; set; }
        public List<string> Available_Translations { get; set; }
        public List<string> Genres { get; set; }
        public int Aired_Episodes { get; set; }
        public TraktImages Images { get; set; }
    }

    public class TraktTVShowSearch
    {
        public double Score { get; set; }
        public TraktTVShow Show { get; set; }
    }
}
