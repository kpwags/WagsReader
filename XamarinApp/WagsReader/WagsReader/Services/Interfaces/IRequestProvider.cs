using System.Collections.Generic;
using System.Threading.Tasks;

namespace WagsReader.Services.Interfaces
{
    public interface IRequestProvider
    {
        Task<string> GetAsync(string uri, string token = "");
        Task<TResult> PostAsync<TResult>(string uri, Dictionary<string, string> data, string clientId = "", string clientSecret = "");
    }
}