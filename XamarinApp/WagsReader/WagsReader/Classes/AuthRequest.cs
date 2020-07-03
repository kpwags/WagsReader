using System;
using System.Collections.Generic;
using System.Text;

namespace WagsReader.Classes
{
    public class AuthRequest
    {
        public string Url { get; set; }
        public string CSRFToken { get; set; }
    }
}
