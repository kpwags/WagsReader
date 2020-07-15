using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace WagsReader.Models
{
    [Table("Feed")]
    public class Feed : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public int ID { get; set; }

        [Column("subscription_id")]
        [JsonProperty("id")]
        public string SubscriptionId { get; set; }

        [Column("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Column("sort_id")]
        [JsonProperty("sortid")]
        public string SortId { get; set; }

        [Column("first_item_timestamp")]
        [JsonProperty("firstitemmsec")]
        public long FirstItemSec { get; set; }

        [Column("url")]
        [JsonProperty("url")]
        public string Url { get; set; }

        [Column("html_url")]
        [JsonProperty("htmlUrl")]
        public string HtmlUrl { get; set; }

        [Column("icon_url")]
        [JsonProperty("iconUrl")]
        public string IconUrl { get; set; }

        [Column("last_pull_timestamp")]
        public long LastPullUSec { get; set; }

        [ManyToMany(typeof(FeedFolder))]
        public List<Folder> Folders { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<FeedItem> Items { get; set; }

        public static async Task<Feed> GetFeedByIdAsync(int feedId, bool recursive = true)
        {
            return await Task.Run(() =>
            {
                Feed feed = _db.Table<Feed>().Where(f => f.ID == feedId).FirstOrDefault();

                if (feed != null)
                {
                    if (recursive)
                    {
                        feed = _db.FindWithChildren<Feed>(pk: feedId, recursive: true);
                    }
                }

                return feed;
            });
        }

        public static async Task<Feed> GetFeedBySubscriptionIdAsync(string subscriptionId)
        {
            return await Task.Run(() =>
            {
                Feed feed = _db.Table<Feed>().Where(f => f.SubscriptionId == subscriptionId).FirstOrDefault();

                if (feed != null)
                {
                    feed = _db.FindWithChildren<Feed>(pk: feed.ID, recursive: true);
                }

                return feed;
            });
        }

        public static async Task<Feed> SaveAsync(Feed feed)
        {
            try
            {
                if (_db.Find<Feed>(f => f.ID == feed.ID) == null)
                {
                    _db.RunInTransaction(() =>
                    {
                        _db.InsertWithChildren(feed, recursive: true);
                    });

                    return await GetFeedByIdAsync(feed.ID, true);
                }
                else
                {
                    // user already exists, update
                    var existingFeed = await GetFeedByIdAsync(feed.ID, true);

                    existingFeed.Folders = feed.Folders;
                    existingFeed.HtmlUrl = feed.HtmlUrl;
                    existingFeed.IconUrl = feed.IconUrl;
                    existingFeed.SortId = feed.SortId;
                    existingFeed.SubscriptionId = feed.SubscriptionId;
                    existingFeed.Title = feed.Title;
                    existingFeed.Url = feed.Url;

                    _db.RunInTransaction(() =>
                    {
                        _db.Update(existingFeed);
                    });

                    return await GetFeedByIdAsync(feed.ID, true);
                }
            }
            catch (SQLiteException ex)
            {
                WriteLine($"SQLite Exception: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

        public static async Task UpdateLastPullTime(int feedId)
        {
            long unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            var feed = await GetFeedByIdAsync(feedId, false);
            feed.LastPullUSec = unixTimestamp;

            _db.RunInTransaction(() =>
            {
                _db.Update(feed);
            });
        }
    }
}
