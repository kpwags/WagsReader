using Newtonsoft.Json;

namespace WagsReaderLibrary.Inoreader.Requests
{
    public class AuthTokenRequest
    {
        [JsonProperty("code")]
        public string Code { get; set; }
        
        [JsonProperty("redirect_uri")]
        public string RedirectURL { get; set; }

        [JsonProperty("client_id")] 
        public string ClientId { get; set; }

        [JsonProperty("client_secret")] 
        public string ClientSecret { get; set; }

        [JsonProperty("scope")] 
        public string Scope { get; set; } = "";

        [JsonProperty("grant_type")] 
        public string GrantType { get; set; } = "authorization_code";
    }
}
