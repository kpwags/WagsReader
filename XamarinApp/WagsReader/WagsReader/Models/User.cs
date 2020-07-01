using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace WagsReader.Models
{
    public class User
    {
        [PrimaryKey]
        [JsonProperty("userId")]
        public string UserId { get; set; }
        
        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty("userProfileId")]
        public string UserProfileId { get; set; }

        [JsonProperty("userEmail")]
        public string UserEmail { get; set; }

        [JsonProperty("isBloggerUser")]
        public bool IsBloggerUser { get; set; }

        [JsonProperty("signupTimeSec")]
        public long SignupTimeSec { get; set; }

        [JsonProperty("isMultiLoginEnabled")]
        public bool IsMultiLoginEnabled { get; set; }
    }
}
