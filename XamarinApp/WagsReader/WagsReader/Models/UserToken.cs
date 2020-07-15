using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WagsReader.Models
{
    [Table("UserToken")]
    public class UserToken : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [OneToOne("UserId", "User", CascadeOperations = CascadeOperation.CascadeRead, ReadOnly = true)]
        public User User { get; set; }

        [ForeignKey(typeof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("access_token")]
        public string AccessToken { get; set; }

        [Column("token_type")]
        public string TokenType { get; set; }

        [Column("expires_in")]
        public long ExpiresIn { get; set; }
        
        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Column("scope")]
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
