using Newtonsoft.Json;

namespace CatImageApi.Domain.ExternalAPI
{
    public class Breeds
    {
        [JsonProperty("temperament")]
        public string Temperament {  get; set; }
    }
}
