using System;
using System.Net;

namespace WagsReaderLibrary
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public HttpStatusCode Code { get; set; } = HttpStatusCode.OK;
        public string ErrorMessage { get; set; } = "";
        public string ErrorContent { get; set; } = "{}";
        public Type ExceptionType { get; set; }
    }
}
