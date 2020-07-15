using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WagsReader.Models;
using WagsReader.Services.Interfaces;
using WagsReaderLibrary;
using WagsReaderLibrary.Inoreader.Models;
using WagsReaderLibrary.Interfaces;
using WagsReaderLibrary.Requests;

namespace WagsReader.Services
{
    public class InoreaderService : IRSSService
    {
        readonly IIdentityService IdentityService;
        readonly IRequestProvider RequestProvider;

        public InoreaderService()
        {
            RequestProvider = new RequestProvider();
            IdentityService = new InoreaderIdentityService();
        }

        public async Task<User> Login()
        {
            try
            {
                Classes.AuthRequest authRequest = IdentityService.CreateAuthorizationRequest();
                var authResponse = await Xamarin.Essentials.WebAuthenticator.AuthenticateAsync(new Uri(authRequest.Url), new Uri(Constants.InoreaderRedirectUri));

                string code = authResponse?.Properties["code"];
                string state = authResponse?.Properties["state"];

                if (state != authRequest.CSRFToken)
                {
                    throw new Exception("Response token does not match request token.");
                }

                var tokenRequest = new UserTokenRequest()
                {
                    code = code
                };

                var content = ApiUtilities.GetRequestContentAsJson(tokenRequest);

                var response = await RequestProvider.PostAsync<ApiResponse<WagsReaderLibrary.Inoreader.Models.UserToken>>($"{Constants.WagsReaderApiUri}/inoreader/getusertoken", content);

                if (response.Data == null)
                {
                    throw new Exception(response.ErrorMessage);
                }

                var userJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/user-info", response.Data.AccessToken);
                var user = JsonConvert.DeserializeObject<User>(userJson);

                user.AccountName = $"Inoreader ({user.Username})";
                user.Token = new Models.UserToken(response.Data);

                // get folders
                var folderJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/tag/list?types=1&counts=1", response.Data.AccessToken);
                user.Folders = GetFoldersFromJson(folderJson);

                await User.Save(user);

                // get feeds
                var feedJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/subscription/list", response.Data.AccessToken);
                await ProcessUserFeeds(feedJson, response.Data.AccessToken);

                System.Diagnostics.Debug.WriteLine($"Done Processing User");

                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error authenticating with Inoreader: {ex.Message}");
                return null;
            }
        }

        public async Task GetLatestFeedsForUser(User user)
        {
            // get folders
            var folderJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/tag/list?types=1&counts=1", user.Token.AccessToken);
            user.Folders = GetFoldersFromJson(folderJson);

            await User.Save(user);

            // get feeds
            var feedJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/subscription/list", user.Token.AccessToken);
            await ProcessUserFeeds(feedJson, user.Token.AccessToken);
        }

        protected List<Folder> GetFoldersFromJson(string folderJson)
        {
            var folderResponse = JsonConvert.DeserializeObject<FolderTagList>(folderJson);

            var folders = new List<Folder>();

            foreach (var f in folderResponse.Tags.Where(t => t.Type == "folder"))
            {
                folders.Add(new Folder { ExternalId = f.ExternalId, SortId = f.SortId, Type = f.Type, UnreadCount = f.UnreadCount, UnseenCount = f.UnseenCount });
            }

            return folders;
        }

        protected async Task ProcessUserFeeds(string feedJson, string accessToken)
        {
            var subscriptionResponse = JsonConvert.DeserializeObject<SubscriptionList>(feedJson);

            foreach (var sub in subscriptionResponse.Subscriptions)
            {
                var feed = await Feed.GetFeedBySubscriptionIdAsync(sub.SubscriptionId);

                if (feed == null)
                {
                    // brand new, save
                    // TODO: Update data?
                    feed = new Feed
                    {
                        SubscriptionId = sub.SubscriptionId,
                        Title = sub.Title,
                        SortId = sub.SortId,
                        FirstItemSec = sub.FirstItemSec,
                        Url = sub.Url,
                        HtmlUrl = sub.HtmlUrl,
                        IconUrl = sub.IconUrl
                    };

                    feed = await Feed.SaveAsync(feed);
                }

                foreach (SubscriptionCategory subscriptionCategory in sub.Categories)
                {
                    var folder = await Folder.GetFolderByExternalIdAsync(subscriptionCategory.Id);

                    var feedFolder = new FeedFolder
                    {
                        FeedId = feed.ID,
                        FolderId = folder.ID
                    };

                    FeedFolder.Save(feedFolder);
                }

                System.Diagnostics.Debug.WriteLine($"Processing Feed: {feed.Title}");
                string streamId = WebUtility.UrlEncode(feed.SubscriptionId);

                string apiEndpoint = $"https://www.inoreader.com/reader/api/0/stream/contents/{streamId}?n={Constants.MaxDownloadPerFeed}";

                if (feed.LastPullUSec > 0)
                {
                    apiEndpoint = $"{apiEndpoint}&ot={feed.LastPullUSec}";
                }

                var feedItemJson = await RequestProvider.GetAsync(apiEndpoint, accessToken);
                await ProcessFeedItems(feed, feedItemJson);

                await Feed.UpdateLastPullTime(feed.ID);
            }
        }

        protected async Task ProcessFeedItems(Feed feed, string feedItemJson)
        {
            var articleResponse = JsonConvert.DeserializeObject<StreamList>(feedItemJson);

            foreach (var item in articleResponse.Items.Where(i => i.Origin != null))
            {
                var feedItem = new FeedItem(item);

                feedItem.FeedId = feed.ID;
                feedItem.Feed = feed;
                await FeedItem.SaveAsync(feedItem);
            }
        }
    }
}
