using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WagsReaderLibrary.Interfaces
{
    public interface IRequestProvider
    {
        Task<string> GetAsync(string uri, string token = "");
        Task<TResult> PostAsync<TResult>(string uri, HttpContent data, string contentType = "application/x-www-form-urlencoded", string clientId = "", string clientSecret = "");
    }
}
