using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WagsReader.Models;
using WagsReader.Services.Interfaces;
using WagsReaderLibrary;
using WagsReaderLibrary.Interfaces;
using WagsReaderLibrary.Requests;
using Xamarin.Essentials;
using Xamarin.Forms;

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
                var authResponse = await WebAuthenticator.AuthenticateAsync(new Uri(authRequest.Url), new Uri(Constants.InoreaderRedirectUri));

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
                user.Token = new UserToken(response.Data);

                // get folders
                var folderJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/tag/list?types=1&counts=1", response.Data.AccessToken);
                user.Folders = GetFoldersFromJson(folderJson);                

                await User.Save(user);
                
                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error authenticating with Inoreader: {ex.Message}");
                return null;
            }
        }

        protected List<Folder> GetFoldersFromJson(string folderJson)
        {
            var folderResponse = JsonConvert.DeserializeObject<WagsReaderLibrary.Inoreader.Models.FolderTagList>(folderJson);

            var folders = new List<Folder>();

            foreach (var f in folderResponse.Tags.Where(t => t.Type == "folder"))
            {
                folders.Add(new Folder { ExternalId = f.ExternalId, SortId = f.SortId, Type = f.Type, UnreadCount = f.UnreadCount, UnseenCount = f.UnseenCount });
            }

            return folders;
        }
    }
}
