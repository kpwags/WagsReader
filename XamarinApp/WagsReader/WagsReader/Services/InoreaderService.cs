using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        IIdentityService IdentityService;
        IRequestProvider RequestProvider;

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

                var content = ApiUtilities.GetRequestContent(tokenRequest);

                var response = await RequestProvider.PostAsync<ApiResponse<UserToken>>($"{Constants.WagsReaderApiUri}/inoreader/getusertoken", content, null);

                if (response.Data == null)
                {
                    throw new Exception(response.ErrorMessage);
                }

                var userJson = await RequestProvider.GetAsync("https://www.inoreader.com/reader/api/0/user-info", response.Data.AccessToken);
                var user = JsonConvert.DeserializeObject<User>(userJson);

                user.Token = response.Data;

                await User.Save(user);
                
                return user;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error authenticating with Inoreader: {ex.Message}");
                return null;
            }
        }
    }
}
