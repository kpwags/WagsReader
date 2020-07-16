using Newtonsoft.Json;
using System.Collections.Generic;

namespace WagsReaderLibrary.Inoreader.Models
{
    [JsonObject]
    public class SubscriptionList
    {
        [JsonProperty("subscriptions")]
        public List<Subscription> Subscriptions { get; set; }
    }
}
