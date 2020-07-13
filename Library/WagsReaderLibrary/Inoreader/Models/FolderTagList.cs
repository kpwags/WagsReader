using Newtonsoft.Json;
using System.Collections.Generic;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class FolderTagList
    {
        [JsonProperty("tags")]
        public List<FolderTag> Tags { get; set; }
    }
}
