using Newtonsoft.Json;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class StreamCanonical
    {
        [JsonProperty("href")]
        public string href { get; set; }
    }
}
