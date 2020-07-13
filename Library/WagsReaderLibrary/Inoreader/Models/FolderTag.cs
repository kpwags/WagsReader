using Newtonsoft.Json;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class FolderTag
    {
        [JsonProperty("id")]
        public string ExternalId { get; set; }

        [JsonProperty("sortid")]
        public string SortId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("unread_count")]
        public int UnreadCount { get; set; }

        [JsonProperty("unseen_count")]
        public int UnseenCount { get; set; }
    }
}
