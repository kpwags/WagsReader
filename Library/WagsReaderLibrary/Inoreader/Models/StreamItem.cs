using Newtonsoft.Json;
using System.Collections.Generic;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class StreamItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("published")]
        public long Published { get; set; }

        [JsonProperty("updated")]
        public long Updated { get; set; }

        [JsonProperty("canonical")]
        public List<StreamCanonical> Canonical { get; set; }

        [JsonProperty("summary")]
        public StreamSummary Summary { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("origin")]
        public StreamOrigin Origin { get; set; }
    }
}
