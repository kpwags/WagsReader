using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WagsReaderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OAuthController : ControllerBase
    {
        private readonly ILogger<OAuthController> _logger;

        public OAuthController(ILogger<OAuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult AuthCallback()
        {
            return Redirect("com.kpwags.wagsreader://oauth2redirect");
        }
    }
}
