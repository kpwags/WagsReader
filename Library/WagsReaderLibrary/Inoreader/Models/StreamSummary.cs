using Newtonsoft.Json;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class StreamSummary
    {
        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
