using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WagsReader.Services.Interfaces;
using WebAuthenticatorDemo.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WagsReader.Services
{
    public class InoreaderService : IRSSService
    {
        IIdentityService IdentityService;
        AuthorizeResponse AuthorizeResponse;

        public InoreaderService()
        {
            IdentityService = new IdentityService(new RequestProvider());
        }

        public async Task<object> Login()
        {
            string authUrl = IdentityService.CreateAuthorizationRequest();
            var authResult = await WebAuthenticator.AuthenticateAsync(new Uri(authUrl), new Uri(Constants.InoreaderRedirectUri));

            string raw = ParseAuthenticatorResult(authResult);
            AuthorizeResponse = new AuthorizeResponse(raw);

            return AuthorizeResponse;
        }

        private string ParseAuthenticatorResult(WebAuthenticatorResult result)
        {
            string code = result?.Properties["code"];
            string idToken = result?.IdToken;
            string scope = result?.Properties["scope"];
            string state = result?.Properties["state"];
            string sessionState = result?.Properties["session_state"];

            return $"{Constants.InoreaderRedirectUri}#code={code}&id_token={idToken}&scope={scope}&state={state}&session_state={sessionState}";
        }

    }
}
