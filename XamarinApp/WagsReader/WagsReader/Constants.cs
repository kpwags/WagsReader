using System;
using System.Collections.Generic;
using System.Text;

namespace WagsReader
{
    public static class Constants
    {
        public static string DatabaseName = "WagsReader.db";

        // WAGSREADER API
        public static string WagsReaderApiUri = "https://localhost:44393/api";

        // INOREADER
        public static string InoreaderAuthorityUri = "https://www.inoreader.com";
        public static string InoreaderAuthorizeUri = "https://www.inoreader.com/oauth2/auth";
        public static string InoreaderTokenUri = "https://www.inoreader.com/oauth2/token";
        public static string InoreaderRedirectUri = "https://localhost:44393/api/inoreader/authcallback";
        public static string InoreaderApiUri = "https://www.inoreader.com/reader/api/0/";
        public static string InoreaderClientId = "999999529";
        public static string InoreaderScope = "read write";
    }
}
