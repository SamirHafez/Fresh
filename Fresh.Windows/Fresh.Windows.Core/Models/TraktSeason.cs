using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json.Converters;

namespace Fresh.Windows.Core.Models
{
    [JsonConverter(typeof(TraktSeasonJsonConverter))]
    public class TraktSeason
    {
        public int Season { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }
        public IList<TraktEpisode> Episodes { get; set; }
    }

    internal class TraktSeasonJsonConverter : CustomCreationConverter<TraktSeason>
    { 
        public override TraktSeason Create(Type objectType)
        {
            return new TraktSeason();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();

            if (properties[1].Name == "episodes" && properties[1].Value is JArray && ((JArray)properties[1].Value).First.Type != JTokenType.Integer)
                return base.ReadJson(jsonObject.CreateReader(), objectType, existingValue, serializer);

            var season = new TraktSeason
            {
                Season = (int)properties[0].Value,
                Url = (string)properties[2].Value,
                Poster = (string)properties[3].Value,
                Episodes = new List<TraktEpisode>()
            };

            foreach (int episode in properties[1].Value)
                season.Episodes.Add(new TraktEpisode
                {
                    Season = season.Season,
                    Number = episode,
                    Episode = episode
                });

            return season;
        }
    }
}
