using System.Collections.Generic;

namespace Fresh.Windows.Core.Models
{
    public class TraktSeason
    {
        public int Season { get; set; }
        public IList<TraktEpisode> Episodes { get; set; }
    }
}
