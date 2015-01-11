using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Models
{
    public class TraktCalendarItem
    {
        public string Airs_At { get; set; }
        public TraktEpisode Episode { get; set; }
        public TraktTVShow Show { get; set; }
    }
}
