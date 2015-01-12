namespace Fresh.Windows.Core.Models
{
    public class TraktSeason
    {
        public int Number { get; set; }
        public TraktIds Ids { get; set; }
        public double Rating { get; set; }
        public int Votes { get; set; }
        public int Episode_Count { get; set; }
        public string Overview { get; set; }
        public TraktImages Images { get; set; }
    }
}
