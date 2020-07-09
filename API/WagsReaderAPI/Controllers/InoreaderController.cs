using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
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

        [HttpGet]
        public ActionResult AuthCallback()
        {
            return Redirect("com.kpwags.wagsreader://oauth2redirect");
        }

        [HttpPost("getusertoken")]
        public async Task<ActionResult<ApiResponse<UserToken>>> GetUserToken(UserTokenRequest req)
        {
            try
            {
                var requestProvider = new RequestProvider();
                var InoreaderTokenRequest = new AuthTokenRequest
                {
                    Code = req.code,
                    RedirectURL = WebUtility.UrlEncode(Utilities.AppSettings.InoreaderRedirectUri),
                    ClientId = Utilities.AppSettings.InoreaderAppID,
                    ClientSecret = Utilities.AppSettings.InoreaderAppKey
                };

                var content = new System.Net.Http.StringContent(JsonConvert.SerializeObject(InoreaderTokenRequest), System.Text.Encoding.UTF8, "application/json");//ApiUtilities.GetRequestContent(InoreaderTokenRequest);

                var token = await requestProvider.PostAsync<UserToken>(Utilities.AppSettings.InoreaderTokenUri, content);

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
                    Code = HttpStatusCode.Unauthorized,
                    ErrorContent = ex.Content,
                    ErrorMessage = ex.AuthErrorMessage,
                    ExceptionType = typeof(ServiceAuthenticationException)
                };
            }
            catch (ApiRequestExceptionException ex)
            {
                return new ApiResponse<UserToken>
                {
                    Data = null,
                    Code = ex.HttpCode,
                    ErrorContent = ex.Content,
                    ErrorMessage = ex.Message,
                    ExceptionType = typeof(ApiRequestExceptionException)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<UserToken>
                {
                    Data = null,
                    Code = HttpStatusCode.BadRequest,
                    ErrorMessage = ex.Message,
                    ExceptionType = typeof(Exception)
                };
            }
        }
    }
}
