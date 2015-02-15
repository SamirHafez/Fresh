using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fresh.Core.Models
{
    public class CalendarItem
    {
        public string Airs_At { get; set; }
        public Episode Episode { get; set; }
        public TVShow Show { get; set; }
    }
}
