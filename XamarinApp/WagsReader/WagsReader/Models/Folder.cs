using System.Collections.Generic;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Threading.Tasks;
using SQLiteNetExtensions.Extensions;
using System.Linq;

namespace WagsReader.Models
{
    [Table("Folder")]
    public class Folder : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [OneToOne("UserId", "User", CascadeOperations = CascadeOperation.CascadeRead, ReadOnly = true)]
        public User User { get; set; }

        [ForeignKey(typeof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("external_id")]
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

        [Column("sort_id")]
        public string SortId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("unread_count")]
        public int UnreadCount { get; set; }

        [Column("unseen_count")]
        public int UnseenCount { get; set; }

        [ManyToMany(typeof(FeedFolder))]
        public List<Feed> Feeds { get; set; }

        public static async Task<Folder> GetFolderByExternalIdAsync(string externalId, bool recursive = true)
        {
            return await Task.Run(() =>
            {
                Folder folder = _db.Table<Folder>().Where(f => f.ExternalId == externalId).FirstOrDefault();

                if (folder != null)
                {
                    if (recursive)
                    {
                        folder = _db.FindWithChildren<Folder>(pk: folder.ID, recursive: true);
                    }
                }

                return folder;
            });
        }

        public static long GetMostRecentUpdateTime(int folderId)
        {
            var folder = _db.FindWithChildren<Folder>(pk: folderId, recursive: true);

            if (folder.Feeds.Count > 0)
            {
                return folder.Feeds.OrderByDescending(f => f.FirstItemSec).FirstOrDefault().FirstItemSec;
            }

            return 0;
        }
    }
}
