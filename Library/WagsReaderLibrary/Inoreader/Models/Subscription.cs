using Newtonsoft.Json;
using System.Collections.Generic;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class Subscription
    {
        [JsonProperty("id")]
        public string SubscriptionId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("categories")]
        public List<SubscriptionCategory> Categories { get; set; }

        [JsonProperty("sortid")]
        public string SortId { get; set; }

        [JsonProperty("firstitemmsec")]
        public long FirstItemSec { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("htmlUrl")]
        public string HtmlUrl { get; set; }

        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }
    }
}
