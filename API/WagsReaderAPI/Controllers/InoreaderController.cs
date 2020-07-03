using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WagsReaderAPI.Classes;
using WagsReaderAPI.Models;
using WagsReaderAPI.Requests;
using WagsReaderAPI.Services;

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

        [HttpPost]
        public async Task<ActionResult<UserToken>> GetUserToken(UserTokenRequest req)
        {
            var requestProvider = new RequestProvider();

            string data = string.Format("code={0}&redirect_uri={1}&client_id={2}&client_secret={3}&scope={4}&grant_type=authorization_code", 
                                            req.code,
                                            WebUtility.UrlEncode(Utilities.AppSettings.InoreaderRedirectUri), 
                                            Utilities.AppSettings.InoreaderAppID, 
                                            Utilities.AppSettings.InoreaderAppKey,
                                            WebUtility.UrlEncode(Utilities.AppSettings.InoreaderScope));

            var token = await requestProvider.PostAsync<UserToken>(Utilities.AppSettings.InoreaderTokenUri, data, "", "");
            //return token;
            return token;
        }
    }
}
