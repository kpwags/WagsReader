using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WagsReader.Models
{
    [Table("Folder")]
    public class Folder
    {
        [PrimaryKey, AutoIncrement]
        public int FolderId { get; set; }

        [OneToOne("UserId", "User", CascadeOperations = CascadeOperation.CascadeRead, ReadOnly = true)]
        public User User { get; set; }

        [ForeignKey(typeof(User))]
        public string UserId { get; set; }

        [JsonProperty("id")]
        public string ExternalId { get; set; }

        public string Name
        {
            get
            {
                if (ExternalId.Split('/').Length >= 4)
                {
                    return ExternalId.Split('/')[3];
                }

                return "";
            }
        }

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
