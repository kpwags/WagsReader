using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WagsReaderAPI.Classes;
using WagsReaderAPI.Services;
using WagsReaderLibrary;
using WagsReaderLibrary.Exceptions;
using WagsReaderLibrary.Inoreader.Models;
using WagsReaderLibrary.Inoreader.Requests;
using WagsReaderLibrary.Requests;

namespace WagsReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InoreaderController : ControllerBase
    {
        private readonly ILogger<InoreaderController> _logger;

        public InoreaderController(ILogger<InoreaderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("authcallback")]
        public ActionResult AuthCallback()
        {
            return Redirect("com.kpwags.wagsreader://oauth2redirect");
        }

        [HttpPost("getusertoken")]
        public async Task<ActionResult<ApiResponse<UserToken>>> GetUserToken(UserTokenRequest req)
        {
            try
            {
                var requestProvider = new RequestProvider(Utilities.AppSettings.InoreaderBaseUri);
                var InoreaderTokenRequest = new AuthTokenRequest
                {
                    Code = req.code,
                    RedirectURL = WebUtility.UrlEncode(Utilities.AppSettings.InoreaderRedirectUri),
                    ClientId = Utilities.AppSettings.InoreaderAppID,
                    ClientSecret = Utilities.AppSettings.InoreaderAppKey
                };

                var content = ApiUtilities.GetRequestContentAsDictionary(InoreaderTokenRequest);

                string contentSerialized = await content.ReadAsStringAsync();
                Debug.WriteLine($"Request Content: {contentSerialized}");

                var token = await requestProvider.PostAsync<UserToken>(
                    uri: Utilities.AppSettings.InoreaderTokenUri,
                    content: content
                );

                var resp = new ApiResponse<UserToken>
                {
                    Data = token
                };

                return resp;
            }
            catch (ServiceAuthenticationException ex)
            {
                return new ApiResponse<UserToken>
                {
                    Data = null,
                    ErrorMessage = ex.AuthErrorMessage
                };
            }
            catch (ApiRequestExceptionException ex)
            {
                return new ApiResponse<UserToken>
                {
                    Data = null,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserToken>
                {
                    Data = null,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
