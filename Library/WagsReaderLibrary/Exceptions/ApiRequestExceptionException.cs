using System;
using System.Net;
using System.Net.Http;

namespace WagsReaderLibrary.Exceptions
{
    public class ApiRequestExceptionException : HttpRequestException
    {
        public HttpStatusCode HttpCode { get; }
        public string Content { get; }

        public ApiRequestExceptionException(HttpStatusCode code)
            : this(code, null, null)
        {
        }

        public ApiRequestExceptionException(HttpStatusCode code, string message)
            : this(code, message, null, null)
        {
        }

        public ApiRequestExceptionException(HttpStatusCode code, string message, string content)
            : base(message, null)
        {
            HttpCode = code;
            Content = content;
        }

        public ApiRequestExceptionException(HttpStatusCode code, string message, string content, Exception inner)
            : base(message, inner)
        {
            HttpCode = code;
            Content = content;
        }
    }
}