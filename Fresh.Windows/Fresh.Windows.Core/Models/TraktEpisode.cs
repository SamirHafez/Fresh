namespace Fresh.Windows.Core.Models
{
    public class TraktEpisode
    {
        public int Season { get; set; }
        public int Episode { get; set; }
        public int Number { get; set; }
        public int Tvdb_id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public int First_aired { get; set; }
        public string First_aired_iso { get; set; }
        public int First_aired_utc { get; set; }
        public string Url { get; set; }
        public string Screen { get; set; }
        public Images Images { get; set; }
        public TraktRatings Ratings { get; set; }
    }

    public class Images
    {
        public string screen { get; set; }
    }

}
