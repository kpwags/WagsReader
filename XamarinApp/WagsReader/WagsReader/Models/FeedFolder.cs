using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System.Threading.Tasks;

namespace WagsReader.Models
{
    [Table("FeedFolder")]
    public class FeedFolder : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [ForeignKey(typeof(Feed))]
        [Column("feed_id")]
        public int FeedId { get; set; }

        [ForeignKey(typeof(Folder))]
        [Column("folder_id")]
        public int FolderId { get; set; }

        public static void Save(FeedFolder feedFolder)
        {
            try
            {
                if (_db.Find<FeedFolder>(f => f.FeedId == feedFolder.FeedId && f.FolderId == feedFolder.FolderId) == null)
                {
                    _db.RunInTransaction(() =>
                    {
                        _db.InsertWithChildren(feedFolder, recursive: true);
                    });
                }
            }
            catch (SQLiteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQLite Exception: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
            }
        }

    }
}
