using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WagsReader.Models;
using WagsReader.Services.Interfaces;
using WebAuthenticatorDemo.Services;
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

        public async Task<object> Login()
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

                var data = new Dictionary<string, string>();
                data.Add("code", code);

                var token = await RequestProvider.PostAsync<UserToken>($"{Constants.WagsReaderApiUri}/inoreader/getusertoken", data);
                
                return token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error authenticating with Inoreader: {ex.Message}");
                return null;
            }
        }
    }
}
