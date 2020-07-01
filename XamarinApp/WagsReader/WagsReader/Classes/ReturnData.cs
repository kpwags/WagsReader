using System;
using System.Collections.Generic;
using System.Text;

namespace WagsReader.Classes
{
    public class ReturnData
    {
        public CallStatus Status { get; set; }
        public string Data { get; set; } = "";
        public List<string> Messages { get; set; } = new List<string>();

        public enum CallStatus
        {
            Ok,
            Error
        }
    }
}
