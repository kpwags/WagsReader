using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WagsReaderAPI.Classes
{
    public sealed class AppSettings
    {
        public string InoreaderAppID { get; set; }
        public string InoreaderAppKey { get; set; }
        public string InoreaderAuthorityUri { get; set; }
        public string InoreaderAuthorizeUri { get; set; }
        public string InoreaderBaseUri { get; set; }
        public string InoreaderTokenUri { get; set; }
        public string InoreaderRedirectUri { get; set; }
        public string InoreaderApiUri { get; set; }
        public string InoreaderScope { get; set; }
    }
}
