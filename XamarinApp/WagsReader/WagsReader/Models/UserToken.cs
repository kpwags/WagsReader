using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WagsReader.Models
{
    public class UserToken : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int UserTokenId { get; set; }

        [OneToOne("UserId", "User", CascadeOperations = CascadeOperation.CascadeRead, ReadOnly = true)]
        public User User { get; set; }

        [ForeignKey(typeof(User))]
        public string UserId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        public UserToken()
        {

        }

        public UserToken(WagsReaderLibrary.Inoreader.Models.UserToken token)
        {
            AccessToken = token.AccessToken;
            TokenType = token.TokenType;
            ExpiresIn = token.ExpiresIn;
            RefreshToken = token.RefreshToken;
            Scope = token.Scope;
        }
    }
}