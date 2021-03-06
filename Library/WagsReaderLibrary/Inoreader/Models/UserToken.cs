﻿using Newtonsoft.Json;
using System;

namespace WagsReaderLibrary.Inoreader.Models
{
    public class UserToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")] 
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
