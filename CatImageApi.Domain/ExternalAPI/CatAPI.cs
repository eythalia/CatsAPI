using Newtonsoft.Json;

namespace CatImageApi.Domain.ExternalAPI
{
    public class CatAPI
    {
        [JsonProperty("id")]
        public string CatId { get; set; }

        [JsonProperty("width")]

        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("breeds")]
        private Breeds[] Breeds { get; set; }

        public string[]? Temperaments => Breeds?.First()?.Temperament?.Split(",");

    }
}
