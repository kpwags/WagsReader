using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System;
using System.Threading.Tasks;

namespace WagsReader.Models
{
    [Table("FeedItem")]
    public class FeedItem : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [OneToOne("FeedId", "Feed", CascadeOperations = CascadeOperation.CascadeRead, ReadOnly = true)]
        public Feed Feed { get; set; }

        [ForeignKey(typeof(Feed))]
        [Column("feed_id")]
        public int FeedId { get; set; }

        [Column("stream_item_id")]
        public string StreamItemId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("publish_timestamp")]
        public long Published { get; set; }

        [Column("update_timestamp")]
        public long Updated { get; set; }

        [Column("url")]
        public string Url { get; set; }

        [Column("content_direction")]
        public string ContentDirection { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("source")]
        public string Source { get; set; }

        [Column("source_url")]
        public string SourceUrl { get; set; }

        public FeedItem()
        {

        }

        public FeedItem(WagsReaderLibrary.Inoreader.Models.StreamItem item)
        {
            StreamItemId = item.Id;
            Title = item.Title;
            Published = item.Published;
            Updated = item.Updated;

            if (item.Canonical.Count > 0)
            {
                Url = item.Canonical[0].href;
            }

            if (item.Summary != null)
            {
                ContentDirection = item.Summary.Direction;
                Content = item.Summary.Content;
            }

            Author = item.Author;

            if (item.Origin != null)
            {
                Source = item.Origin.Title;
                SourceUrl = item.Origin.HtmlUrl;
            }
        }

        public static async Task<FeedItem> GetFeedItemByIdAsync(int feedItemId, bool recursive = true)
        {
            return await Task.Run(() =>
            {
                FeedItem feedItem = _db.Table<FeedItem>().Where(f => f.ID == feedItemId).FirstOrDefault();

                if (feedItem != null)
                {
                    if (recursive)
                    {
                        feedItem = _db.FindWithChildren<FeedItem>(pk: feedItemId, recursive: true);
                    }
                }

                return feedItem;
            });
        }

        public static async Task<FeedItem> SaveAsync(FeedItem feedItem)
        {
            try
            {
                if (_db.Find<FeedItem>(f => f.StreamItemId == feedItem.StreamItemId) == null)
                {
                    _db.RunInTransaction(() =>
                    {
                        _db.InsertWithChildren(feedItem, recursive: true);
                    });

                    return await GetFeedItemByIdAsync(feedItem.FeedId, true);
                }
                else
                {
                    // user already exists, update
                    var existingFeedItem = await GetFeedItemByIdAsync(feedItem.FeedId, true);

                    existingFeedItem.Author = feedItem.Author;
                    existingFeedItem.Content = feedItem.Content;
                    existingFeedItem.ContentDirection = feedItem.ContentDirection;
                    existingFeedItem.Published = feedItem.Published;
                    existingFeedItem.Source = feedItem.Source;
                    existingFeedItem.SourceUrl = feedItem.SourceUrl;
                    existingFeedItem.Title = feedItem.Title;
                    existingFeedItem.Updated = feedItem.Updated;
                    existingFeedItem.Url = feedItem.Url;

                    _db.RunInTransaction(() =>
                    {
                        _db.Update(existingFeedItem);
                    });

                    return await GetFeedItemByIdAsync(feedItem.FeedId, true);
                }
            }
            catch (SQLiteException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQLite Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
    }
}
