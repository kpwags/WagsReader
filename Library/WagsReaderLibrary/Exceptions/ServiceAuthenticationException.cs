using System;

namespace WagsReaderLibrary.Exceptions
{
    public class ServiceAuthenticationException : Exception
    {
        public string Content { get; }
        public string AuthErrorMessage { get; }

        public ServiceAuthenticationException()
        {
        }

        public ServiceAuthenticationException(string content, string msg = "")
        {
            Content = content;
            AuthErrorMessage = msg;
        }
    }
}
