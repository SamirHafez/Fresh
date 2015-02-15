using System.Collections.Generic;

namespace Fresh.Core.Models
{
	public class Ids
	{
		public int Trakt { get; set; }
		public string Slug { get; set; }
		public int? Tvdb { get; set; }
		public string Imdb { get; set; }
		public int? Tmdb { get; set; }
		public int? Tvrage { get; set; }
	}

	public class Airs
	{
		public string Day { get; set; }
		public string Time { get; set; }
		public string Timezone { get; set; }
	}

	public class Fanart
	{
		public string Full { get; set; }
		public string Medium { get; set; }
		public string Thumb { get; set; }
	}

	public class Poster
	{
		public string Full { get; set; }
		public string Medium { get; set; }
		public string Thumb { get; set; }
	}

	public class Screenshot
	{
		public string Full { get; set; }
		public string Medium { get; set; }
		public string Thumb { get; set; }
	}

	public class Logo
	{
		public string Full { get; set; }
	}

	public class Clearart
	{
		public string Full { get; set; }
	}

	public class Banner
	{
		public string Full { get; set; }
	}

	public class Thumb
	{
		public string Full { get; set; }
	}

	public class Images
	{
		public Fanart Fanart { get; set; }
		public Poster Poster { get; set; }
		public Logo Logo { get; set; }
		public Clearart Clearart { get; set; }
		public Banner Banner { get; set; }
		public Thumb Thumb { get; set; }
		public Screenshot Screenshot { get; set; }
	}

	public class TVShow
	{
		public string Title { get; set; }
		public int? Year { get; set; }
		public Ids Ids { get; set; }
		public string Overview { get; set; }
		public string First_Aired { get; set; }
		public Airs Airs { get; set; }
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
		public Images Images { get; set; }
	}
}
