using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fresh.Core.Models
{
    public class Episode
    {
        public int Season { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public Ids Ids { get; set; }
        public int? Number_Abs { get; set; }
        public string Overview { get; set; }
        public string First_Aired { get; set; }
        public string Updated_At { get; set; }
        public double Rating { get; set; }
        public int Votes { get; set; }
        public int? Plays { get; set; }
        public List<string> Available_Translations { get; set; }
        public Images Images { get; set; }
    } 
}
