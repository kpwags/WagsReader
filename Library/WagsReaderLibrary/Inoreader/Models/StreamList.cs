using Newtonsoft.Json;
using System.Collections.Generic;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class StreamList
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("items")]
        public List<StreamItem> Items { get; set; }
    }
}
