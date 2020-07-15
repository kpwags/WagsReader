using Newtonsoft.Json;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class StreamOrigin
    {
        [JsonProperty("streamId")]
        public string StreamId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("htmlUrl")]
        public string HtmlUrl { get; set; }
    }
}
