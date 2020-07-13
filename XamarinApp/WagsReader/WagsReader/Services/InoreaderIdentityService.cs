using IdentityModel.Client;
using System;
using System.Collections.Generic;
using WagsReader.Classes;
using WagsReader.Services.Interfaces;

namespace WagsReader.Services
{
    public class InoreaderIdentityService : IIdentityService
    {
        public InoreaderIdentityService()
        {

        }

        public AuthRequest CreateAuthorizationRequest()
        {
            // Create URI to authorization endpoint
            var authorizeRequest = new RequestUrl(Constants.InoreaderAuthorizeUri);

            // Dictionary with values for the authorize request
            var dic = new Dictionary<string, string>();
            dic.Add("client_id", Constants.InoreaderClientId);
            dic.Add("response_type", "code");
            dic.Add("scope", Constants.InoreaderScope);
            dic.Add("redirect_uri", Constants.InoreaderRedirectUri);

            // Add CSRF token to protect against cross-site request forgery attacks.
            var currentCSRFToken = Guid.NewGuid().ToString("N");
            dic.Add("state", currentCSRFToken);

            var authorizeUri = authorizeRequest.Create(dic);

            var authRequest = new AuthRequest
            {
                Url = authorizeUri,
                CSRFToken = currentCSRFToken
            };

            return authRequest;
        }
    }
}
