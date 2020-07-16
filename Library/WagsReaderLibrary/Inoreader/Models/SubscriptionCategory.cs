using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class SubscriptionCategory
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
